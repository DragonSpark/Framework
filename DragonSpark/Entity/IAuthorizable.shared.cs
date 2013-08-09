using System.Collections.Generic;

namespace DragonSpark.Entity
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Authorizable", Justification = "http://www.thefreedictionary.com/Authorizable" )]
	public interface IAuthorizable
	{
		IEnumerable<string> AuthorizedRoles { get; }
	}
}