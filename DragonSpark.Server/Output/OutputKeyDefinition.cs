namespace DragonSpark.Server.Output;

sealed class OutputKeyDefinition<T> : Text.Text, IOutputKeyDefinition<T>
{
	readonly IOutputKey<T> _instance;

	public OutputKeyDefinition(IOutputKey<T> instance) : base(instance.Name) => _instance = instance;

	public IOutputKey<T> Get(IServiceProvider parameter) => _instance;
}