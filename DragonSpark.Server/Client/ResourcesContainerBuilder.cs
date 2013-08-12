using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Server.Client
{
	public class ResourcesContainerBuilder : Factory<ResourcesContainer>
	{
		readonly string resourcesNamespace;
		readonly Assembly applicationAssembly;

		public ResourcesContainerBuilder( Assembly applicationAssembly = null, string resourcesNamespace = "Resources" )
		{
			this.resourcesNamespace = resourcesNamespace;
			this.applicationAssembly = applicationAssembly ?? BuildManager.GetGlobalAsaxType().BaseType.Assembly;
		}

		protected override ResourcesContainer CreateItem( object parameter )
		{
			var result = new ResourcesContainer();
			applicationAssembly.GetTypes().Where( x => x.Namespace == resourcesNamespace ).Apply( x =>
			{
				var propertyInfos = Enumerable.Where<PropertyInfo>( x.GetProperties( DragonSparkBindingOptions.AllProperties ), y => y.PropertyType == typeof(string) );
				result.Add( x.Name, propertyInfos.ToDictionary( y =>  y.Name, y => (string)y.GetValue( null, null ) ) );
			} );
			return result;
		}
	}
}