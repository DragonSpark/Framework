using System.Security;

namespace DragonSpark.Application.Security.Data;

public sealed class EncryptionSettings
{
	public string CertificatePath { get; set; } = default!;

	public string PasswordStored
	{
		get => string.Empty;
		set
		{
			var secure = new SecureString();
			foreach (var @char in value)
			{
				secure.AppendChar(@char);
			}

			Password = secure;
		}
	}

	public SecureString Password { get; private set; } = default!;
}