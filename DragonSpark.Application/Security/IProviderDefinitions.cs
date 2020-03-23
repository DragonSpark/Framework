using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Security {
	public interface IProviderDefinitions : IConditional<string, ProviderDefinition> {}
}