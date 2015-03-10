using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Interaction;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms
{
    [MessageName( "Xamarin.ShowActionSheet" )]
    class DisplayOptionsEvent : FormsEvent<Page, ActionSheetArguments>
    {}
    
    public class DisplayOptionsRequest : InteractionRequest<OptionsNotification>
    {
        public DisplayOptionsRequest()
        {
            this.Event<DisplayOptionsEvent>().Subscribe( this, OnAlert );
        }

        ~DisplayOptionsRequest()
        {
            Events.With<DisplayOptionsEvent>( @event => @event.Unsubscribe( this ) );
        }

       /* public void Test()
        {
            this.Event<DisplayOptionsEvent>().Publish( new ContentPage(), new ActionSheetArguments( "Do Something", "Nevermind", "Remove", new [] { "Button 1", "Button 2", "Button 3" } ) );
        }*/

        void OnAlert( Page page, ActionSheetArguments arguments )
        {
            var notification = arguments.MapInto<OptionsNotification>().With( item => item.Content = arguments.Buttons );
            Raise( notification, item =>
            {
                Trace.WriteLine( string.Format( "Option Selected: {0}", item.Result ) );
                arguments.SetResult( item.Result );
            }  );
        }
    }
    
    [MessageName( "Xamarin.SendAlert" )]
    class SystemMessageEvent : FormsEvent<Page, AlertArguments>
    {}

    public class SystemMessageRequest : InteractionRequest<DialogNotification>
    {
        public SystemMessageRequest()
        {
            this.Event<SystemMessageEvent>().Subscribe( this, OnAlert );
        }

        ~SystemMessageRequest()
        {
            Events.With<SystemMessageEvent>( @event => @event.Unsubscribe( this ) );
        }

        void OnAlert( Page page, AlertArguments arguments )
        {
            var notification = arguments.MapInto<DialogNotification>().With( item => item.Content = arguments.Message );
            Raise( notification, item => arguments.SetResult( item.Result.GetValueOrDefault() )  );
        }
    }
}