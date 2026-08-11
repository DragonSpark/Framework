using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Model;

public class SwitchModel<TValue> : BindingModel<TValue>
{
	readonly Func<TValue> _initialize;

	protected SwitchModel(IMutable<TValue?> store) : this(store.Execute, store.Get) {}

	protected SwitchModel(Action<TValue?> set, Func<TValue?> get) : this(set, get, get!) {}

	protected SwitchModel(Action<TValue?> set, Func<TValue?> get, Func<TValue> initialize) : base(set, get)
	{
		_initialize = initialize;
		On          = get() is not null;
	}

	public bool On
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				Execute(field ? _initialize() : default);
			}
		}
	}
}