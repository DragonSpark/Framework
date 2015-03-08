using System;

namespace Common.EntityModel
{
	public interface IEntityServiceResolver
	{
		IEntityService Resolve( Type entityType );
	}
}