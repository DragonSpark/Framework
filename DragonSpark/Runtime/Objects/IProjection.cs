namespace DragonSpark.Runtime.Objects;

public interface IProjection : IReadOnlyDictionary<string, object>
{
	Type InstanceType { get; }
}