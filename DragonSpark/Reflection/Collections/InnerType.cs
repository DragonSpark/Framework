using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Reflection.Collections
{
	public sealed class InnerType : IAlteration<TypeInfo>
	{
		public static InnerType Default { get; } = new InnerType();

		InnerType() : this(Always<TypeInfo>.Default) {}

		readonly Func<TypeInfo, bool> _condition;

		readonly Func<TypeInfo, Array<TypeInfo>> _hierarchy;
		readonly Func<Type[], TypeInfo>          _select;

		public InnerType(ICondition<TypeInfo> condition)
			: this(HasGenericArguments.Default.Then().And(condition), TypeHierarchy.Default.Get,
			       Start.A.Selection<Type>().As.Sequence.Open.By.Self.Query().Only().Then().Metadata()) {}

		public InnerType(Func<TypeInfo, bool> condition, Func<TypeInfo, Array<TypeInfo>> hierarchy,
		                 Func<Type[], TypeInfo> select)
		{
			_condition = condition;
			_hierarchy = hierarchy;
			_select    = select;
		}

		public TypeInfo Get(TypeInfo parameter)
		{
			var hierarchy = _hierarchy(parameter).Open();
			foreach (var info in hierarchy)
			{
				var result = _condition(info)
					             ? _select(info.GenericTypeArguments)
					             : info.IsArray
						             ? info.GetElementType()
						                   .GetTypeInfo()
						             : null;
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}
	}
}