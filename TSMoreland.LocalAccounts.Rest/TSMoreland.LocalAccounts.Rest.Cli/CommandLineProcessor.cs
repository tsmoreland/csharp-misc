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

namespace TSMoreland.LocalAccounts.Rest.Cli;

public sealed class CommandLineProcessor
{
    private readonly IUserRepository _userRepository;
    private IReadOnlyDictionary<string, (Action<string?> Consumer, bool hasArg)> _commands;
    private int? _id;
    private string? _username;
    private string? _operation;

    public CommandLineProcessor(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _commands = new Dictionary<string, (Action<string?> Consumer, bool hasArg)>()
        {
            { "u", (SetUsername, true) },
            { "username", (SetUsername, true) },
            { "o", (SetOperation, true) },
            { "operation", (SetOperation, true) },
            { "i", (SetId, true) },
            { "id", (SetId, true) }
            // TODO: usage 
        };
    }

    public void Process(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            string command = GetCommandOrThrow(args[i]);
            if (!_commands.ContainsKey(command))
            {
                throw new NotSupportedException("unknown option");
            }

            (Action<string?> consumer, bool hasArg) = _commands[command];

            string? argument = hasArg && i + 1 < args.Length
                ? args[++i]
                : throw new ArgumentException("invalid arguments", nameof(args));
            consumer(argument);
        }

        Execute();
    }
    private void Execute()
    {
        switch (_operation?.ToUpperInvariant())
        {
            // throw away and fake passwords in use here
            case "LIST":
                _userRepository.PrintAll();
                break;
            case "ADD":
                ThrowInvalidOperationIfNull(_username, "username");
                _userRepository.Add(_username!, "SuP3rP@55w0rd");
                break;
            case "UPDATE":
                ThrowInvalidOperationIfNull(_id, "id");
                ThrowInvalidOperationIfNull(_username, "username");
                _userRepository.Upsert(_id!.Value, _username!, "SuP3rP@55w0rd");
                break;
            case "DELETE":
                ThrowInvalidOperationIfNull(_id, "id");
                _userRepository.Delete(_id!.Value);
                break;
            default:
                break;
        }
    }

    private static void ThrowInvalidOperationIfNull(object? value, string name)
    {
        if (value is null)
        {
            throw new InvalidOperationException($"Error, {name} is required");
        }
    }

    private static string GetCommandOrThrow(string argument)
    {
        if (argument.StartsWith("--"))
        {
            return argument[2..];
        }
        else if (argument.StartsWith("-"))
        {
            return argument[1..];
        }
        else
        {
            throw new ArgumentException("Invalid option");
        }
    }

    private void SetId(string? value)
    {
        if (value is { Length: > 0 } && int.TryParse(value, out int id))
        {
            _id = id;
        }
    }

    private void SetOperation(string? value)
    {
        _operation = value;
    }
    private void SetUsername(string? value)
    {
        _username = value;
    }
}
