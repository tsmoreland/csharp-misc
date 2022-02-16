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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TSMoreland.Messaging.Demo.App;

public sealed class WorkerService : IHostedService, IDisposable
{
    private readonly IBus _bus;
    private readonly Timer _timer;
    private readonly ILogger<WorkerService> _logger;

    public WorkerService(IBus bus, ILoggerFactory loggerFactory)
    {
        _bus = bus;
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
            string content = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            Message message = new(content);
            TargetedMessage targetedMessage = new(content);

            Task messageTask = _bus.Publish(message, CancellationToken.None);
            Task targettedMessageTask = _bus.Publish(targetedMessage, CancellationToken.None);
            await Task.WhenAll(messageTask, targettedMessageTask)
                .ConfigureAwait(false);
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
