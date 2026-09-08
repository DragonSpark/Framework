using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime.Invocation.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
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

		var span = members.AsMemory();

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

// TODO
public readonly record struct AssignInput(PropertyValues From, PropertyValues To)
{
	public AssignInput(EntityEntry From, EntityEntry To) : this(From.CurrentValues, To.CurrentValues) {}
}

sealed class Assign : ICommand<AssignInput>
{
	public static Assign Default { get; } = new();

	Assign() {}

	public void Execute(AssignInput parameter)
	{
		var (from, to) = parameter;

		foreach (var property in from.Properties)
		{
			var name = property.Name;
			if (name is not "Id" && name != to.StructuralType.GetDiscriminatorPropertyName() &&
			    Allow(property, to.Properties, name))
			{
				to[name] = from[name];
			}
		}
	}

	static bool Allow(IProperty source, IReadOnlyList<IProperty> properties, string name)
	{
		for (var i = 0; i < properties.Count; i++)
		{
			var destination = properties[i];
			if (destination.Name == name && destination.ClrType.IsAssignableTo(source.ClrType))
			{
				return true;
			}
		}

		return false;
	}
}

// TODO
public readonly record struct ApplyInput<T>(DbContext Context, EntityEntry<T> Entry) where T : class;

sealed class Applied<T> : ISelect<ApplyInput<T>, EntityEntry<T>> where T : class
{
	public static Applied<T> Default { get; } = new();

	Applied() {}

	public EntityEntry<T> Get(ApplyInput<T> parameter)
	{
		var (context, entry) = parameter;

		if (!Equals(entry.Context, context))
		{
			var current = context.Entry(entry.Entity);
			var attach  = current is { State: EntityState.Detached } ? context.Attach(entry.Entity) : current;
			return attach.Assigned(entry);
		}

		return entry;
	}
}