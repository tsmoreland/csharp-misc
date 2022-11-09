//
// Copyright (c) 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Microsoft.AspNetCore.Mvc;
using TSMoreland.Authorization.Demo.Middleware.Abstractions;

namespace TSMoreland.Authorization.Demo.App.Controllers
{
    // deepcode ignore AntiforgeryTokenDisabled: fall through error catch, no post/put data will be used at this point
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IErrorResponseProvider _errorProvider;

        public ErrorController(IErrorResponseProvider errorProvider)
        {
            _errorProvider = errorProvider ?? throw new ArgumentNullException(nameof(errorProvider));
        }

        [HttpGet]
        [Route("/error/401")]
        public IActionResult ErrorUnauthorized()
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        [HttpGet]
        [Route("/error/403")]
        public IActionResult ErrorForbidden()
        {
            return Problem(statusCode: StatusCodes.Status403Forbidden);
        }

        [HttpGet]
        [Route("/error/404")]
        public IActionResult ErrorNotFound()
        {
            return Problem(statusCode: StatusCodes.Status404NotFound);
        }

        [HttpGet]
        [Route("/error/{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            return Problem(statusCode: statusCode);
        }

        // deepcode ignore AntiforgeryTokenDisabled: fall through error catch, no post/put data will be used at this point
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpPatch]
        [HttpDelete]
        [HttpOptions]
        [HttpHead]
        [IgnoreAntiforgeryToken]
        [Route("/error")]
        public ValueTask<IActionResult> Error()
        {
            return _errorProvider.BuildResponse(this);
        }
    }
}
