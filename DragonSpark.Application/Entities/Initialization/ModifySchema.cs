using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Initialization;

public class ModifySchema : IModifySchema
{
	readonly ICondition<Type>       _allowed;
	readonly ICommand<ModelBuilder> _instance;

	protected ModifySchema(ICondition<Type> allowed, ICommand<ModelBuilder> instance)
	{
		_allowed  = allowed;
		_instance = instance;
	}

	public ICommand<ModelBuilder>? Get(Type parameter) => _allowed.Get(parameter) ? _instance : null;
}

public class ModifySchema<T> : ModifySchema where T : DbContext
{
	protected ModifySchema(Action<ModelBuilder> configure) : this(Start.A.Command(configure).Get()) {}

	protected ModifySchema(ICommand<ModelBuilder> instance) : base(Is.AssignableFrom<T>().Out(), instance) {}
}