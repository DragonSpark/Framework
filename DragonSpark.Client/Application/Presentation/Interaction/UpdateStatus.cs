using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class UpdateStatus : Behavior<HyperlinkButton>
	{
		protected override void OnAttached()
		{
			AssociatedObject.MouseEnter += AssociatedObjectMouseEnter;
			AssociatedObject.MouseLeave += AssociatedObjectMouseLeave;
			var toolTip = ToolTipService.GetToolTip( AssociatedObject );
			toolTip.NotNull( item => ToolTipService.SetToolTip( AssociatedObject, string.Format( "{0}: {1}", item, AssociatedObject.NavigateUri ) ) );
		}

		protected override void OnDetaching()
		{
			AssociatedObject.MouseEnter -= AssociatedObjectMouseEnter;
			AssociatedObject.MouseLeave -= AssociatedObjectMouseLeave;
		}

		string Status { get; set; }
		
		void AssociatedObjectMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if ( HtmlPage.IsEnabled )
			{
				var toolTip = ToolTipService.GetToolTip( AssociatedObject ).As<string>();
				if ( !string.IsNullOrEmpty( toolTip ) )
				{
					Status = HtmlPage.Document.GetProperty( "title" ).Transform( item => item.ToString() );
					HtmlPage.Document.SetProperty( "title", toolTip );
				}
			}
		}
		void AssociatedObjectMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if ( HtmlPage.IsEnabled && !string.IsNullOrEmpty( Status ) )
			{
				HtmlPage.Document.SetProperty( "title", Status );
				Status = null;
			}
		}
	}
}