using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Server.Security
{
	[ContentProperty( "Mappings" )]
	public class ClaimMappingDefinition
	{
		public string ProviderName { get; set; }

		public List<ClaimMapping> Mappings
		{
			get { return mappings; }
		}	readonly List<ClaimMapping> mappings = new List<ClaimMapping>();
	}
}