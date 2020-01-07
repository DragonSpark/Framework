using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Model.Aspects
{
	public interface IRegistration : IConditional<Array<Type>, object> {}
}