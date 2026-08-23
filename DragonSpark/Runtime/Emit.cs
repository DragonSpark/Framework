using DragonSpark.Model.Selection;
using System.Runtime.InteropServices;
using System.Security;

namespace DragonSpark.Runtime;

public sealed class Emit : ISelect<SecureString, string>
{
	public static Emit Default { get; } = new();

	Emit() {}

	public string Get(SecureString parameter)
	{
		var marshal = Marshal.SecureStringToBSTR(parameter);
		try
		{
			return Marshal.PtrToStringBSTR(marshal);
		}
		finally
		{
			Marshal.ZeroFreeBSTR(marshal);
		}
	}
}