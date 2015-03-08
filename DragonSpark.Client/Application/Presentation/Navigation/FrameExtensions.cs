using System;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Navigation
{
    public static class FrameExtensions
    {
        public static void RefreshFromState( this Frame target )
        {
            var uri = System.Windows.Application.Current.Host.NavigationState.NullIfEmpty().Transform( y => new Uri( y, UriKind.Relative ) );
            uri.NotNull( x => target.Navigate( x ) );
        }
    }
}