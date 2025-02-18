using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using System.Runtime.CompilerServices;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class PrincipalStores : ReferenceValueTable<string, PrincipalStore>, ICommand
{
	readonly ConditionalWeakTable<string, PrincipalStore> _store;
	public static PrincipalStores Default { get; } = new();

	public PrincipalStores() : this(new()) {}

	public PrincipalStores(ConditionalWeakTable<string, PrincipalStore> store) : base(x => new(x)) => _store = store;

	public void Execute(None parameter)
	{
		_store.Clear();
	}
}