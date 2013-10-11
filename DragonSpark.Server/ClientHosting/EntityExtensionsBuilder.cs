using Breeze.WebApi;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Method )]
	public class EntityInitializerAttribute : Attribute
	{}

	[AttributeUsage( AttributeTargets.Method )]
	public class EntityMethodAttribute : Attribute
	{
		public string EntityName { get; set; }
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
				InitializationMethod = methods.FirstOrDefault( x => x.IsDecoratedWith<EntityInitializerAttribute>() ).Transform( x => x.Name ),
				Queries = methods
				.Where( x => x.IsDecoratedWith<EntityMethodAttribute>() /*&& x.ReturnType.IsGenericType && typeof(IQueryable<>).IsAssignableFrom( x.ReturnType.GetGenericTypeDefinition() )*/ )
				.Select( x => new EntityQuery
				{
					SetName = CodeIdentifier.MakeCamel( x.Name ),
					EntityName = x.FromMetadata<EntityMethodAttribute, string>( y => y.EntityName ) ?? x.ReturnType.GetCollectionElementType().Transform( y => y == typeof(object) ? null : y.Name ), 
					Path = x.Name.ToLower(),
					IsLocal = x.IsPrivate
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