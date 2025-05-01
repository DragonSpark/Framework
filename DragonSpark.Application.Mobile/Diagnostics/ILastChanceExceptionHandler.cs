using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Diagnostics;

public interface ILastChanceExceptionHandler : ICondition<Exception>, ICommand<Exception>;