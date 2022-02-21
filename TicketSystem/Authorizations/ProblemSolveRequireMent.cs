using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Services;

namespace TicketSystem.Authorizations
{
    public class ProblemSolveRequireMent:IAuthorizationRequirement
    {
    }
    public class ProblemSolveHandler : AuthorizationHandler<ProblemSolveRequireMent>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ProblemCatrgoryService _problemCatrgoryService;
        public ProblemSolveHandler(IHttpContextAccessor accessor, ProblemCatrgoryService service)
        {
            _accessor = accessor;
            _problemCatrgoryService = service;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProblemSolveRequireMent requirement)
        {
            string errorMessage = string.Empty; //ProblemCategoryId
            HttpRequest request = _accessor.HttpContext.Request;          
            int problemCategoryId = int.Parse(request.Form.FirstOrDefault(p => p.Key == "ProblemCategoryId").Value);
            string categoryName = (await _problemCatrgoryService.
                GetProblemCategorybyId(problemCategoryId)).Name;
            if(categoryName.ToUpper()== "Test Case".ToUpper())
            {
                if (context.User.IsInRole("Qa"))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    errorMessage = $"{categoryName}類型的錯誤只有QA可以關閉";
                }
            }
            else
            {
                if(context.User.IsInRole("Rd"))
                {
                    context.Succeed(requirement);
                    return;
                }
                errorMessage = $"只有RD可以解決{categoryName}類型的問題";
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
