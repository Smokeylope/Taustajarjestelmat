using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Layered
{
    public class AuditFilter : ActionFilterAttribute
    {
        private readonly IRepository _repository;

        public AuditFilter(IRepository repository)
        {
            _repository = repository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _repository.AuditDeleteStarted();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _repository.AuditDeleteSuccess();
        }
    }
}