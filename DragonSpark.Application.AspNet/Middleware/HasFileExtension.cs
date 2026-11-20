using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Middleware;

sealed class HasFileExtension : ICondition<string>
{
	public static HasFileExtension Default { get; } = new();

	HasFileExtension() {}

	public bool Get(string parameter)
	{
		var dot = parameter.LastIndexOf('.');
		return dot > 0 && dot < parameter.Length - 1 && dot > parameter.LastIndexOf('/');
	}
}