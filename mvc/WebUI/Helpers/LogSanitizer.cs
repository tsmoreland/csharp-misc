using System.Web;

namespace WebUI.Helpers
{
    public static class LogSanitizer
    {
        public static string Sanitize(string value)
        {
            return  value is {Length: > 0}
                ? HttpUtility.JavaScriptStringEncode(value.Replace("\r", "").Replace("\n", ""))
                : null;
        }
    }
}
