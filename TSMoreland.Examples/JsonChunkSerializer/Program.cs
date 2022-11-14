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


using System.Text.Json;
using TSMoreland.Examples.JsonChunkSerializer;

const string source = """
    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Tortor at risus viverra adipiscing at. Enim tortor at auctor urna nunc id. Lacus vel facilisis volutpat est velit egestas dui id ornare. Etiam non quam lacus suspendisse faucibus. Volutpat diam ut venenatis tellus in. Commodo quis imperdiet massa tincidunt nunc. Turpis cursus in hac habitasse. Adipiscing elit pellentesque habitant morbi. Pellentesque habitant morbi tristique senectus. Ornare aenean euismod elementum nisi quis eleifend quam adipiscing. Feugiat pretium nibh ipsum consequat nisl vel pretium lectus. Erat imperdiet sed euismod nisi porta lorem.
    Est ullamcorper eget nulla facilisi etiam dignissim diam quis. Diam maecenas ultricies mi eget mauris pharetra et. Facilisi morbi tempus iaculis urna id volutpat. Nunc pulvinar sapien et ligula ullamcorper malesuada. Amet aliquam id diam maecenas ultricies. At erat pellentesque adipiscing commodo. Faucibus vitae aliquet nec ullamcorper. Elementum curabitur vitae nunc sed velit dignissim. Aenean et tortor at risus viverra adipiscing at in tellus. Imperdiet nulla malesuada pellentesque elit eget.
    Cursus vitae congue mauris rhoncus aenean vel elit scelerisque. Vivamus arcu felis bibendum ut tristique et egestas quis ipsum. Quam viverra orci sagittis eu volutpat. Risus feugiat in ante metus dictum at. Sed euismod nisi porta lorem mollis aliquam ut porttitor. Vulputate odio ut enim blandit volutpat maecenas volutpat. Velit laoreet id donec ultrices tincidunt arcu. Vitae aliquet nec ullamcorper sit amet risus nullam eget. Viverra orci sagittis eu volutpat odio facilisis mauris sit amet. Scelerisque purus semper eget duis at tellus at. Pellentesque id nibh tortor id aliquet lectus. Neque convallis a cras semper auctor. At in tellus integer feugiat scelerisque. Id semper risus in hendrerit. Mauris a diam maecenas sed enim ut sem. Aliquam vestibulum morbi blandit cursus.
    Amet aliquam id diam maecenas ultricies mi eget mauris pharetra. At auctor urna nunc id cursus metus aliquam eleifend mi. Scelerisque felis imperdiet proin fermentum leo vel orci. Nisi porta lorem mollis aliquam ut porttitor leo a diam. Diam quam nulla porttitor massa id neque aliquam vestibulum morbi. Consequat ac felis donec et odio pellentesque diam volutpat. Sit amet consectetur adipiscing elit duis tristique sollicitudin nibh sit. Nibh tellus molestie nunc non blandit massa enim nec. Porttitor rhoncus dolor purus non enim praesent elementum facilisis. Ornare lectus sit amet est placerat. In hac habitasse platea dictumst quisque. Proin fermentum leo vel orci. Proin sagittis nisl rhoncus mattis. Duis ultricies lacus sed turpis. Sagittis vitae et leo duis ut diam. Neque vitae tempus quam pellentesque nec nam aliquam sem. Ornare aenean euismod elementum nisi quis eleifend quam. Molestie at elementum eu facilisis sed odio.
    Id interdum velit laoreet id donec. Elit ullamcorper dignissim cras tincidunt lobortis. Malesuada proin libero nunc consequat interdum varius. Diam maecenas ultricies mi eget mauris pharetra et. Condimentum vitae sapien pellentesque habitant morbi tristique senectus et. Aliquet sagittis id consectetur purus ut faucibus pulvinar elementum integer. Massa sed elementum tempus egestas. Risus nec feugiat in fermentum posuere. Suspendisse faucibus interdum posuere lorem. A diam maecenas sed enim ut sem viverra aliquet eget. Eget arcu dictum varius duis at consectetur. Commodo elit at imperdiet dui. Amet venenatis urna cursus eget nunc. Montes nascetur ridiculus mus mauris vitae ultricies leo integer. Enim tortor at auctor urna nunc id. Libero nunc consequat interdum varius sit amet mattis. Urna et pharetra pharetra massa massa ultricies. Ut etiam sit amet nisl purus. Tempor nec feugiat nisl pretium fusce id velit.
    """;

string serialized = JsonSerializer.Serialize(source);

Console.WriteLine(serialized);
Console.WriteLine("------------------------------------");

string[] lines = source.Split('\n');


using MemoryStream stream = new();
await using StreamWriter writer = new(stream, leaveOpen: true);
LargeStringJsonSerializer serializer = new(writer);

Task writeTask = serializer.WriteAsync(default);

for (int i = 0; i < lines.Length; i++)
{
    serializer.Append(lines[i]);
    if (i != lines.Length - 1)
    {
        serializer.Append("\n");
    }
}

serializer.Complete();

await writeTask;

stream.Position = 0;
using StreamReader reader = new(stream, leaveOpen: true);

string largeSerialized = reader.ReadToEnd();

Console.WriteLine(largeSerialized);

if (largeSerialized == serialized)
{
    Console.WriteLine("Content matches");
}
else
{
    if (serialized.Length != largeSerialized.Length)
    {
        Console.WriteLine($"Size mismatch {serialized.Length} != {largeSerialized.Length}");
    }

    for (int i = 0; i < Math.Min(serialized.Length, largeSerialized.Length); i++)
    {
        if (serialized[i] != largeSerialized[i])
        {
            Console.WriteLine($"First Difference at position {i}");
            break;
        }
    }

}
