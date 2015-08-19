using System;
using System.Windows.Interactivity;

namespace Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Custom event trigger for using with <see cref="IInteractionRequest"/> objects.
    /// </summary>
    /// <remarks>
    /// The standard <see cref="System.Windows.Interactivity.EventTrigger"/> class can be used instead, as long as the 'Raised' event 
    /// name is specified.
    /// </remarks>
    [Obsolete("InteractionRequestTrigger is not needed to use the PopupWindowAction. You can use the built-in EventTrigger the same way except you also have to set EventName='Raised'")]
    public class InteractionRequestTrigger : EventTrigger
    {
        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        /// <returns>This implementation always returns the Raised event name for ease of connection with <see cref="IInteractionRequest"/>.</returns>
        protected override string GetEventName()
        {
            return "Raised";
        }
    }
}
