using Foundation;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class SecurityRecord : DragonSpark.Model.Results.Instance<SecRecord>
{
    public static SecurityRecord Default { get; } = new();

    SecurityRecord() : this(SecurityRecordName.Default) {}

    public SecurityRecord(string name)
        : base(new SecRecord(SecKind.Key)
                   { KeyClass = SecKeyClass.Private, ApplicationTag = NSData.FromString(name) }) {}
}