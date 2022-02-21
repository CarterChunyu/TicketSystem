using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;
using TicketSystem.ViewModels;
using TicketSystem.Helpers;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketSystem.Services
{
    public class LoginService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly string _sessionKey;
        public LoginService(IHttpContextAccessor accessor,IConfiguration config)
        {     
            _accessor = accessor;
            _config = config;
            _sessionKey = _config.GetSection("LginSetting")["SessionKey"];
        }
        //// set使用者資訊        
        //public void SetUserInFo(LoginStateVM loginStateVM)
        //{
        //    _accessor.HttpContext.Session.SetObject(_sessionKey, loginStateVM);            
        //}
        //// get使用者資訊
        //public T GetUserInfo<T>() where T : LoginStateVM
        //{
        //    return _accessor.HttpContext.Session.GetObject<T>(_sessionKey);
        //}
        //// 移除Session
        //public void RemoveUserInfo()
        //{
        //    _accessor.HttpContext.Session.Remove(_sessionKey);
        //}
        // 設置權限
        public async Task SetClaims(string account,string roleName)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account));
            claims.Add(new Claim(ClaimTypes.UserData, roleName));
            AuthorizeObj obj = new AuthorizeObj();
            _config.GetSection("AuthorizeSetting").Bind(obj);
            string value = string.Empty;
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                if (info.Name.ToUpper() == roleName.ToUpper())
                {
                    value = (string)info.GetValue(obj);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(value))
            {
                foreach (string item in value.Split(','))
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            await _accessor.HttpContext.SignInAsync(principal, new AuthenticationProperties()
            {
                IsPersistent = false
            });       
        }
        
            
        // 登出移除權限
        public async Task SignOutAsync()
        {
           await _accessor.HttpContext.SignOutAsync();
        }
    }
}
