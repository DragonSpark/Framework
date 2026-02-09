using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Foundation;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class CreateSecurityKey : ISelect<SecRecord, SecKey>
{
    public static CreateSecurityKey Default { get; } = new();

    CreateSecurityKey() {}

    public SecKey Get(SecRecord parameter)
    {
        // Private key attributes
        var privateAttrs = new NSMutableDictionary();
        privateAttrs.SetValueForKey(NSNumber.FromBoolean(true), (NSString)"kSecAttrIsPermanent");
        privateAttrs.SetValueForKey(parameter.ApplicationTag.Verify(), (NSString)"kSecAttrApplicationTag");

        // Key generation parameters
        var parameters = new NSMutableDictionary();
        parameters.SetValueForKey((NSString)"EC", (NSString)"kSecAttrKeyType");
        parameters.SetValueForKey(NSNumber.FromInt32(256), (NSString)"kSecAttrKeySizeInBits");
        parameters.SetValueForKey((NSString)"SecureEnclave", (NSString)"kSecAttrTokenID");
        parameters.SetValueForKey(privateAttrs, (NSString)"kSecPrivateKeyAttrs");

        // NOTE: 4‑arg overload: (SecKeyType, int, NSDictionary, out NSError)
        var result = SecKey.CreateRandomKey(SecKeyType.EC, 256, parameters, out var error) ??
                     throw new InvalidOperationException($"Key generation failed: {error}");
        return result;
    }
}