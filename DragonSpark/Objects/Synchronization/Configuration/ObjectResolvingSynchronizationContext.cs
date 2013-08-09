using System.Windows.Markup;
using DragonSpark.Objects.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	[ContentProperty( "ObjectResolver" )]
	public class ObjectResolvingSynchronizationContext : SynchronizationContext
	{
		public ObjectResolvingSynchronizationContext()
		{
			IncludeBasePolicies = true;
		}

		public ObjectResolver ObjectResolver { get; set; }

		public string MappingName { get; set; }

		public bool IncludeBasePolicies { get; set; }

		protected override Synchronization.SynchronizationContext Create()
		{
			var result = new Synchronization.ObjectResolvingSynchronizationContext( ObjectResolver.Instance, FirstExpression ?? Expression, SecondExpression ?? Expression, MappingName, IncludeBasePolicies );
			return result;
		}
	}
}