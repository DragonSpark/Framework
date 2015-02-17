using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Security
{
	[Singleton( typeof(IClaimsRepository) )]
	class ClaimsRepository : IClaimsRepository
	{
		readonly IEnumerable<ClaimMappingDefinition> definitions;

		public ClaimsRepository( IEnumerable<ClaimMappingDefinition> definitions )
		{
			this.definitions = definitions;
		}

		public IEnumerable<Claim> DetermineClaims( string providerName, IDictionary<string, string> userData )
		{
			var definition = definitions.FirstOrDefault( x => string.Equals( x.ProviderName, providerName, StringComparison.InvariantCultureIgnoreCase ) );
			var result = definition.Transform( x => Extract( providerName, x.Mappings, userData ) );
			return result;
		}

		static IEnumerable<Claim> Extract( string providerName, IEnumerable<ClaimMapping> mappings, IDictionary<string, string> data )
		{
			var map = mappings.ToDictionary( x => x.FieldName, x => x.Claim );

			var result = data.Keys.Where( key => map.Keys.Contains( key ) && data[key] != null ).Select( key => new Claim( map[key], data[key], ClaimValueTypes.String, providerName ) ).ToArray();
			return result;
		}
	}
}