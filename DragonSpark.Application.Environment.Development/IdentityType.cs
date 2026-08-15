using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Environment.Development;

sealed class IdentityType : ICondition<Type>
{
	public static IdentityType Default { get; } = new();

	IdentityType() : this("Microsoft.AspNetCore.Identity") {}

	readonly string _namespace;

	public IdentityType(string @namespace) => _namespace = @namespace;

	public bool Get(Type parameter) => parameter.Namespace?.StartsWith(_namespace) == true;
}