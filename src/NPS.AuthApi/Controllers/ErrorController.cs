using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NPS.AuthApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
         [Route("/errordevelopment",Name ="errordevelopment")]
         public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
         {
             if (!hostEnvironment.IsDevelopment())
             {
                 return NotFound();
             }
         
             var exceptionHandlerFeature =
                 HttpContext.Features.Get<IExceptionHandlerFeature>()!;
         
             return Problem(
                 detail: exceptionHandlerFeature.Error.StackTrace,
                 title: exceptionHandlerFeature.Error.Message);
         }

        //[Route("error")]
        //public IActionResult HandleError() => Problem();
    }
}