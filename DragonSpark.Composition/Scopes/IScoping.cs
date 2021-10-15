using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes;

public interface IScoping : IResult<AsyncServiceScope> {}