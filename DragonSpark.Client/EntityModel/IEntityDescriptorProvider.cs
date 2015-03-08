using System.Collections.Generic;

namespace Common.EntityModel
{
	public interface IEntityDescriptorProvider
	{
		IEnumerable<EntityDescriptor> Resolve();
	}
}