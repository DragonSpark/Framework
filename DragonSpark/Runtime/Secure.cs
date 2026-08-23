using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Security;

namespace DragonSpark.Runtime;

public sealed class Secure : ISelect<string, SecureString>
{
    public static Secure Default { get; } = new();

    Secure() { }

    [MustDisposeResource]
    public SecureString Get(string parameter)
    {
	    // ReSharper disable once RedundantUnsafeContext ISSUE: 
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
