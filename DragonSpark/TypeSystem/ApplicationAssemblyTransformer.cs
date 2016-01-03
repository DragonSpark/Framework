using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static T Item => DefaultFactory<T>.Instance.Create();
		public static T[] Items => DefaultFactory<T[]>.Instance.Create();
	}

	public class DefaultFactory<T> : FactoryBase<T>
	{
		public static DefaultFactory<T> Instance { get; } = new DefaultFactory<T>();

		[Cache]
		protected override T CreateItem()
		{
			var adapter = typeof(T).Adapt();
			var type = adapter.GetEnumerableType();
			var value = type != null ? typeof(Enumerable).InvokeGeneric( nameof(Enumerable.Empty), type.ToItem() ) : adapter.GetDefaultValue();
			var result = value.To<T>();
			return result;
		}
	}

	public class ApplicationAssemblyTransformer : FactoryBase<Assembly[], Assembly[]>
	{
		public static ApplicationAssemblyTransformer Instance { get; } = new ApplicationAssemblyTransformer();

		readonly string[] namespaces;

		public ApplicationAssemblyTransformer() : this( Default<Assembly>.Items )
		{}

		public ApplicationAssemblyTransformer( [Required]IEnumerable<Assembly> coreAssemblies ) : this( Determine( coreAssemblies ) )
		{}

		static string[] Determine( IEnumerable<Assembly> coreAssemblies ) => coreAssemblies.NotNull().Append( typeof(ApplicationAssemblyTransformer).Assembly() ).Distinct().Select( assembly => assembly.GetRootNamespace() ).ToArray();

		ApplicationAssemblyTransformer( string[] namespaces )
		{
			this.namespaces = namespaces;
		}

		protected override Assembly[] CreateItem( Assembly[] parameter )
		{
			var result = parameter.Where( assembly => assembly.IsDefined( typeof(RegistrationAttribute) ) || namespaces.Any( assembly.GetName().Name.StartsWith ) ).ToArray();
			return result;
		}
	}
}