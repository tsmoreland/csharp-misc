using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TSMoreland.ObjectTracker.Data;
using TSMoreland.ObjectTracker.Data.Abstractions;

namespace TSMoreland.ObjectTracker.Test;

[TestFixture]
public class ObjectContextTest
{

    [Test]
    public void ObjectContext_DoesNotThrow_WhenUsingJsonStreamSourceForConfiguration()
    {
        Assert.DoesNotThrow(() =>
        {
            using MemoryStream stream = new();
            using (StreamWriter streamWriter = new(stream, new UTF8Encoding(false), 8196, true))
            {
                streamWriter.Write(@"{""databaseOptions"":{""pooling"":true}}");
                streamWriter.Flush();
            }

            stream.Seek(0, SeekOrigin.Begin);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            DbContextOptionsBuilder<ObjectContext> optionsBuilder = new();
            _ = new ObjectContext(optionsBuilder.Options, configuration);
        });
    }

}
