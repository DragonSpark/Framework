using System.Collections.Generic;

namespace DragonSpark.Server.Legacy.Entity
{
	public interface IAuthorizable
	{
		IEnumerable<string> AuthorizedRoles { get; }
	}
}