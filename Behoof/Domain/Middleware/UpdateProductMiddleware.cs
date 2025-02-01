using Behoof.Domain.Entity.Context;
using Behoof.Domain.Parsing;

namespace Behoof.Domain.Middleware
{
    public class UpdateProductMiddleware
    {
        private readonly RequestDelegate next;
        private ParsingSites Parsing;
        private static bool isTaskRunning = false;
        public UpdateProductMiddleware(RequestDelegate next, ParsingSites parsing)
        {
            this.next = next;
            Parsing = parsing;
        }

        public async Task Invoke(HttpContext context)
        {
            
        }
    }
}
