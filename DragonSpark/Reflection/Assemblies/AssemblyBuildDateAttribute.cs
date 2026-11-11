namespace DragonSpark.Reflection.Assemblies;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AssemblyBuildDateAttribute : Attribute
{
	public AssemblyBuildDateAttribute(string value) 
		: this(DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture)) {}

	public AssemblyBuildDateAttribute(DateTimeOffset on) => On = on;

	public DateTimeOffset On { get; }
}