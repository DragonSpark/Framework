using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity;

sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
{
	readonly IUsers<T> _users;
	readonly ITime     _time;

	public MarkModified(IUsers<T> edit) : this(edit, Time.Default) {}

	public MarkModified(IUsers<T> users, ITime time)
	{
		_users = users;
		_time  = time;
	}

	public async ValueTask Get(T parameter)
	{
		using var users   = _users.Get();
		var       subject = await users.Subject.FindByIdAsync(parameter.Id.ToString()).ConfigureAwait(false);
		subject.Modified = _time.Get();
		await users.Subject.UpdateAsync(subject).ConfigureAwait(false);
	}
}