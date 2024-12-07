namespace DragonSpark.Application.Entities.Initialization;

public class SchemaModification : ISchemaModification
{
	readonly IModifySchema[] _initializers;

	public SchemaModification(params IModifySchema[] initializers) => _initializers = initializers;

	public void Execute(ModelCreating parameter)
	{
		var (context, builder) = parameter;
		var type = context.GetType();
		for (byte i = 0; i < _initializers.Length; i++)
		{
			_initializers[i].Get(type)?.Execute(builder);
		}
	}
}