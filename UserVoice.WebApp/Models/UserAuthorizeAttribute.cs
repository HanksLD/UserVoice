using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserVoice.WebApp.Models
{
    /// <summary>
    /// 跳过检查属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SkipUserAuthorizeAttribute : Attribute, IFilterMetadata
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public UserAuthorizeAttribute()
        {
            this.AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
        }
        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var authenticate = filterContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authenticate.Result.Succeeded || this.SkipUserAuthorize(filterContext.ActionDescriptor))
            {
                return;
            }
            HttpRequest httpRequest = filterContext.HttpContext.Request;
            string url = "/Account/Login";
            url = string.Concat(url, "?returnUrl=", httpRequest.Path);
            RedirectResult redirectResult = new RedirectResult(url);
            filterContext.Result = redirectResult;
            return;
        }

        protected virtual bool SkipUserAuthorize(ActionDescriptor actionDescriptor)
        {
            bool skipAuthorize = actionDescriptor.FilterDescriptors.Where(a => a.Filter is SkipUserAuthorizeAttribute).Any();
            if (skipAuthorize)
            {
                return true;
            }
            return false;
        }
    }
}
