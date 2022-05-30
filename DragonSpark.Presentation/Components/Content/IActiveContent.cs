using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContent<T> : IResulting<T?>
{
	IOperation<Action> Refresh { get; }
}