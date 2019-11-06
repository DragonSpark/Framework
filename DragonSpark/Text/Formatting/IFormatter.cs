using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Text.Formatting
{
	public interface IFormatter : IConditional<object, IFormattable> {}
}