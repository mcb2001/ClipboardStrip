using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TextCopy;

namespace ClipboardStrip
{
    internal static class Program
    {
        private static void Main()
        {
            if (Clipboard.GetText() is string text)
            {
                if (JsonSerializerExtensions.TryDeserialize(text, out dynamic json))
                {
                    text = JsonSerializer.Serialize<dynamic>(json, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });
                }

                Clipboard.SetText(text);
            }
        }
    }

    internal static class JsonSerializerExtensions
    {
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Try pattern")]
        public static bool TryDeserialize(string value, out dynamic json)
        {
            try
            {
                json = JsonSerializer.Deserialize<dynamic>(value);
                return true;
            }
            catch (JsonException)
            {
                json = new object();
                return false;
            }
        }
    }
}
