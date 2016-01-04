using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class ApplicationAssemblyTransformer : TransformerBase<Assembly[]>
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