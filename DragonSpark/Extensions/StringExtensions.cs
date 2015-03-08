using System;
using System.Linq;
using System.Net;

namespace DragonSpark.Extensions
{
	public static partial class StringExtensions
	{
		public static string ToMembershipName( this string target )
		{
			NetworkCredential credential = CredentialHelper.Create( target, null );
			string result = credential.UserName ?? string.Empty;
			return result;
		}
	}
}