using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TicketSystem.FIlters
{
    public class SystemFilter : IExceptionFilter
    {
        // 為了使用ViewData
        private readonly IModelMetadataProvider _provider;
        public SystemFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _provider = modelMetadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            string ErrotMessage = context.Exception.ToString();

            ViewResult result = new ViewResult() { ViewName = "ErrorPage" };

            result.ViewData = new ViewDataDictionary(_provider, context.ModelState);
#if DEBUG
            result.ViewData.Add("ErrorMsg", ErrotMessage);

#else
            result.ViewData.Add("ErrorMsg", context.Exception.Message);
#endif
            context.Result = result;
        }
    }
}
