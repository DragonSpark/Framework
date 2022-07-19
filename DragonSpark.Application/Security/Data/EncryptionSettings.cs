using DragonSpark.Runtime;
using System.Security;

namespace DragonSpark.Application.Security.Data;

public sealed class EncryptionSettings
{
	public string CertificatePath { get; set; } = default!;

	public string PasswordStored
	{
		get => string.Empty;
		set => Password = Secure.Default.Get(value);
	}

	public SecureString Password { get; private set; } = default!;
}

