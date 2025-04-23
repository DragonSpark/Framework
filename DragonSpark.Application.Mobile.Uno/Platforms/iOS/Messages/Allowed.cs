using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection.Conditions;
using MessageUI;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Messages;

sealed class Allowed : IDepending
{
    public static Allowed Default { get; } = new();

    Allowed() : this(Display.Default) {}

    readonly IAllocated<DisplayInput> _display;

    public Allowed(IAllocated<DisplayInput> display) => _display = display;

    public async ValueTask<bool> Get(None parameter)
    {
        var result = MFMessageComposeViewController.CanSendText;
        if (!result)
        {
            // Handle the case where SMS is not available (e.g., no SIM card)
            await _display.Off(new("SMS Not Available", "This device cannot send SMS."));
        }

        return result;
    }
}