using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Uno.Run;

public record ConfigureNew<TIn, TOut>(Func<TIn, TOut> New, ICommand<TOut> Configure);
