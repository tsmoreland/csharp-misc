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
    
using MediatR;
using Microsoft.Extensions.Logging;

namespace TSMoreland.Messaging.Demo.App;

public sealed class PingHandler : IRequestHandler<Ping, Message>
{
    private readonly ILogger<PingHandler> _logger;

    public PingHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PingHandler>();
    }

    /// <inheritdoc />
    public Task<Message> Handle(Ping request, CancellationToken cancellationToken)
    {
        // thread id will match send call because the mediation is handled on the same thread, while we can use async/await we will start on the
        // sender thread
        _logger.LogInformation("Handle Ping on {Thread}", Environment.CurrentManagedThreadId);
        return Task.FromResult(request.Response);
    }
}
