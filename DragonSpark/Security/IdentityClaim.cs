using DragonSpark.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonSpark.Security
{
	public class IdentityClaim
	{
		[NewGuidDefaultValue, Key, Column( Order = 0 )]
		public Guid Id { get; set; }
		
		[Key, Column( Order = 1 )]
		public string Name { get; set; }

		public string Issuer { get; set; }

		public string OriginalIssuer { get; set; }

		public string Type { get; set; }

		public string Value { get; set; }

		public string ValueType { get; set; }
	}
}