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
		readonly string[] namespaces;

		static string[] Determine( IEnumerable<Assembly> coreAssemblies ) => coreAssemblies.NotNull().Append( typeof(ApplicationAssemblyTransformer).Assembly() ).Distinct().Select( assembly => assembly.GetRootNamespace() ).ToArray();

		public ApplicationAssemblyTransformer( [Required]params Assembly[] coreAssemblies ) : this( Determine( coreAssemblies ) )
		{}

		ApplicationAssemblyTransformer( string[] namespaces )
		{
			this.namespaces = namespaces;
		}

		protected override Assembly[] CreateItem( Assembly[] parameter ) => 
			parameter.Where( assembly => assembly.Has<RegistrationAttribute>() || namespaces.Any( assembly.GetName().Name.StartsWith ) ).Prioritize().ToArray();
	}
}