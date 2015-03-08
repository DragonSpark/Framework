using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Application.Communication.Security;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Following convention from outside library." )]
	public static class DbContextExtensions
	{
		static readonly MethodInfo 
			ApplyChangesMethod = typeof(DbContextExtensions).GetMethod( "ApplyChangesInternal", DragonSparkBindingOptions.AllProperties );

		public static T Add<T>( this DbContext context, T entity ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );
			var dbEntityEntry = context.Entry( entity );
			dbEntityEntry.State = EntityState.Added;
			return entity;
		}

		public static T Update<T>( this DbContext context, T entity ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );
			var entry = context.Entry( entity );
			EnsureAttached( context, entity, entry );
			entry.State = EntityState.Modified;
			return entity;
		}

		static void EnsureAttached<T>( DbContext context, T entity, DbEntityEntry<T> entry ) where T : class
		{
			switch ( entry.State )
			{
				case EntityState.Detached:
					context.Set<T>().Attach( entity );
					break;
			}
		}

		public static T Update<T>( this DbContext context, T current, T original ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( current != null );
			Contract.Requires( original != null );

			var entry = context.Entry( current );
			EnsureAttached( context, current, entry );
			entry.State = EntityState.Unchanged;
			entry.OriginalValues.SetValues( original );
			var properties = TypeDescriptor.GetProperties( typeof(T) );
			var attributes = TypeDescriptor.GetAttributes( typeof(T) );

			foreach ( var propertyName in from propertyName in entry.CurrentValues.PropertyNames
			                              let descriptor = properties[ propertyName ]
			                              where
											descriptor != null && descriptor.Attributes[ typeof(RoundtripOriginalAttribute) ] == null && 
											attributes[ typeof(RoundtripOriginalAttribute) ] == null && 
											descriptor.Attributes[ typeof(ExcludeAttribute) ] == null
			                              select propertyName )
			{
				entry.Property( propertyName ).IsModified = true;
			}

			if ( entry.State != EntityState.Modified )
			{
				entry.State = EntityState.Modified;
			}
			return current;
		}

		public static object ApplyChanges( this DbContext target, object entity )
		{
			var result = ApplyChangesMethod.MakeGenericMethod( entity.GetType() ).Invoke( null, new[]{ target, entity } );
			return result;
		}

		public static TEntity ApplyChanges<TEntity>( this DbContext target, TEntity entity ) where TEntity : class
		{
			return ApplyChangesInternal( target, entity );
		}

		static TEntity ApplyChangesInternal<TEntity>( DbContext target, TEntity entity ) where TEntity : class
		{
			var values = target.ResolveKeyValues( entity );
			var set = target.Set<TEntity>();
			var current = set.Find( values );
			if ( current != null )
			{
				var entry = target.Entry( current );
				entry.CurrentValues.SetValues( entity );
				switch ( entry.State )
				{
					case EntityState.Unchanged:
					case EntityState.Detached:
						Update( target, current );
						break;
				}
				return current;
			}
			
			target.Add( entity );
			return entity;
		}

		public static void Remove<T>( this DbContext context, T entity ) where T : class
		{
			Contract.Requires( context != null );
			Contract.Requires( entity != null );
			context.Entry( entity ).State = EntityState.Deleted;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target", Justification = "Used as a convenience to keep from adding another extension method to the object class." )]
        public static object[] ResolveKeyValues( this DbContext target, object entity )
		{
			var type = entity.GetType();
			var propertyInfos = type.GetProperties().Where( x => x.IsDecoratedWith<KeyAttribute>() ).OrderBy( x => x.FromMetadata<DisplayAttribute, int>( y => y.GetOrder().GetValueOrDefault( 0 ), () => 0 ) );
			var result = propertyInfos.Select( x => type.GetProperty( x.Name ).GetValue( entity, null ) ).ToArray();
			return result;
		}

		public static IEnumerable<PropertyInfo> ResolveNavigationProperties( this DbContext context, Type type )
		{
			var types = context.GetType().GetProperties().Where( x => x.PropertyType.IsGenericType && typeof(DbSet<>).IsAssignableFrom( x.PropertyType.GetGenericTypeDefinition() ) ).Select( x => x.PropertyType.GetGenericArguments().FirstOrDefault() ).NotNull().ToArray();
			var result = type.GetProperties().Where( x => types.Contains( x.ResolveEntityType() ) ).ToArray();
			return result;
		}

		public static IDisposable EnsureAssociations<TContext>( this TContext target ) where TContext : DbContext
		{
			var result = new EnsuredAssociationContext( target );
			return result;
		}

		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, params Expression<Func<TEntity,object>>[] expressions ) where TEntity : class
		{
			var result = target.Include( entity, expressions.Select( x => x.GetMemberInfo().Name ).ToArray() );
			return result;
		}
		
		public static TEntity Include<TEntity>( this DbContext target, TEntity entity, params string[] associationNames ) where TEntity : class
		{
			var result = target.ApplyChanges( entity );
			var entry = target.Entry( result );
			var type = entity.GetType();
			foreach ( var name in associationNames )
			{
				if ( type.GetProperty( name ).GetCollectionType() != null )
				{
					var collection = entry.Collection( name );
					var canLoad = !collection.IsLoaded && collection.CurrentValue.To<IEnumerable>().Cast<object>().All( x => target.Entry( x ).State != EntityState.Added );
					try
					{
						canLoad.IsTrue( collection.Load );
					}
					catch ( InvalidOperationException )
					{
						// HACK: Gotta figure out what's going on here.
					}
				}
				else
				{
					var reference = entry.Reference( name );
					reference.IsLoaded.IsFalse( reference.Load );
				}
			}
			return result;
		}

		public static IQueryable<TUser> GetAdministrators<TUser>( this EntityStorage<TUser> target ) where TUser : ApplicationUser
		{
			var result = target.Users.Where( x => x.RolesSource.Contains( "Administrator" ) );
			return result;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract the expression." )]
        public static IEnumerable<TEntity> Total<TEntity>( this DbContext context, Expression<Func<TEntity,bool>> predicate ) where TEntity : class
		{
			var dbSet = context.Set<TEntity>();
			var result = dbSet.Where( predicate ).ToArray().Union( dbSet.Local ).Where( predicate.Compile() ).ToArray();
			return result;
		}

		class EnsuredAssociationContext : IDisposable
		{
			readonly DbContext context;
			readonly bool saved;

			public EnsuredAssociationContext( DbContext context )
			{
				this.context = context;
				saved = context.Configuration.LazyLoadingEnabled;
				context.Configuration.LazyLoadingEnabled = true;
			}

			public void Dispose()
			{
				context.Configuration.LazyLoadingEnabled = saved;
			}
		}
	}
}