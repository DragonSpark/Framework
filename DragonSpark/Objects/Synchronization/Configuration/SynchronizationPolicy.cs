using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	[ContentProperty( "Resolvers" )]
	public class SynchronizationPolicy : SynchronizationPolicyBase
	{
		public SynchronizationKey Key { get; set; }

		public ObservableCollection<SynchronizationExpressionResolverBase> Resolvers
		{
			get { return resolvers ?? ( resolvers = new ObservableCollection<SynchronizationExpressionResolverBase>() ); }
		}	ObservableCollection<SynchronizationExpressionResolverBase> resolvers;


		protected override ISynchronizationPolicy Create()
		{
			var instances = ( from provider in Resolvers select provider.Instance ).ToArray();
			var result = new Synchronization.SynchronizationPolicy( Key.Instance, instances );
			return result;
		}
	}
}