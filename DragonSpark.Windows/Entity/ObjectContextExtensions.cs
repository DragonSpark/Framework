using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Entity
{
	public static class ObjectContextExtensions
	{
		readonly static List<ObjectContext> InitializedContexts = new List<ObjectContext>();

		/// <summary>
		/// Initializeds the specified target.
		/// </summary>
		/// <typeparam name="TContext">The type of the t context.</typeparam>
		/// <param name="target">The target.</param>
		/// <returns>TContext.</returns>
		public static TContext Initialized<TContext>( this TContext target ) where TContext : ObjectContext
		{
			if ( !InitializedContexts.Contains( target ) )
			{
				target.MetadataWorkspace.LoadFromAssembly( target.GetType().Assembly );
				InitializedContexts.Add( target );
			}
			return target;
		}

		/// <summary>
		/// Creates the query text.
		/// </summary>
		/// <typeparam name="TEntity">The type of the t entity.</typeparam>
		/// <param name="target">The target.</param>
		/// <param name="where">The where.</param>
		/// <returns>System.String.</returns>
		public static string CreateQueryText<TEntity>( this ObjectContext target, string where ) where TEntity : EntityObject
		{
			var result = target.CreateQueryText( typeof(TEntity), where );
			return result;
		}

		/// <summary>
		/// Creates the query text.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="where">The where.</param>
		/// <returns>System.String.</returns>
		public static string CreateQueryText( this ObjectContext target, Type entityType, string where )
		{
			var result = string.Format( "SELECT VALUE entity FROM {0}.{1} AS entity WHERE {2}", target.DefaultContainerName,
				target.DetermineEntitySet( entityType ).Name, where );
			return result;
		}


		/// <summary>
		/// Finds the specified target.
		/// </summary>
		/// <typeparam name="TItem">The type of the t item.</typeparam>
		/// <param name="target">The target.</param>
		/// <param name="key">The key.</param>
		/// <returns>TItem.</returns>
		public static TItem Find<TItem>( this ObjectContext target, object[] key ) where TItem : class
		{
			var entityKey = MetadataHelper.CreateKey<TItem>( target, key );
			object entity;
			var result = target.TryGetObjectByKey( entityKey, out entity ) ? (TItem)entity : null;
			return result;
		}

		/// <summary>
		/// Determines the entity set.
		/// </summary>
		/// <typeparam name="TEntity">The type of the t entity.</typeparam>
		/// <param name="context">The context.</param>
		/// <returns>EntitySet.</returns>
		public static EntitySet DetermineEntitySet<TEntity>( this ObjectContext context )
		{
			var result = context.DetermineEntitySet( typeof(TEntity) );
			return result;
		}

		/// <summary>
		/// Determines the entity set.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns>EntitySet.</returns>
		public static EntitySet DetermineEntitySet( this ObjectContext context, Type entityType )
		{
			var objects = (ObjectItemCollection)context.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);
			var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
			var result = container.BaseEntitySets.OfType<EntitySet>().FirstOrDefault(x => !x.Name.Contains( "_" ) && objects.GetClrType( context.MetadataWorkspace.GetObjectSpaceType( x.ElementType ) ).IsAssignableFrom( entityType ));
			return result;
		}

		/// <summary>
		/// Adds the or attach.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="entity">The entity.</param>
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

		/// <summary>
		/// Adds the specified target.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="entity">The entity.</param>
		public static void Add( this ObjectContext target, object entity )
		{
			var entitySet = target.DetermineEntitySet( entity.GetType() );
			target.AddObject( entitySet.Name, entity );
		}

		/// <summary>
		/// Gets the query.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns>IQueryable.</returns>
		public static IQueryable GetQuery( this ObjectContext target, Type entityType )
		{
			var entitySet = target.DetermineEntitySet( entityType );
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( entityType ).Invoke( query, null ).As<IQueryable>();
			return result;
		}

		/// <summary>
		/// Gets the query.
		/// </summary>
		/// <typeparam name="TEntity">The type of the t entity.</typeparam>
		/// <param name="target">The target.</param>
		/// <returns>ObjectQuery&lt;TEntity&gt;.</returns>
		public static ObjectQuery<TEntity> GetQuery<TEntity>( this ObjectContext target )
		{
			var entitySet = target.DetermineEntitySet<TEntity>();
			var query = target.GetType().GetProperty( entitySet.Name ).GetValue( target, null );
			var result = query.GetType().GetMethod( "OfType" ).MakeGenericMethod( typeof(TEntity) ).Invoke( query, null ).As<ObjectQuery<TEntity>>();
			return result;
		}

		/// <summary>
		/// Deletes the object ensured.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="entity">The entity.</param>
		public static void DeleteObjectEnsured( this ObjectContext target, object entity )
		{
			// HACK: Ensure all ends are loaded:
			var query = from property in entity.GetType().GetProperties( BindingOptions.AllProperties )
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

		public static bool IsPropertyChanged( this ObjectContext context, object instance, string propertyName )
		{
			var entry = GetObjectStateEntry( context, instance );
			if ( entry != null )
			{
				var edmMember = GetEdmMember( context, entry, propertyName );

				switch ( edmMember.BuiltInTypeKind )
				{
					case BuiltInTypeKind.NavigationProperty: /*navigation property*/
					{
						var navigationProperty = edmMember as NavigationProperty;
						var sourceRelatedEnd = entry.RelationshipManager.GetRelatedEnd( navigationProperty.RelationshipType.FullName, navigationProperty.ToEndMember.Name );
						const EntityState state = EntityState.Added | EntityState.Deleted;
						var relationshipGroups = GetRelationshipsByRelatedEnd( context, entry, state );
						return relationshipGroups.Select( relationshipGroup => relationshipGroup.Key ).Any( targetRelatedEnd => Check( targetRelatedEnd, sourceRelatedEnd ) );
					}

					case BuiltInTypeKind.EdmProperty: /*scalar field*/
					{
						ObjectStateEntry containerStateEntry = null;
						return context.IsScalarPropertyModified( propertyName, entry, out containerStateEntry );
					}
				}

				throw new InvalidOperationException( "Property type not supported" );
			}
			return false;
		}

		static ObjectStateEntry GetObjectStateEntry( ObjectContext context, object instance )
		{
			ObjectStateEntry result;
			return context.ObjectStateManager.TryGetObjectStateEntry( instance, out result ) || TryGetObjectStateEntry( context, instance, ref result ) ? result : null;
		}

		static bool TryGetObjectStateEntry( ObjectContext context, object instance, ref ObjectStateEntry result )
		{
			object item;
			return context.TryGetObjectByKey( context.ExtractKey( instance ), out item ) && context.ObjectStateManager.TryGetObjectStateEntry( item, out result );
		}

		static bool Check( IRelatedEnd targetRelatedEnd, object sourceRelatedEnd )
		{
			var isEntityReference = targetRelatedEnd.IsEntityReference();
			var same = targetRelatedEnd == sourceRelatedEnd;
			var result = isEntityReference && same;
			return result;
		}

		public static EntityState GetEntityState( this ObjectContext context, EntityKey key ) {
			var entry = context.ObjectStateManager.GetObjectStateEntry( key );
			return entry.State;
		}

		public static string GetFullEntitySetName( this EntityKey key ) {
			return key.EntityContainerName + "." + key.EntitySetName;
		}

		public static IEntityWithKey GetEntityByKey( this ObjectContext context, EntityKey key ) {
			return (IEntityWithKey)context.ObjectStateManager.GetObjectStateEntry( key ).Entity;
		}

		//
		// ObjectStateEntry
		//

		public static IExtendedDataRecord UsableValues( this ObjectStateEntry entry ) {
			switch ( entry.State ) {
				case EntityState.Added:
				case EntityState.Detached:
				case EntityState.Unchanged:
				case EntityState.Modified:
					return (IExtendedDataRecord)entry.CurrentValues;
				case EntityState.Deleted:
					return (IExtendedDataRecord)entry.OriginalValues;
				default:
					throw new InvalidOperationException( "This entity state should not exist." );
			}
		}

		public static EdmType EdmType( this ObjectStateEntry entry ) {
			return entry.UsableValues().DataRecordInfo.RecordType.EdmType;
		}

		public static bool IsManyToMany( this AssociationType associationType ) {
			foreach ( RelationshipEndMember endMember in associationType.RelationshipEndMembers ) {
				if ( endMember.RelationshipMultiplicity != RelationshipMultiplicity.Many ) {
					return false;
				}
			}
			return true;
		}

		//
		// RelationshipEntry
		//

		public static bool IsRelationshipForKey( this ObjectStateEntry entry, EntityKey key ) {
			if ( entry.IsRelationship == false ) {
				return false;
			}
			return ( (EntityKey)entry.UsableValues()[ 0 ] == key ) || ( (EntityKey)entry.UsableValues()[ 1 ] == key );
		}

		public static EntityKey OtherEndKey( this ObjectStateEntry relationshipEntry, EntityKey thisEndKey ) {
			Debug.Assert( relationshipEntry.IsRelationship );
			Debug.Assert( thisEndKey != null );

			if ( (EntityKey)relationshipEntry.UsableValues()[ 0 ] == thisEndKey ) {
				return (EntityKey)relationshipEntry.UsableValues()[ 1 ];
			}
			else if ( (EntityKey)relationshipEntry.UsableValues()[ 1 ] == thisEndKey ) {
				return (EntityKey)relationshipEntry.UsableValues()[ 0 ];
			}
			else {
				throw new InvalidOperationException( "Neither end of the relationship contains the passed in key." );
			}
		}

		public static string OtherEndRole( this ObjectStateEntry relationshipEntry, EntityKey thisEndKey ) {
			Debug.Assert( relationshipEntry != null );
			Debug.Assert( relationshipEntry.IsRelationship );
			Debug.Assert( thisEndKey != null );

			if ( (EntityKey)relationshipEntry.UsableValues()[ 0 ] == thisEndKey ) {
				return relationshipEntry.UsableValues().DataRecordInfo.FieldMetadata[ 1 ].FieldType.Name;
			}
			else if ( (EntityKey)relationshipEntry.UsableValues()[ 1 ] == thisEndKey ) {
				return relationshipEntry.UsableValues().DataRecordInfo.FieldMetadata[ 0 ].FieldType.Name;
			}
			else {
				throw new InvalidOperationException( "Neither end of the relationship contains the passed in key." );
			}
		}

		//
		// IRelatedEnd methods
		//

		public static bool IsEntityReference( this IRelatedEnd relatedEnd ) {
			Type relationshipType = relatedEnd.GetType();
			return ( relationshipType.GetGenericTypeDefinition() == typeof( EntityReference<> ) );
		}

		public static EntityKey GetEntityKey( this IRelatedEnd relatedEnd ) {
			Debug.Assert( relatedEnd.IsEntityReference() );
			Type relationshipType = relatedEnd.GetType();
			PropertyInfo pi = relationshipType.GetProperty( "EntityKey" );
			return (EntityKey)pi.GetValue( relatedEnd, null );
		}

		public static void SetEntityKey( this IRelatedEnd relatedEnd, EntityKey key ) {
			Debug.Assert( relatedEnd.IsEntityReference() );
			Type relationshipType = relatedEnd.GetType();
			PropertyInfo pi = relationshipType.GetProperty( "EntityKey" );
			pi.SetValue( relatedEnd, key, null );
		}

		public static bool Contains( this IRelatedEnd relatedEnd, EntityKey key ) {
			foreach ( object relatedObject in relatedEnd ) {
				Debug.Assert( relatedObject is IEntityWithKey );
				if ( ( (IEntityWithKey)relatedObject ).EntityKey == key ) {
					return true;
				}
			}
			return false;
		}

		//
		// queries over the context
		//

		public static IEnumerable<IEntityWithKey> GetEntities( this ObjectContext context, EntityState state ) {
			return from e in context.ObjectStateManager.GetObjectStateEntries( state )
				where e.IsRelationship == false && e.Entity != null
				select (IEntityWithKey)e.Entity;
		}

		public static IEnumerable<ObjectStateEntry> GetRelationships( this ObjectContext context, EntityState state ) {
			return from e in context.ObjectStateManager.GetObjectStateEntries( state )
				where e.IsRelationship == true
				select e;
		}

		public static IEnumerable<ObjectStateEntry> GetUnchangedManyToManyRelationships( this ObjectContext context ) {
			return context.GetRelationships( EntityState.Unchanged )
				.Where( e => ( (AssociationType)e.EdmType() ).IsManyToMany() );
		}

		public static IEnumerable<ObjectStateEntry> GetRelationshipsForKey( this ObjectContext context, EntityKey key, EntityState state ) {
			return context.GetRelationships( state ).Where( e => e.IsRelationshipForKey( key ) );
		}

		public static IEnumerable<IGrouping<IRelatedEnd, ObjectStateEntry>> GetRelationshipsByRelatedEnd( this ObjectContext context,
			ObjectStateEntry entry, EntityState state ) {
			return from e in context.GetRelationshipsForKey( entry.EntityKey, state )
				group e by ( entry.RelationshipManager
					.GetRelatedEnd( ( (AssociationType)( e.EdmType() ) ).Name,
						e.OtherEndRole( entry.EntityKey ) ) );
			}

		//
		// original values
		//

		// Extension method for the ObjectContext which will create an object instance that is essentially equivalent
		// to the original object that was added or attached to the context before any changes were performed.
		// NOTE: This object will have no relationships--just the original value properties.
		public static object CreateOriginalValuesObject( this ObjectContext context, object source ) {
			// Get the state entry of the source object
			//     NOTE: For now we require the object to implement IEntityWithKey.
			//           This is something we should be able to relax later.
			Debug.Assert( source is IEntityWithKey );
			EntityKey sourceKey = ( (IEntityWithKey)source ).EntityKey;
			// This method will throw if the key is null or an entry isn't found to match it.  We
			// could throw nicer exceptions, but this will catch the important invalid cases.
			ObjectStateEntry sourceStateEntry = context.ObjectStateManager.GetObjectStateEntry( sourceKey );

			// Return null for added entities & throw an exception for detached ones.  In other cases we can
			// always make a new object with the original values.
			switch ( sourceStateEntry.State ) {
				case EntityState.Added:
					return null;
				case EntityState.Detached:
					throw new InvalidOperationException( "Can't get original values when detached." );
			}

			// Create target object and add it to the context so that we can easily set properties using 
			// the StateEntry.  Since objects in the added state use temp keys, we know this won't
			// conflict with anything already in the context.
			object target = Activator.CreateInstance( source.GetType() );
			string fullEntitySetName = sourceKey.EntityContainerName + "." + sourceKey.EntitySetName;
			context.AddObject( fullEntitySetName, target );
			EntityKey targetKey = context.CreateEntityKey( fullEntitySetName, target );
			
			ObjectStateEntry targetStateEntry = context.ObjectStateManager.GetObjectStateEntry( targetKey );

			// Copy original values from the sourceStateEntry to the targetStateEntry.  This will
			// cause the corresponding properties on the object to be set.
			for ( int i = 0; i < sourceStateEntry.OriginalValues.FieldCount; i++ ) {
				targetStateEntry.CurrentValues.SetValue( i, sourceStateEntry.OriginalValues[ i ] );
			}

			// Detach the object we just created since we only attached it temporarily in order to use 
			// the stateEntry.
			context.Detach( target );

			// Set the EntityKey property on the object (if it implements IEntityWithKey).
			IEntityWithKey targetWithKey = target as IEntityWithKey;
			if ( targetWithKey != null ) {
				targetWithKey.EntityKey = sourceKey;
			}

			return target;
		}

		private static bool IsScalarPropertyModified(this ObjectContext context, string scalarPropertyName, ObjectStateEntry entityContainer, out ObjectStateEntry containerStateEntry)
		{
			var isModified = false;
			containerStateEntry = context.ObjectStateManager.GetObjectStateEntry( entityContainer.EntityKey );
			var modifiedProperties = containerStateEntry.GetModifiedProperties();

			var changedProperty = modifiedProperties.FirstOrDefault( element => ( element == scalarPropertyName ) );
			isModified = ( null != changedProperty );

			if ( isModified )
			{
				var originalValue = containerStateEntry.OriginalValues[changedProperty];
				var currentValue = containerStateEntry.CurrentValues[changedProperty];
				//sometimes property can be treated as changed even though you set the same value it had before
				isModified = !Equals( originalValue, currentValue );
			}

			return isModified;
		}



		static EdmMember GetEdmMember( this ObjectContext context, ObjectStateEntry entry, string propertyName )
		{
			EdmMember edmMember = null;
			var entityType = context.MetadataWorkspace.GetEntityMetaData( entry.Entity.GetType() );
			var edmMembers = entityType.MetadataProperties.First( p => p.Name == "Members" ).Value as IEnumerable<EdmMember>;
			edmMember = edmMembers.FirstOrDefault( item => item.Name == propertyName );
			if ( edmMember == null )
			{
				throw new ArgumentException(
					string.Format( "Cannot find property metadata: property '{0}' in '{1}' entity object", propertyName, entityType.Name ) );
			}
			return edmMember;
		}

	}
}