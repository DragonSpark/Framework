using System;
using System.Linq;
using System.Web.Http;
using System.Xml.Serialization;
using Breeze.WebApi2;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Server.ClientHosting
{
	public class EntityServicesBuilder : Factory<EntityService[]>
	{
		readonly EntityExtensionsBuilder extensionsBuilder;

		public EntityServicesBuilder( EntityExtensionsBuilder extensionsBuilder )
		{
			this.extensionsBuilder = extensionsBuilder;
		}

		protected override EntityService[] CreateItem( object parameter )
		{
			var result = AppDomain.CurrentDomain.GetAllTypesWith<BreezeControllerAttribute>().Select( x => CreateService( x.Item2 ) ).ToArray();
			return result;
		}

		EntityService CreateService( Type type )
		{
			var location = type.Name.Replace( "Controller", string.Empty );
			var methods = type.GetMethods( DragonSparkBindingOptions.AllProperties );
			var result = new EntityService
			{
				Extensions = extensionsBuilder.Create( string.Concat( "entityextensions.", location ) ),
				Location = location,
				InitializationMethod = methods.FirstOrDefault( x => x.IsDecoratedWith<EntityInitializerAttribute>() ).Transform( x => x.FromMetadata<RouteAttribute, string>( y => y.Template ) ?? x.Name ),
				Queries = methods
					.Where( x => x.IsDecoratedWith<EntityQueryAttribute>() /*&& x.ReturnType.IsGenericType && typeof(IQueryable<>).IsAssignableFrom( x.ReturnType.GetGenericTypeDefinition() )*/ )
					.Select( x =>
					{
						var identifier = x.FromMetadata<RouteAttribute, string>( z => z.Template ) ?? x.Name;
						return new EntityQuery
						{
							SetName = CodeIdentifier.MakeCamel( identifier ),
							EntityName = x.FromMetadata<EntityQueryAttribute, string>( y => y.EntityName ) ?? x.ReturnType.GetCollectionElementType().Transform( y => y == typeof(object) ? null : y.Name ),
							Path = identifier.ToLower(),
							IsLocal = x.IsPrivate
						};
					} )
			};
			return result;
		}
	}
}