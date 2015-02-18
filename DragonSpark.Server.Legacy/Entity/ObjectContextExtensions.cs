using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Entity
{
	public static class ObjectContextExtensions
	{
		readonly static List<ObjectContext> InitializedContexts = new List<ObjectContext>();

		public static TContext Initialized<TContext>( this TContext target ) where TContext : ObjectContext
		{
			if ( !InitializedContexts.Contains( target ) )
			{
				target.MetadataWorkspace.LoadFromAssembly( target.GetType().Assembly );
				InitializedContexts.Add( target );
			}
			return target;
		}

		public static string CreateQueryText<TEntity>( this ObjectContext target, string where ) where TEntity : EntityObject
		{
			var result = target.CreateQueryText( typeof(TEntity), where );
			return result;
		}

		public static string CreateQueryText( this ObjectContext target, Type entityType, string where )
		{
			var result = string.Format( "SELECT VALUE entity FROM {0}.{1} AS entity WHERE {2}", target.DefaultContainerName,
				target.DetermineEntitySet( entityType ).Name, where );
			return result;
		}


		public static TItem Find<TItem>( this ObjectContext target, object[] key ) where TItem : class
		{
			var entityKey = MetadataHelper.CreateKey<TItem>( target, key );
			object entity;
			var result = target.TryGetObjectByKey( entityKey, out entity ) ? (TItem)entity : null;
			return result;
		}

		public static EntitySet DetermineEntitySet<TEntity>( this ObjectContext context )
		{
			var result = context.DetermineEntitySet( typeof(TEntity) );
			return result;
		}

		public static EntitySet DetermineEntitySet( this ObjectContext context, Type entityType )
		{
			var objects = (ObjectItemCollection)context.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);
			var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
			var result = container.BaseEntitySets.OfType<EntitySet>().FirstOrDefault(x => !x.Name.Contains( "_" ) && objects.GetClrType( context.MetadataWorkspace.GetObjectSpaceType( x.ElementType ) ).IsAssignableFrom( entityType ));
			return result;
		}

		public static void AddOrAttach( this ObjectContext target, object entity )
		{
			object existing;
			var key = MetadataHelper.ExtractKey( target, entity );
			if ( target.TryGetObjectByKey( key, out existing ) )
			{
				var entityWithKey = entity.To<IEntityWithKey>();
				entityWithKey.EntityKey = key;
				target.Detach( existing );
				target.Attach( entityWithKey );
			}
			else
			{
				target.Add( entity );
			}
		}

		public static void Add( this ObjectContext target, object entity )
		{
			var entitySet = target.DetermineEntitySet( entity.GetType() );
			target.AddObject( entitySet.Name, entity );
		}

		public static IQueryable GetQuery( this ObjectContext target, Type entityType )
		{
			var entitySet = target.DetermineEntitySet( entityType );
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( entityType ).Invoke( query, null ).As<IQueryable>();
			return result;
		}

		public static ObjectQuery<TEntity> GetQuery<TEntity>( this ObjectContext target )
		{
			var entitySet = target.DetermineEntitySet<TEntity>();
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( typeof(TEntity) ).Invoke( query, null ).As<ObjectQuery<TEntity>>();
			return result;
		}

		public static void DeleteObjectEnsured( this ObjectContext target, object entity )
		{
			// HACK: Ensure all ends are loaded:
			var query = from property in entity.GetType().GetProperties( DragonSparkBindingOptions.AllProperties )
				where typeof(IRelatedEnd).IsAssignableFrom( property.PropertyType )
				let end = property.GetValue( entity, null ) as IRelatedEnd
				where end != null && !end.IsLoaded
				select end;
			foreach ( var end in query )
			{
				end.Load();
			}

			target.DeleteObject( entity );
		}
	}
}