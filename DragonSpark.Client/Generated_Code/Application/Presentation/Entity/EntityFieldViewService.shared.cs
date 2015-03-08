using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity
{
	[Singleton( typeof(IEntityFieldViewService), Priority = Priority.Lowest )]
	public class EntityFieldViewService : IEntityFieldViewService
	{
		readonly IDictionary<EntityField, IEntityFieldView> cache = new Dictionary<EntityField, IEntityFieldView>();
		readonly IPrincipal principal;
		readonly IEnumerable<IEntityView> views;

		public EntityFieldViewService( IPrincipal principal, IEnumerable<IEntityView> views )
		{
			this.principal = principal;
			this.views = views;
		}

		public IEntityFieldView RetrieveView( EntityField field )
		{
			var result = cache.Ensure( field, Create );
			return result;
		}

		IEntityFieldView Create( EntityField field )
		{
			var tuple = ResolveView( field.EntityType, field.FieldName, field.ViewName );
			var viewField = tuple.Transform( x => x.Item2 );
			var visible = viewField.Transform( x => x.IsViewable && x.AuthorizedRoles.Transform( y => principal.Identity.IsAuthenticated && y.All( principal.IsInRole ), () => true ), () => IsVisibleFromMetadata( field ) );
			var model = viewField.Transform( x => x.Activated<object>() );
			var profile = tuple.Transform( x => x.Item1 ) ?? views.FirstOrDefault( x => x.EntitySet.EntityType == field.EntityType ).Transform( x => x.EntitySet );
			var editable = viewField.Transform( x => x.IsEditable, () => IsEditableMetadata( field ) );
			var result = new EntityFieldView( profile, model, visible, editable );
			return result;
		}

		static bool IsVisibleFromMetadata( EntityField field )
		{
			var result = field.EntityType.GetProperty( field.FieldName ).FromMetadata<RoundtripOriginalAttribute, bool>( x => false, () => true );
			return result;
		}

		static bool IsEditableMetadata( EntityField field )
		{
			return field.EntityType.GetProperty( field.FieldName ).FromMetadata<EditableAttribute,bool>( x => x.AllowEdit, () => true );
		}

		Tuple<IEntitySetProfile,IEntityViewField> ResolveView( Type entityType, string fieldName, string viewName = null )
		{
			var items = new[] { viewName, null }.Select( x => views.FirstOrDefault( y => entityType.IsAssignableFrom( y.EntitySet.EntityType ) && y.ViewName == x ) ).NotNull().ToArray();

			var result = new Tuple<IEntitySetProfile, IEntityViewField>( items.Select( x => x.EntitySet ).NotNull().FirstOrDefault(), items.SelectMany( x => x.Fields ).FirstOrDefault( x => x.FieldName == fieldName ) );
			return result;
		}
	}
}