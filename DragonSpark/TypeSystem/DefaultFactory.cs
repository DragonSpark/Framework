using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Threading;

namespace DragonSpark.TypeSystem
{
	[Synchronized]
	public class DefaultFactory<T> : FactoryBase<T>
	{
		public static DefaultFactory<T> Instance { get; } = new DefaultFactory<T>();

		[Freeze]
		protected override T CreateItem()
		{
			var name = typeof(T).AssemblyQualifiedName;
			var adapter = typeof(T).Adapt();
			var type = adapter.GetEnumerableType();
			var value = type != null ? typeof(Enumerable).InvokeGeneric( nameof(Enumerable.Empty), type.ToItem() ) : adapter.GetDefaultValue();
			var result = value.To<T>();
			return result;
		}
	}
}