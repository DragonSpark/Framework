using System;
using System.Collections.Generic;

namespace Common.EntityModel
{
	public interface IExternalEntityReferenceProvider
	{
		IEnumerable<ExternalEntityReference> Resolve( Type entityType );
	}
}