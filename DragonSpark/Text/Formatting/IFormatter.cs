using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Text.Formatting
{
	public interface IFormatter : IConditional<object, IFormattable> {}
}