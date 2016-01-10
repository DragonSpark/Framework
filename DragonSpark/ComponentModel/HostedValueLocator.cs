using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System.Linq;

namespace DragonSpark.ComponentModel
{
	public class HostedValueLocator<T> : FactoryBase<object, T[]> where T : class
	{
		public static HostedValueLocator<T> Instance { get; } = new HostedValueLocator<T>();

		protected override T[] CreateItem( object parameter ) => parameter.GetAttributes<HostingAttribute>().Select( attribute => attribute.Create( parameter ) ).OfType<T>().ToArray();
	}
}