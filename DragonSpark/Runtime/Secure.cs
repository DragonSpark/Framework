using System.Security;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

public sealed class Secure : ISelect<string, SecureString>
{
    public static Secure Default { get; } = new();

    Secure() { }

    [MustDisposeResource]
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
