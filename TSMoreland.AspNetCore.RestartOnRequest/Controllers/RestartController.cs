//
// Copyright © 2022 Terry Moreland
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

namespace TSMoreland.AspNetCore.RestartOnRequest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestartController : ControllerBase
{
    private readonly IHostApplicationLifetime _applicationLifetime;

    public RestartController(IHostApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
    }

    [HttpGet]
    [Route("~/restart_server")]
    public IActionResult Restart()
    {
        // this just outrights stops the application (after the response is returned)
        // if running as a service configured to restart and/or a watch dog process exists then this could be a first
        // step in restarting but we'd lose everything we have right now (any singletons, ... would be disposed and re-created)
        _applicationLifetime.StopApplication();

        return Ok();
    }
}
