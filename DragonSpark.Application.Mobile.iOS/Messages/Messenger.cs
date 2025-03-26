using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Device.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Results;
using MessageUI;
using UIKit;

namespace DragonSpark.Application.Mobile.iOS.Messages;

sealed class Messenger : IMessenger
{
    public static Messenger Default { get; } = new();

    Messenger() : this(Allowed.Default, CurrentController.Default) {}

    readonly IDepending                 _allowed;
    readonly IResult<UIViewController?> _controller;

    public Messenger(IDepending allowed, IResult<UIViewController?> controller)
    {
        _allowed    = allowed;
        _controller = controller;
    }

    public async ValueTask<bool> Get(Token<MessageInput> parameter)
    {
        var ((recipient, message), _) = parameter;

        if (await _allowed.Off(None.Default))
        {
            // Get the current view controller
            var controller = _controller.Get();
            if (controller is not null)
            {
                // Create the message composer
                var compose = new MFMessageComposeViewController();
                compose.Recipients = [recipient];
                compose.Body       = message;

                // Set up the delegate to handle completion
                var source = new TaskCompletionSource<bool>();
                compose.Finished += (_, args) =>
                                    {
                                        args.Controller.DismissViewController(true, null);
                                        source.SetResult(args.Result == MessageComposeResult.Sent);
                                    };

                // Present the message composer
                await controller.PresentViewControllerAsync(compose, true).Off();
                await source.Task.Off();
                return true;
            }
        }

        return false;
    }
}