using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	[ContentProperty( "Policies" )]
	public class SynchronizationContainer : IInstanceSource<Synchronization.SynchronizationContainer>
	{
		public SynchronizationContainer()
		{
			ContinueOnMappingException = true;
		}

		public ObservableCollection<SynchronizationPolicyBase> Policies
		{
			get { return policies ?? ( policies = new ObservableCollection<SynchronizationPolicyBase>() ); }
		}	ObservableCollection<SynchronizationPolicyBase> policies;

		public bool ContinueOnMappingException { get; set; }

		public Synchronization.SynchronizationContainer Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	Synchronization.SynchronizationContainer instance;

		protected virtual Synchronization.SynchronizationContainer Create()
		{
			var result = new Synchronization.SynchronizationContainer( from policy in Policies select policy.Instance )
         	{
         		ContinueOnMappingException = ContinueOnMappingException
         	};
			return result;
		}
	}
}