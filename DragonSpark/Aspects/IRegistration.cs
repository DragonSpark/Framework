using System;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Aspects
{
	public interface IRegistration : IConditional<Array<Type>, object> {}
}