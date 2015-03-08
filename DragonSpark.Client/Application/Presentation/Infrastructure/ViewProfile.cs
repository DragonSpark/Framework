using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    [ContentProperty( "Content" )]
    public class ViewProfile
    {
        public string Identifier { get; set; }

        public bool IsSelected { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public Rect? MinimumSize { get; set; }
        public Rect? MaximumSize { get; set; }
		
        public object Content { get; set; }
        public DataTemplate ContentTemplate { get; set; }
    }
}