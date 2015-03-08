using System.Collections.Generic;

namespace Common.EntityModel
{
	public class EntityDescriptorProvider : IEntityDescriptorProvider
	{
		readonly IEnumerable<EntityDescriptor> descriptors;
		readonly IEntityServiceResolver resolver;

		public EntityDescriptorProvider( IEnumerable<EntityDescriptor> descriptors, IEntityServiceResolver resolver )
		{
			this.descriptors = descriptors;
			this.resolver = resolver;
		}
		
		public IEnumerable<EntityDescriptor> Resolve()
		{
			var result = new EntityDescriptorCollection( descriptors, resolver );
			return result;
		}
	}
}