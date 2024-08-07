using System;

namespace DragonSpark.Presentation.Model;

public class ByteModel : BindingModel<sbyte>
{
	protected ByteModel(Action<sbyte> set, Func<sbyte> get) : base(set, get) {}
}