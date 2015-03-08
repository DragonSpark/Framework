using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DragonSpark.Application.Presentation.Controls
{
	public class TabItem : System.Windows.Controls.TabItem
	{
		TabControl tabControl;
		public TabItem()
		{
			this.EnsureLoaded( item =>
			{
				tabControl = this.GetParentOfType<TabControl>();
				tabControl.LayoutUpdated += TabItemLayoutUpdated;
			} );
		}

		void TabItemLayoutUpdated(object sender, EventArgs e)
		{
			Width = Math.Max( 0, tabControl.ActualWidth - tabControl.Padding.Left - tabControl.Padding.Right ) / tabControl.Items.Count;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var query = from quadrant in new [] { "Top", "Bottom", "Left", "Right" }
						from prefix in new [] { "Uns", "S" }
						select GetTemplateChild( string.Format( "Header{0}{1}elected", quadrant, prefix ) );

			var frameworkElements = query.OfType<ContentControl>();
			frameworkElements.Apply( item => {
												item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
												item.VerticalContentAlignment = VerticalAlignment.Stretch;
			} );
		}
	}
}