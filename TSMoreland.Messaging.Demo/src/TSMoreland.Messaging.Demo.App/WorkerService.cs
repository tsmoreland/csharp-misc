﻿//
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

using MassTransit;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TSMoreland.Messaging.Demo.App;

public sealed class WorkerService : IHostedService, IDisposable
{
    private readonly IBus _bus;
    private readonly IMediator _mediator;
    private readonly IRequestClient<RequestMessage> _requestClient;
    private readonly Timer _timer;
    private readonly ILogger<WorkerService> _logger;

    public WorkerService(
        IBus bus,
        IMediator mediator,
        IRequestClient<RequestMessage> requestClient,
        ILoggerFactory loggerFactory)
    {
        _bus = bus;
        _mediator = mediator;
        _requestClient = requestClient;
        _logger = loggerFactory.CreateLogger<WorkerService>();
        _timer = new Timer(Callback, this, Timeout.Infinite, Timeout.Infinite);
    }

    private static void Callback(object? state)
    {
        WorkerService service = (WorkerService)state!;
        _ = service.Callback();
    }

    private async Task Callback()
    {
        _logger.LogInformation("Publish message");

        try
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            RequestMessage requestMessage = new(Guid.NewGuid());

            Response<ResponseMessage> response = await _requestClient.GetResponse<ResponseMessage>(requestMessage, CancellationToken.None);

            QueueMessage queueMessage = new(response.Message.Content);
            ISendEndpoint endpoint = await _bus.GetSendEndpoint(new Uri("queue:queue1"));
            await endpoint.Send(queueMessage);
    
            Message message = new(response.Message.Content);

            Task<Message> reflectedTask = _mediator.Send(new Ping(message), CancellationToken.None);
            Task busTask = _bus.Publish(message, CancellationToken.None);

            await Task.WhenAll(reflectedTask, busTask);

            Message reflected = reflectedTask.Result;
            if (reflected == message)
            {
                _logger.LogInformation("Message reflected back via medaitor on {Thread}", Environment.CurrentManagedThreadId);
            }

            // like thread this will publish to the current thread, essentially a decoupled event - but still an event whil Bus uses
            // thread pool.  For in process MediatR is still the better option (for simplicity) but worth noting the difference anyway
            _logger.LogInformation("publish notification from {Thread}", Environment.CurrentManagedThreadId);
            await _mediator.Publish(new GreekLetterNotification(DateTime.UtcNow.ToShortTimeString()), CancellationToken.None);
        }
        finally
        {
            _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // should track this token with linked token source so we can use it to prevent 
        _timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        return Task.CompletedTask;
    }


    #region IDisposable
    ///<summary>Finalize</summary>
    ~WorkerService() => Dispose(false);

    ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        System.GC.SuppressFinalize(this);
    }

    ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    ///<param name="disposing">if <c>true</c> then release managed resources in addition to unmanaged</param>
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
        }
    }
    #endregion

}
