using System.Web;

namespace WebUI.Helpers
{
    public static class LogSanitizer
    {
        public static string Sanitize<T>(T @object)
        {
            string value = @object.Equals(default)
                ? @object.ToString()
                : string.Empty;
            return  value is {Length: > 0}
                ? HttpUtility.JavaScriptStringEncode(value.Replace("\r", "").Replace("\n", ""))
                : null;
        }
    }
}
