using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Model;

public class BindingModel<TValue> : Mutable<TValue?>
{
	protected BindingModel(IMutable<TValue?> store) : this(store.Execute, store.Get) {}

	protected BindingModel(Action<TValue?> set, Func<TValue?> get) : base(set, get)
	{
	}

	public TValue? Value
	{
		get => Get();
		set => Execute(value);
	}
}