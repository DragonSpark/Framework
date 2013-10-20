using Breeze.WebApi;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Xml.Serialization;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Method )]
	public class EntityInitializerAttribute : Attribute
	{}

	public class QueryHelper : Breeze.WebApi.QueryHelper
	{
		static readonly MethodInfo CreateInfo = typeof(QueryHelper).GetMethod( "Create", DragonSparkBindingOptions.AllProperties );

		public QueryHelper( ODataQuerySettings querySettings ) : base( querySettings )
		{}

		static IEnumerable<TItem> Create<TItem>( IEnumerable source )
		{
			var result = source.Cast<TItem>().ToList();
			return result;
		}

		/// <summary>
		///     Perform any work after the query is executed.  Does nothing in this implementation but is available to derived
		///     classes.
		/// </summary>
		/// <param name="queryResult"></param>
		/// <returns></returns>
		public override IEnumerable PostExecuteQuery( IEnumerable queryResult )
		{
			var list = queryResult.Cast<object>().Select( x => x.GetType().GetProperty( "Instance" ).Transform( y => y.GetValue( x, null ), () => x ) ).ToArray();

			var type = list.FirstOrDefault().Transform( x => x.GetType() ) ?? typeof(object);

			var result = (IEnumerable)CreateInfo.MakeGenericMethod( type ).Invoke( null, new object[] { list } );
			return result;
		}

		public override IQueryable ApplySelectAndExpand( IQueryable queryable, NameValueCollection map )
		{
			return queryable;
		}
	}

	[AttributeUsage( AttributeTargets.Method )]
	public class EntityQueryAttribute : BreezeQueryableAttribute
	{
		public string EntityName { get; set; }

		protected override Breeze.WebApi.QueryHelper NewQueryHelper()
		{
			return new QueryHelper( GetODataQuerySettings() );
		}
	}

	public class EntityExtensionsBuilder : ClientModuleBuilder
	{
		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = IsResource( resource, "entityextensions" );
			return result;
		}
	}

	public class HubNameBuilder : Factory<string[]>
	{
		protected override string[] CreateItem( object parameter )
		{
			var result = AppDomain.CurrentDomain.GetAllTypesWith<HubNameAttribute>().Select( x => x.Item1.HubName ).ToArray();
			return result;
		}
	}

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

	public class ServerConfigurationBuilder : Factory<ServerConfiguration>
	{
		readonly HubNameBuilder hubNameBuilder;
		readonly EntityServicesBuilder servicesBuilder;

		public ServerConfigurationBuilder( HubNameBuilder hubNameBuilder, EntityServicesBuilder servicesBuilder )
		{
			this.hubNameBuilder = hubNameBuilder;
			this.servicesBuilder = servicesBuilder;
		}

		protected override ServerConfiguration CreateItem( object parameter )
		{
			var result = new ServerConfiguration
			{
				Hubs = hubNameBuilder.Create(),
				EntityServices = servicesBuilder.Create(),
			};
			return result;
		}
	}

	public class ServerConfiguration
	{
		public IEnumerable<string> Hubs { get; set; }

		public IEnumerable<EntityService> EntityServices { get; set; }
	}

	public class EntityService
	{
		public string Location { get; set; }

		public string InitializationMethod { get; set; }

		public IEnumerable<ClientModule> Extensions { get; set; }

		public IEnumerable<EntityQuery> Queries { get; set; }
	}

	public class EntityQuery
	{
		public string SetName { get; set; }
		public string EntityName { get; set; }
		public string Path { get; set; }
		public bool IsLocal { get; set; }
	}
}