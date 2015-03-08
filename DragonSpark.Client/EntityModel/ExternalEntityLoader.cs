using System;
using System.Linq;
using Common.Extensions;

namespace Common.EntityModel
{
	public class ExternalEntityLoader : IExternalEntityLoader
	{
		readonly IExternalEntityReferenceProvider references;
		readonly IEntityServiceResolver resolver;

		public ExternalEntityLoader( IExternalEntityReferenceProvider references, IEntityServiceResolver resolver )
		{
			this.references = references;
			this.resolver = resolver;
		}

		public void LoadExternalEntities( System.Windows.Ria.Entity entity )
		{
			var type = entity.GetType();
			var items = references.Resolve( type );
			var query = from item in items
			            let current = item.EntityProperty.GetValue( entity, null )
			            where current == null
			            let service = resolver.Resolve( item.EntityProperty.PropertyType )
			            where service != null
			            let key = item.Keys.ToDictionary( i => i.Key, i => i.Value.GetValue( entity, null ) )
			            select new Action( () => service.Retrieve( key ) );

			query.Execute( item => item() );
		}
	}
}