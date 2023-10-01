namespace Common.Model;

public enum MessageType
{
    INFORMATION = 0,
    WARNING = 1,
    ERROR = 2
}
public class MessageModel
{
    public MessageType Type { get; private set; }
    public string Text { get; private set; }

    public MessageModel(string message, MessageType type = MessageType.INFORMATION)
    {
        Type = type;
        Text = message;
    }
}