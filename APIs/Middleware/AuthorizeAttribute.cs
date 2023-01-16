using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Middleware
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = (string)context.HttpContext.Items["Token"];
            if (token == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                //- Check for Token expiration Date
                long tokenExpiry = (long)context.HttpContext.Items["TokenExpirationDateInSeconds"];
                long currentTimeInSeconds = (long)context.HttpContext.Items["CurrentTimeInSconds"];
                if (currentTimeInSeconds > tokenExpiry)
                {
                    context.Result = new JsonResult(new { message = "Forbidden...you're using an expired token" }) { StatusCode = StatusCodes.Status403Forbidden }; ;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        public readonly string _role;
        public readonly string[] _roles;

        public AuthorizeRoleAttribute(string role)
        {
            _role = role;
        }

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = (string)context.HttpContext.Items["Token"];
            //string token = context.HttpContext.Request.Headers["Authorization"];
            string role = (string)context.HttpContext.Items["Role"];
            if (token == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                //- Check for Token expiration Date
                long tokenExpiry = (long)context.HttpContext.Items["TokenExpirationDateInSeconds"];
                long currentTimeInSeconds = (long)context.HttpContext.Items["CurrentTimeInSconds"];
                if (currentTimeInSeconds > tokenExpiry)
                {
                    context.Result = new JsonResult(new { message = "Forbidden...you're using an expired token" }) { StatusCode = StatusCodes.Status403Forbidden }; ;
                }
                if (!string.IsNullOrEmpty(_role))
                {
                    if (_role != role)
                    {
                        string roleInText = GetRoleInText(_role);
                        string message = string.Format("Unauthorized access. Only '{0}(s)' can access this route(s)", roleInText);
                        context.Result = new JsonResult(new { message }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }

                //- Check for multiple roles
                if (_roles?.Length > 0)
                {
                    //- Convert roles to list of easy use
                    List<string> rolesToList = new List<string>(_roles);
                    if (!rolesToList.Contains(role))
                    {
                        List<string> rolesInText = GetRolesInText(_roles);
                        string message = string.Format("Unauthorized access. Only '{0} role(s)' can access this route(s)", string.Join(", ", rolesInText));
                        context.Result = new JsonResult(new { message }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
            }
        }

        private List<string> GetRolesInText(string[] rolesToBeTranslated)
        {
            List<string> translatedRoles = new List<string>();
            for (int i = 0; i < rolesToBeTranslated.Length; i++)
            {
                string roleInText = GetRoleInText(rolesToBeTranslated[i]);
                translatedRoles.Add(roleInText);
            }
            return translatedRoles;
        }

        private string GetRoleInText(string role)
        {
            string roleInText;
            switch (int.Parse(role))
            {
                case 1:
                    roleInText = "superadmin";
                    break;
                case 2:
                    roleInText = "schooladmin";
                    break;
                case 3:
                    roleInText = "instructor";
                    break;
                case 4:
                    roleInText = "departmentadministrator";
                    break;
                case 5:
                    roleInText = "student";
                    break;
                default:
                    roleInText = string.Empty;
                    break;
            }
            return roleInText;
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeApiAttribute : Attribute, IAuthorizationFilter
    {
        public readonly string _apiCall;

        public AuthorizeApiAttribute(string apiCall)
        {
            _apiCall = apiCall;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = (string)context.HttpContext.Items["API-TOKEN"];
            string apiCall = (string)context.HttpContext.Items["InstitutionPaymentApiCall"];
            if (token == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {

                if (!string.IsNullOrEmpty(_apiCall))
                {
                    if (_apiCall != apiCall)
                    {
                        string message = string.Format("Unauthorized access. Only '{0}(s)' can access this route(s)", _apiCall);
                        context.Result = new JsonResult(new { message }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
            }
        }

    }
}
