using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System.Linq;

namespace DragonSpark.ComponentModel
{
	public class HostedValueLocator<T> : FactoryBase<object, T[]> where T : class
	{
		readonly IAttributeProvider provider;
		public static HostedValueLocator<T> Instance { get; } = new HostedValueLocator<T>();

		public HostedValueLocator() : this( AttributeProvider.Instance ) {}

		public HostedValueLocator( [Required]IAttributeProvider provider )
		{
			this.provider = provider;
		}

		protected override T[] CreateItem( object parameter ) => provider.GetAttributes<HostingAttribute>( parameter ).Select( attribute => attribute.Item ).OfType<T>().ToArray();
	}
}