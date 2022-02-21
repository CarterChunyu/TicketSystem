using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using AutoMapper;
using TicketSystem.ViewModels;
using TicketSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace TicketSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;


        public RoleController(RoleService roleService,IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        // GET: Roles
        public IActionResult Index()
        {
            IEnumerable<Role> roles = _roleService.GetAllRoles();
            IEnumerable<RoleShowVM> roleShowVMs
                = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleShowVM>>(roles);
            return View(roleShowVMs);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleShowVM roleShowVM = _mapper.Map<Role, RoleShowVM>(role); 
            return View(roleShowVM);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")]RoleCreateVM roleCreateVM)
        {
            if (ModelState.IsValid)
            {
                Role role = _mapper.Map<RoleCreateVM, Role>(roleCreateVM);
                await _roleService.AddRoleAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(roleCreateVM);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var role = await _roleService.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleEditVM roleEditVM = _mapper.Map<Role, RoleEditVM>(role);
            return View(roleEditVM);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] RoleEditVM roleEditVM)
        {
            if (id != roleEditVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Role role = _mapper.Map<RoleEditVM, Role>(roleEditVM);
                    await _roleService.UpdateRoleAsync(role);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _roleService.RoleExistedAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roleEditVM);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var role =  await _roleService.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleShowVM roleShowVM = _mapper.Map<Role, RoleShowVM>(role);
            return View(roleShowVM);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Role role = await _roleService.GetRoleAsync(id);
            await _roleService.RemoveRoleAsync(role);
            return RedirectToAction(nameof(Index));
        }      
    }
}
