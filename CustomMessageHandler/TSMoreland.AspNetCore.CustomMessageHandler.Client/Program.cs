using System.Collections;
using System.Net.Http.Headers;
using System.Reflection;

var field = typeof(HttpHeaders)
.GetField("_headerStore", BindingFlags.NonPublic | BindingFlags.Instance);


var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000");
requestMessage.Content = new StringContent("negative length");
_ = requestMessage.Content.Headers.ContentLength; // side effect to add the header

dynamic headers = requestMessage.Content.Headers;

dynamic store = field!.GetValue(headers);
PropertyInfo property = store.GetType().GetProperty("Values", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
var value = property.GetValue(store);

var enumerator = ((ICollection)value).GetEnumerator();
object? item = null;
while (enumerator.MoveNext())
{
    item = enumerator.Current;
}

field = item!.GetType().GetField("ParsedValue", BindingFlags.Instance | BindingFlags.NonPublic);
field!.SetValue(item, -42L);

using var client = new HttpClient();
try
{
    var response = await client.SendAsync(requestMessage, CancellationToken.None);

    if (response.IsSuccessStatusCode)
    {
        var message = await response.Content.ReadAsStringAsync(CancellationToken.None);
        Console.WriteLine($"Response: {message}");

    }
    else
    {
        Console.WriteLine($"Unsuccessful request: {response.StatusCode}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
