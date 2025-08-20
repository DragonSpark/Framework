using System;

namespace DragonSpark.Presentation.Model;

public class ByteModel : BindingModel<byte>
{
	protected ByteModel(Action<byte> set, Func<byte> get) : base(set, get) {}
}