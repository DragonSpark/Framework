using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Activation;

sealed class SingletonCandidates : Instances<string>, ISingletonCandidates
{
	public static SingletonCandidates Default { get; } = new SingletonCandidates();

	SingletonCandidates() : this("Default", "Instance", "Implementation", "Singleton") {}

	public SingletonCandidates(params string[] items) : base(items) {}
}