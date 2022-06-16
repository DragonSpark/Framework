using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContent<T> : ICommand<T>, IResulting<T?>, IDisposable
{
	IUpdateMonitor Monitor { get; }
}