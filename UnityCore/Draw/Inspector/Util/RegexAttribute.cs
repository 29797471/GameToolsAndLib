using UnityEngine;

/// <summary>
/// [Regex (@"^(?:\d{1,3}\.){3}\d{1,3}$", "Invalid IP address!\nExample: '127.0.0.1'")]
/// public string serverAddress = "192.168.0.1";
/// </summary>
public class RegexAttribute : PropertyAttribute
{
    public readonly string pattern;
    public readonly string helpMessage;
    public RegexAttribute(string pattern, string helpMessage)
    {
        this.pattern = pattern;
        this.helpMessage = helpMessage;
    }
}