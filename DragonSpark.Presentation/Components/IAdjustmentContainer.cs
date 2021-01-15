using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Presentation.Components
{
	public interface IAdjustmentContainer<T> : ISelect<IAdjust<T>, IDisposable>, ICommand {}
}