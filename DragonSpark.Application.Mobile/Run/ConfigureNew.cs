using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Run;

public record ConfigureNew<TIn, TOut>(Func<TIn, TOut> New, ICommand<TOut> Configure);
