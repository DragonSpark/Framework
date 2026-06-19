using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Runtime;

public sealed class IsDeployedEnvironment : Condition
{
	public IsDeployedEnvironment(IHostEnvironment environment) : base(_ => !environment.IsDevelopment()) {}
}