using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.MVC.Filters
{
    public class MovieStoreFilter :  ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // execute after an action method executes


        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var data = context.HttpContext.Request.Path;
            var someotherData = context.HttpContext.User.Identity.IsAuthenticated;

            // we can log this information in our text files or database
            // we wanna track the information of how many people came to the Movie Details Page
            // will execute this method before and action method executes
        }
    }
}
