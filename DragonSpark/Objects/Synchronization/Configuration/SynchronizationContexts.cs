using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	[ContentProperty( "Contexts" )]
	public class SynchronizationContexts : SynchronizationExpressionResolverBase
	{
		public ObservableCollection<SynchronizationContext> Contexts
		{
			get { return contexts ?? ( contexts = new ObservableCollection<SynchronizationContext>() ); }
		}	ObservableCollection<SynchronizationContext> contexts;

		protected override ISynchronizationExpressionResolver Create()
		{
			var result = new SynchronizationExpressionResolver( from context in Contexts select context.Instance );
			return result;
		}
	}
}