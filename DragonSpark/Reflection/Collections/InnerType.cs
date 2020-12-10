using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Collections
{
	public sealed class InnerType : Select<TypeInfo, TypeInfo?>
	{
		public static InnerType Default { get; } = new InnerType();

		InnerType() : this(Always<TypeInfo>.Default) {}

		public InnerType(ICondition<TypeInfo> condition) : base(Start.An.Instance(HasGenericArguments.Default)
		                                                             .Then()
		                                                             .And(condition)
		                                                             .To(x => new Implementation(x))
		                                                             .Then()
		                                                             .Stores()
		                                                             .New()
		                                                             .Select(x => x.Account())) {}

		sealed class Implementation : IAlteration<TypeInfo>
		{
			readonly Func<TypeInfo, bool>                  _condition;
			readonly Func<TypeInfo, IEnumerable<TypeInfo>> _hierarchy;
			readonly Func<Type[], TypeInfo>                _select;

			public Implementation(Func<TypeInfo, bool> condition)
				: this(condition, Start.A.Selection<Type>()
				                       .As.Sequence.Open.By.Self.Then()
				                       .Only()
				                       .Then()
				                       .Metadata()) {}

			public Implementation(Func<TypeInfo, bool> condition, Func<Type[], TypeInfo> select)
				: this(condition, TypeHierarchy.Default.Get, @select) {}

			public Implementation(Func<TypeInfo, bool> condition, Func<TypeInfo, IEnumerable<TypeInfo>> hierarchy,
			                      Func<Type[], TypeInfo> select)
			{
				_condition = condition;
				_hierarchy = hierarchy;
				_select    = select;
			}

			public TypeInfo Get(TypeInfo parameter)
			{
				foreach (var element in _hierarchy(parameter).AsValueEnumerable())
				{
					var metadata = element.Verify();
					var result = _condition(metadata)
						             ? _select(metadata.GenericTypeArguments)
						             : metadata.IsArray
							             ? metadata.GetElementType().Verify().GetTypeInfo()
							             : null;
					if (result != null)
					{
						return result;
					}
				}

				return default!;
			}
		}
	}
}