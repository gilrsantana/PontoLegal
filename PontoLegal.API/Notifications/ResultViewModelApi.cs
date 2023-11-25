using Common.Model;
using Common.Notifications;

namespace PontoLegal.API.Notifications;

public class ResultViewModelApi<T> : ResultViewModelBase<T>
{
    public ResultViewModelApi(T viewData, List<MessageModel> messages) 
        : base(viewData, messages)
    {
    }

    public ResultViewModelApi(T viewData) 
        : base(viewData)
    {
    }

    public ResultViewModelApi(List<MessageModel> messages) 
        : base(messages)
    {
    }

    public ResultViewModelApi(MessageModel message) 
        : base(message)
    {
    }

    public ResultViewModelApi(string message, MessageType type = MessageType.INFORMATION) 
        : base(message, type)
    {
    }

    public ResultViewModelApi(IReadOnlyCollection<string> list, MessageType type = MessageType.INFORMATION) 
        : base(list, type)
    {
    }
}