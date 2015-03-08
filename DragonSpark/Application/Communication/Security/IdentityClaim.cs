using DragonSpark.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DragonSpark.Application.Communication.Security
{
	[DataContract]
	public class IdentityClaim
	{
		[DataMember, NewGuidDefaultValue, Key]
		public Guid Id { get; set; }
		
		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public string Issuer { get; set; }

		[DataMember]
		public string OriginalIssuer { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Following name from System.IdentityModel" ), DataMember]
		public string Type { get; set; }

		[DataMember]
		public string Value { get; set; }

		[DataMember]
		public string ValueType { get; set; }
	}
}