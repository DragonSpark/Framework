using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Runtime;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class ParameterRequestRelay : ISpecimenBuilder
	{
		readonly Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner;
		readonly static NoSpecimen NoSpecimen = new NoSpecimen();

		public ParameterRequestRelay( Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner )
		{
			this.inner = inner;
		}

		public object Create( object request, ISpecimenContext context )
		{
			var parameter = request as ParameterInfo;
			var result = parameter != null ? ( ShouldDefault( parameter ) ? parameter.DefaultValue : inner.Create( request, context ) ) : NoSpecimen;
			return result;
		}

		static bool ShouldDefault( ParameterInfo info ) => 
			info.IsOptional && !MethodContext.Default.Get().GetParameterTypes().Any( info.ParameterType.Adapt().IsAssignableFrom );
	}
}