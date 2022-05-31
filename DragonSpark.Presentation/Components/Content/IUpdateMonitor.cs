using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components.Content;

public interface IUpdateMonitor : IOperation<Action>, IMutable<bool> {}