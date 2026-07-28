using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Output.Compose;

public sealed class SelectedKeyRegistration<TIn, TKey> where TIn : notnull
{
	readonly RegistrationComponents _components;
	readonly Func<TIn, TKey>        _selection;

	public SelectedKeyRegistration(RegistrationComponents components, Func<TIn, TKey> selection)
	{
		_components = components;
		_selection  = selection;
	}

	public StartRegistration<TIn> Register()
	{
		_components.Registrations.Add(new Registration<TIn, TKey>(_selection));
		return new(_components);
	}

	public StartRegistration<TIn> Many<TOut>(IStopAware<TKey, TOut> select, ISelect<TOut, IEnumerable<TKey>> keys)
		=> Many(select, keys.Get);

	public StartRegistration<TIn> Many<TOut>(IStopAware<TKey, Leasing<TOut>> select,
	                                         ISelect<TOut, IEnumerable<TKey>> keys)
		=> Many(select, keys.Get);

	public StartRegistration<TIn> Many<TOut>(IStopAware<TKey, Leasing<TOut>> select,
	                                         Func<TOut, IEnumerable<TKey>> keys)
	{
		_components.Registrations.Add(new ManyRegistrations<TIn, TOut, TKey>(_selection, select, keys));
		return new(_components);
	}

	public StartRegistration<TIn> Many<TOut>(IStopAware<TKey, TOut> select, Func<TOut, IEnumerable<TKey>> keys)
	{
		_components.Registrations.Add(new Registrations<TIn, TOut, TKey>(_selection, select, keys));
		return new(_components);
	}

	public StartRegistration<TIn> Single(IStopAware<TKey, TKey> select)
	{
		_components.Registrations.Add(new Single<TIn, TKey>(_selection, select));
		return new(_components);
	}
}