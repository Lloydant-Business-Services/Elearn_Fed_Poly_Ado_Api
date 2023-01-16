//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace BusinessLayer.Middlewares
//{

//    public class ErrorHandlerMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public ErrorHandlerMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception error)
//            {
//                logger.LogError(error.ToString());
//                var response = context.Response;
//                response.ContentType = "application/json";

//                switch (error)
//                {
//                    case AppException:
//                        // custom application error
//                        response.StatusCode = (int)HttpStatusCode.BadRequest;
//                        break;
//                    case KeyNotFoundException:
//                        // not found error
//                        response.StatusCode = (int)HttpStatusCode.NotFound;
//                        break;
//                    //case UnauthorizedAccessException:
//                    //    // not authorized error
//                    //    response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                    //    break;
//                    //case ArgumentNullException:
//                    //    // null error
//                    //    response.StatusCode = (int)HttpStatusCode.BadRequest;
//                    //    break;
//                    default:
//                        // unhandled error
//                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                        break;
//                }

//                var result = JsonSerializer.Serialize(new { message = error?.Message });
//                await response.WriteAsync(result);

//            }
//        }
//    }
//}
