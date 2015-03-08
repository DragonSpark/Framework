using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class SetIndexAction : TargetedTriggerAction<Selector>
	{
		protected override void Invoke( object parameter )
		{
			var index = Target.ItemContainerGenerator.Transform( x => x.IndexFromContainer( x.ContainerFromItem( AssociatedObject.GetDataContext() ) ) );
			Target.SelectedIndex = index;
		}
	}
}