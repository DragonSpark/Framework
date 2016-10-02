using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DragonSpark.Commands;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Entity
{
	public class RegisterComplexTypesCommand : CommandBase<DbContextBuildingParameter>
	{
		public override void Execute( DbContextBuildingParameter parameter )
		{
			var method = parameter.Builder.GetType().GetMethod( nameof(parameter.Builder.ComplexType) );
			parameter.Context.GetDeclaredEntityTypes().First().Assembly.GetTypes().Where( x => AttributeProviderExtensions.Has<ComplexTypeAttribute>( x ) ).Each( x =>
																																								  {
																																									  var info = method.MakeGenericMethod( x );
																																									  info.Invoke( parameter.Builder, null );
																																								  } );
		}
	}
}