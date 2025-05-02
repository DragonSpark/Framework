using System;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Diagnostics;

public interface ILastChanceExceptionHandler : IConditionAware<Exception>, ITokenOperation<Exception>;