using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Services;
using TicketSystem.ViewModels;
using TicketSystem.Models;
using AutoMapper;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TicketSystem.Controllers
{   
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly LoginService _loginService;

        public UserController(IHttpContextAccessor accessor,IMapper mapper,
            UserService userService,RoleService roleService,LoginService loginService)
        {
            _accessor = accessor;
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
            _loginService = loginService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            IEnumerable<User> users =_userService.GetAllUSers();
            IEnumerable<UserShowVM> userShowVMs = _mapper.
                Map<IEnumerable<User>,IEnumerable<UserShowVM>>(users);
            return View(userShowVMs);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["RoleId"] = new SelectList(_roleService.GetAllRoles(), "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM userCreateVM)
        {
            if (ModelState.IsValid)
            {                
                User user = _mapper.Map<UserCreateVM, User>(userCreateVM);
                await _userService.AddUserAsync(user);
                return RedirectToAction("Index");
            }
            ViewData["RoleId"] = new SelectList(_roleService.GetAllRoles(), "Id", "Name");
            return View(userCreateVM);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(loginVM);
            User user =await _userService.GetUserByAccountPasswordAsync(loginVM.Account, loginVM.Password);
           
            if(user==null)
            {
                ModelState.AddModelError("DenialReason", "查無此人");
                return View(loginVM);
            }
            // 先登出
            await _loginService.SignOutAsync();

            //// 用Session記錄使用者
            //LoginStateVM loginStateVM = _mapper.Map<User, LoginStateVM>(user);
            //_loginService.SetUserInFo(loginStateVM);

            // todo... 設置權限
            await _loginService.SetClaims(user.Account,user.Role.Name);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Profile");
        }
        [Authorize()]
        public async Task<IActionResult>Profile()
        {
            string account= _accessor.HttpContext.User.Claims.
                FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
            User user = await _userService.GetUserByAccountAsync(account);
            UserShowVM userShowVM = _mapper.Map<User, UserShowVM>(user);
            return View(userShowVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditProfile(int id)
        {
            //if (id != _loginService.GetUserInfo<LoginStateVM>().Id)
            //    return NotFound();
            User user=  await _userService.GetUserByIdAsync(id);
            UserEditVM userEditVM = _mapper.Map<User, UserEditVM>(user);
            ViewData["RoleId"] = new SelectList(_roleService.GetAllRoles(), "Id", "Name", userEditVM.RoleId);
            return View(userEditVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditVM userEditVM)
        {
            if (ModelState.IsValid)
            {
                User user = _mapper.Map<UserEditVM, User>(userEditVM);
                await _userService.UpdateUserAsync(user);

                // 取有Include Role 的User
                User userRole = await _userService.GetUserByIdAsync(user.Id);
                //todo... 直接登出
                //await _loginService.SignOutAsync();
                //await _loginService.SetClaims(user.Name,userRole.Role.Name);

                //LoginStateVM loginStateVM = _mapper.Map<User, LoginStateVM>(userRole);
                //_loginService.SetUserInFo(loginStateVM);

                return RedirectToAction("Profile");
            }
            ViewData["RoleId"] = new SelectList(_roleService.GetAllRoles(), "Id", "Name", userEditVM.RoleId);
            return View(userEditVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {           
            User user = await _userService.GetUserByIdAsync(id);
            UserShowVM userShowVM = _mapper.Map<User, UserShowVM>(user);
            return View(userShowVM);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ManagePassword(int id)
        {           
            UserPasswordVM userPasswordVM = new UserPasswordVM() { Id = id };
            return View(userPasswordVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManagePassword(UserPasswordVM userPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                User user = await _userService.GetUserByIdAsync(userPasswordVM.Id);
                user = _mapper.Map<UserPasswordVM, User>(userPasswordVM, user);
                await _userService.UpdateUserAsync(user);
                return RedirectToAction("Login");
            }
            return View(userPasswordVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserShowVM userShowVM = _mapper.Map<User, UserShowVM>(user);
            return View(userShowVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = await _userService.GetUserByIdAsync(id);
            await _userService.RemoveUserAsync(user);
            //todo...  移除權限跟登出系統
            //_loginService.RemoveUserInfo();
            await _loginService.SignOutAsync();
            return RedirectToAction("Register");
        }

        public async Task<IActionResult> Logout()
        {
            //_loginService.RemoveUserInfo();
            await _loginService.SignOutAsync();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public IActionResult NoPermission()
        {
            return View();
        }
        [AllowAnonymous]
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsAccountExisted(string Account)
        {
            if (await _userService.IsAccountExistedAsync(Account))
                return Json($"Account: {Account} is already in use");            
            return Json(true);
        }
        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPasswordCorrect(int id,string CurrentPassword)
        {
            if (!await _userService.IsPasswordCorrect(id, CurrentPassword))
                return Json($"CurrentPassword: {CurrentPassword} is wrong");
            return Json(true);
        }
    }
}
