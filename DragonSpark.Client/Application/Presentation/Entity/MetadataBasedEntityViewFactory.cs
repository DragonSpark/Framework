using System.Collections.Generic;
using System.Linq;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity
{
	public class MetadataBasedEntityViewFactory : Factory<IEnumerable<IEntityView>>
	{
		protected override IEnumerable<IEntityView> CreateItem(object source)
		{
			var attributes = MetadataBasedEntitySetProfileFactory.Instance.Create().SelectMany( x => x.EntityType.GetProperties().SelectMany( y => y.GetAttributes<EntityFieldViewAttribute>().Select( z => new { EntitySet = x, y.Name, Attribute = z } ) ) );
			var enumerable = attributes.GroupBy( x => new { x.EntitySet, x.Attribute.ViewName } );
			var result = enumerable.Select( x => new EntityView( x.Key.EntitySet, x.Key.ViewName, x.Select( y => new EntityViewField( y.Name, y.Attribute.ModelType, y.Attribute.ModelName, y.Attribute.AuthorizedRoles.Transform( z => z.ToStringArray() ), y.Attribute.IsViewable, y.Attribute.IsEditable ) ).ToArray() ) ).ToArray();
			return result;
		}
	}
}