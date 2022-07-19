using DragonSpark.Model.Selection;
using System.Security;

namespace DragonSpark.Runtime;

public sealed class Secure : ISelect<string, SecureString>
{
	public static Secure Default { get; } = new();

	Secure() {}

	public SecureString Get(string parameter)
	{
		unsafe
		{
			fixed (char* psz = parameter)
			{
				var result = new SecureString(psz, parameter.Length);
				result.MakeReadOnly();
				return result;
			}
		}
	}
}