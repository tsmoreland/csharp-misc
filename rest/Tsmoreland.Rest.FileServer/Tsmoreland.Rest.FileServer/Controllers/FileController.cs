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

namespace Tsmoreland.Rest.FileServer.Controllers;

[ApiController]
[Route("/api/file")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    /// <inheritdoc />
    public FileController(ILogger<FileController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpPost]
    public IActionResult Upload([FromForm(Name = "upload_file")] IFormFile file)
    {
        string authorization = HttpContext.Request.Headers.Authorization.ToString();

        _logger.LogInformation("Authorization {Authorization}", authorization);
        _logger.LogInformation("Receving file {Filename}", file.FileName);


        return File(file.OpenReadStream(), "application/octet-stream"); 
    }
}
