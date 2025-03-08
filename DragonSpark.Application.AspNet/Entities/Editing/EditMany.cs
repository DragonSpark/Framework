using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditMany<TIn, T> : IEditMany<TIn, T>
{
	readonly IEdit<TIn, Leasing<T>> _edit;

	public EditMany(IEnlistedScopes scope, IQuery<TIn, T> query)
		: this(scope.Then().Use(query).Edit.Lease()) {}

	protected EditMany(IEdit<TIn, Leasing<T>> edit) => _edit = edit;

	[MustDisposeResource]
	public async ValueTask<ManyEdit<T>> Get(TIn parameter)
	{
		var (editor, subject) = await _edit.Off(parameter);
		return new(editor, subject);
	}
}
