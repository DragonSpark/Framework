using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public class ModifySchema : IModifySchema
	{
		readonly ICondition<Type>          _allowed;
		readonly IAlteration<ModelBuilder> _instance;

		protected ModifySchema(ICondition<Type> allowed, IAlteration<ModelBuilder> instance)
		{
			_allowed  = allowed;
			_instance = instance;
		}

		public IAlteration<ModelBuilder>? Get(Type parameter) => _allowed.Get(parameter) ? _instance : null;
	}

	public class ModifySchema<T> : ModifySchema where T : DbContext
	{
		protected ModifySchema(Func<ModelBuilder, ModelBuilder> configure)
			: this(Start.A.Selection(configure).Then().Out()) {}

		protected ModifySchema(IAlteration<ModelBuilder> instance) : base(Is.AssignableFrom<T>().Out(), instance) {}
	}
}