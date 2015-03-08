using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
// using System.Windows.Ria;
using Common.Extensions;

namespace Common.EntityModel
{
	public class EntityDescriptorCollection : ObservableCollection<EntityDescriptor>
	{
		readonly IEntityServiceResolver resolver;

		public EntityDescriptorCollection()
		{}

		public EntityDescriptorCollection( IEnumerable<EntityDescriptor> descriptors, IEntityServiceResolver resolver )
		{
			this.resolver = resolver;
			this.AddAll( descriptors ).Execute( Ensure );
		}

		void Ensure( EntityDescriptor descriptor )
		{
			var name = descriptor.Type.Name;
			descriptor.Title = descriptor.ItemName ?? descriptor.Type.FromMetadata<SummaryAttribute, string>( i => i.Title, name );
			descriptor.ItemName = descriptor.ItemName ?? descriptor.Type.FromMetadata<SummaryAttribute, string>( i => i.ItemName, name );
			descriptor.ItemNamePlural = descriptor.ItemName ?? descriptor.Type.FromMetadata<SummaryAttribute, string>( i => i.ItemNamePlural, string.Concat( name, "s" ) );
			descriptor.Description = descriptor.Description ?? descriptor.Type.FromMetadata<DescriptionAttribute, string>( i => i.Description );
			descriptor.Service = descriptor.Service ?? resolver.Resolve( descriptor.Type );
		}
	}
}