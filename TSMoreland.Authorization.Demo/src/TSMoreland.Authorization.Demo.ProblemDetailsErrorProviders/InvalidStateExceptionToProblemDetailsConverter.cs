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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TSMoreland.Authorization.Demo.Middleware.Abstractions;

namespace TSMoreland.Authorization.Demo.ProblemDetailsErrorProviders;

public sealed class InvalidStateExceptionToProblemDetailsConverter : IExceptionToProblemDetailsConverter<InvalidModelStateException>
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public InvalidStateExceptionToProblemDetailsConverter(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    /// <inheritdoc />
    public ProblemDetails Convert(HttpContext context, InvalidModelStateException ex)
    {
        string details = string.Join(", ", ex.ModelState
            .Values
            .SelectMany(e => e.Errors)
            .Select(e => e.ErrorMessage));

        return _problemDetailsFactory
            .CreateValidationProblemDetails(
                context,
                ex.ModelState,
                StatusCodes.Status422UnprocessableEntity,
                detail: details);
    }

    /// <inheritdoc />
    public ProblemDetails Convert(HttpContext context, Exception ex)
    {
        if (ex is InvalidModelStateException invalidModelStateException)
        {
            return Convert(context, invalidModelStateException);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
