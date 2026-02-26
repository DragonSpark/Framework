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
        // 1. Generate Secure Enclave key
        var privateAttrs = new NSMutableDictionary();
        privateAttrs.SetValueForKey(NSNumber.FromBoolean(true), (NSString)"kSecAttrIsPermanent");
        privateAttrs.SetValueForKey(parameter.ApplicationTag.Verify(), (NSString)"kSecAttrApplicationTag");

        var parameters = new NSMutableDictionary();
        parameters.SetValueForKey(privateAttrs, (NSString)"kSecPrivateKeyAttrs");
        parameters.SetValueForKey((NSString)"EC", (NSString)"kSecAttrKeyType");
        parameters.SetValueForKey(NSNumber.FromInt32(256), (NSString)"kSecAttrKeySizeInBits");
        parameters.SetValueForKey((NSString)"SecureEnclave", (NSString)"kSecAttrTokenID");

        var key = SecKey.CreateRandomKey(SecKeyType.EC, 256, parameters, out var error)
                  ?? throw new InvalidOperationException($"Key generation failed: {error}");

        var record = new SecRecord(key)
        {
            KeyClass       = parameter.KeyClass,
            ApplicationTag = parameter.ApplicationTag,
        };

        var status = SecKeyChain.Add(record);
        _ = ExistingSecurityKey.Default.Get(parameter) ??
            throw new InvalidOperationException("Key was not found"); // TODO
        return status switch
        {
            SecStatusCode.Success or SecStatusCode.DuplicateItem => key,
            _ => throw new InvalidOperationException($"Keychain add failed: {status}")
        };
    }
}