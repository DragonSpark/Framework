using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime.Invocation.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class LoadMembers : IStopAware<LoadMembersInput>
{
	public static LoadMembers Default { get; } = new();

	LoadMembers() : this(Members.Default) {}

	readonly ILease<Expression, MemberInfo> _members;

	public LoadMembers(ILease<Expression, MemberInfo> members) => _members = members;

	public async ValueTask Get(Stop<LoadMembersInput> parameter)
	{
		var ((expression, entry), stop) = parameter;
		using var members = _members.Get(expression);

		var current = entry.Entity;
		var context = entry.Context;

		var span  = members.AsMemory();
		
		await entry.Load(stop).Off();

		for (var i = 0; i < members.Length; i++)
		{
			var member       = span.Span[i];
			var currentEntry = context.Entry(current);
			var last         = i == span.Length - 1;
			var navigation   = currentEntry.Navigation(member.Name);

			var collection = navigation.Metadata.IsCollection;
			if (!navigation.IsLoaded)
			{
				NavigationEntry target = last && collection
					                         ? currentEntry.Collection(member.Name)
					                         : currentEntry.Reference(member.Name);
				await target.LoadAsync(stop).Off();
			}

			if (!collection)
			{
				current = navigation.CurrentValue;
				if (current is null)
				{
					break;
				}
			}
		}
	}
}