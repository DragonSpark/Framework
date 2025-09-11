using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.Runtime;

/// <summary>
/// Attribution: https://x.com/i/grok/share/OiKEkhi8WdhbxQT2VsO1mlLmF
/// </summary>
public sealed class StandardUniqueIdentity : IAlteration<Guid>
{
	public static StandardUniqueIdentity Default { get; } = new();

	StandardUniqueIdentity() {}

	public Guid Get(Guid parameter)
	{
		var bytes   = parameter.ToByteArray();
		var octet8  = bytes[8];
		var variant = (byte)(octet8 >> 4);
		switch (variant)
		{
			case 0xC:
			case 0xD:
			{
				var bits = (byte)(octet8 & 0x03);
				bytes[8] = (byte)(0x80 | bits);
				return new(bytes);
			}
		}

		return parameter;
	}
}