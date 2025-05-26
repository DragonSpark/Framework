using System;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Diagnostics;

public interface ILastChanceExceptionHandler : IConditionAware<Exception>, IStopAware<Exception>;