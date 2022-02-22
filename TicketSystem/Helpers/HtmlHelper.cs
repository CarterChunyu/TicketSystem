using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Helpers
{
    public static class HtmlHelper
    {

        public static string WriteHtml(this string source)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine(@"<meta charset=""utf-8"">");
            builder.AppendLine("<title> Error Message</title>");
            builder.AppendLine("<head>");
            builder.AppendLine(@"<body><h3><span>");
            builder.AppendLine(source);
            builder.AppendLine("</span></h3>");
            builder.AppendLine(@"<div><a href=""/User/Logout"">登出後重新登入</a></div>");
            builder.AppendLine(@"<div><a href=""/Problem/Index"">回問題列表</a></div>");
            builder.AppendLine("</body></html>");
            return builder.ToString();
        }
    }
}
