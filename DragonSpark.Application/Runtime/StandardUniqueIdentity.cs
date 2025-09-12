using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.Runtime;

/// <summary>
/// Attribution: https://grok.com/share/c2hhcmQtMw%3D%3D_a2483803-1fc6-4b62-aa71-c2921ef2824c
/// </summary>
public sealed class StandardUniqueIdentity : IAlteration<Guid>
{
	public static StandardUniqueIdentity Default { get; } = new();

	StandardUniqueIdentity() {}

	public Guid Get(Guid parameter)
	{
		var data = parameter.ToByteArray();
		data[8] = (byte)((data[8] & 0x3F) | 0x80);
		return new(data);
	}
}