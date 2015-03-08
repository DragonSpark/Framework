using System;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
    public partial class Menu : INavigate
    {
        public bool Navigate( Uri source )
        {
            var result = Content.AsTo<INavigate, bool>( x => x.Navigate( source ) );
            return result;
        }
    }
}