using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace TicketSystem.Authorizations
{
    public class ProblemCreateRequireMent:IAuthorizationRequirement
    {
    }
    public class ProblemCreateHandler : AuthorizationHandler<ProblemCreateRequireMent>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ProblemCatrgoryService _problemCatrgoryService;
        public ProblemCreateHandler(IHttpContextAccessor accessor,ProblemCatrgoryService service)
        {
            _accessor = accessor;
            _problemCatrgoryService=service;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProblemCreateRequireMent requirement)
        {
            string errorMessage = string.Empty;

            int problemCategoryId = int.Parse(_accessor.HttpContext.Request.Form.
                FirstOrDefault(p => p.Key == "ProblemCategoryId").Value);
            string categoryName = (await _problemCatrgoryService.
                GetProblemCategorybyId(problemCategoryId)).Name;
            if (categoryName.ToUpper() == "Feature Request".ToUpper())
            {
                if (context.User.IsInRole("Pm"))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                    errorMessage = $"只有PM能新增{categoryName}的問題";    
            }
            else
            {
                if (context.User.IsInRole("Qa"))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                    errorMessage = $"只有QA能新增{categoryName}的問題";
            }           
            HttpResponse response = _accessor.HttpContext.Response;
            byte[] bytes = Encoding.UTF8.GetBytes(errorMessage);
            response.StatusCode = 405;
            response.ContentType = "application/json";
            await response.Body.WriteAsync(bytes, 0, bytes.Length);
            return;
        }
    }
}
