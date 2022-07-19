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
			unsafe
			{
				var pointer = (char*)marshal.ToPointer();
				return new string(pointer);
			}
		}
		finally
		{
			Marshal.ZeroFreeBSTR(marshal);
		}
	}
}