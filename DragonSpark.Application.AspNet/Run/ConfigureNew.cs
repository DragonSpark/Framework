using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Run;

public record ConfigureNew<TIn, TOut>(Func<TIn, TOut> New, ICommand<TOut> Configure);