// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using TSMoreland.BackgroundWorker.Console;

Console.WriteLine("Hello, World!");

string line;



Channel<Message> channel = Channel.CreateBounded<Message>(new BoundedChannelOptions(10));
Worker worker = new(channel);

CancellationTokenSource cancelSource = new();
await worker.StartAsync(cancelSource.Token);

do
{
    line = Console.ReadLine()?.ToUpperInvariant() ?? string.Empty;

    if (!int.TryParse(line, out int value))
    {
        break;
    }

    Message msg = new(value, new TaskCompletionSource<int>());
    await channel.Writer.WriteAsync(msg);

    int result = await msg.CompletionSource.Task;
    Console.WriteLine(result);

} while (line != "QUIT");

CancellationTokenSource stopSource = new();
await worker.StopAsync(stopSource.Token);

