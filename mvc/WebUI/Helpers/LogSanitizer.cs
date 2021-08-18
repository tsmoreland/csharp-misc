using System.Text.Json;

namespace WebUI.Helpers
{
    public static class LogSanitizer
    {
        public static string Sanitize<T>(T value)
        {
            var options = new JsonSerializerOptions(); // defaults are fine here
            return JsonSerializer.Serialize(value.ToString(), options).Trim('"');
        }
    }
}
