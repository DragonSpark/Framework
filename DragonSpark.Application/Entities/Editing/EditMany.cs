using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class EditMany<TIn, T> : IEditMany<TIn, T>
{
	readonly IEdit<TIn, Leasing<T>> _edit;

	public EditMany(IEnlistedScopes context, IQuery<TIn, T> query)
		: this(context.Then().Use(query).Edit.Lease()) {}

	protected EditMany(IEdit<TIn, Leasing<T>> edit) => _edit = edit;

	public async ValueTask<ManyEdit<T>> Get(TIn parameter)
	{
		var (editor, subject) = await _edit.Await(parameter);
		return new(editor, subject);
	}
}