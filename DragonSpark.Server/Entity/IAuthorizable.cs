using System.Collections.Generic;

namespace DragonSpark.Server.Entity
{
	public interface IAuthorizable
	{
		IEnumerable<string> AuthorizedRoles { get; }
	}
}