using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.Entities.Initialization;

public sealed class ApplySchemaModification : ICommand<ModelCreating>
{
	public static ApplySchemaModification Default { get; } = new ApplySchemaModification();

	ApplySchemaModification() {}

	public void Execute(ModelCreating parameter)
	{
		var (context, _) = parameter;
		context.GetService<ISchemaModification>().Execute(parameter);
	}
}