using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	// TODO: Remove
	public class ProviderDefinition
	{
		public ProviderDefinition(string name, params IUserMapping[] definitions)
			: this(name, definitions.Result()) {}

		protected ProviderDefinition(string name, Array<IUserMapping> profileMappings)
			: this(name, profileMappings,
			       Array.Of<IClaimMapping>(new ClaimMapping(DisplayName.Default, ClaimTypes.Name))) {}

		protected ProviderDefinition(string name, Array<IUserMapping> profileMappings,
		                             Array<IClaimMapping> claimMappings)
		{
			Name            = name;
			ProfileMappings = profileMappings;
			ClaimMappings   = claimMappings;
		}

		public string Name { get; }

		public Array<IUserMapping> ProfileMappings { get; }

		public Array<IClaimMapping> ClaimMappings { get; }
	}
}