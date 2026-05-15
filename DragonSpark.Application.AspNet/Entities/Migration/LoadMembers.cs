using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

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

public sealed class DisableConcurrency : ICommand<ModelBuilder>
{
	public static DisableConcurrency Default { get; } = new();

	DisableConcurrency() {}
	
	public void Execute(ModelBuilder parameter)
	{
		foreach (var entity in parameter.Model.GetEntityTypes())
		{
			foreach (var property in entity.GetProperties())
			{
				if (property.IsConcurrencyToken)
				{
					property.IsConcurrencyToken = false;
					property.ValueGenerated     = ValueGenerated.Never;
				}
			}
		}
	}
}

public sealed class DisableIdentities : ICommand<ModelBuilder>
{
	public static DisableIdentities Default { get; } = new();

	DisableIdentities() {}
	
	public void Execute(ModelBuilder parameter)
	{
		foreach (var entityType in parameter.Model.GetEntityTypes())
		{
			var pk = entityType.FindPrimaryKey();
			if (pk is not null)
			{
				foreach (var property in pk.Properties.Where(p => p.ClrType == typeof(int) &&
				                                                  p.ValueGenerated == ValueGenerated.OnAdd))
				{
					property.ValueGenerated = ValueGenerated.Never;
				}
				
				foreach (var property in pk.Properties.Where(p => p.ClrType == typeof(Guid)))
				{
					property.SetValueGeneratorFactory((_, _) => SmartGuidGenerator.Default);
					property.ValueGenerated = ValueGenerated.Never;
				}
			}
		}
	}
}
sealed class SmartGuidGenerator : ValueGenerator<Guid>
{
	public static SmartGuidGenerator Default { get; } = new();

	SmartGuidGenerator() {}
	
	public override bool GeneratesTemporaryValues => false;

	public override Guid Next(EntityEntry entry)
	{
		var value = entry.Property("Id").CurrentValue;
		return value is Guid identity && identity != Guid.Empty ? identity : Guid.NewGuid();
	}
}