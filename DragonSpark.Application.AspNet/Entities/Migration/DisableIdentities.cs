using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class DisableIdentities : ICommand<ModelBuilder>
{
	public static DisableIdentities Default { get; } = new();

	DisableIdentities() {}
	
	public void Execute(ModelBuilder parameter)
	{
		foreach (var entityType in parameter.Model.GetEntityTypes())
		{
			var pk = entityType.FindPrimaryKey();
			if (pk is not null)
			{
				foreach (var property in pk.Properties.Where(p => p.ClrType == typeof(int) &&
				                                                  p.ValueGenerated == ValueGenerated.OnAdd))
				{
					property.ValueGenerated = ValueGenerated.Never;
				}
				
				foreach (var property in pk.Properties.Where(p => p.ClrType == typeof(Guid)))
				{
					property.SetValueGeneratorFactory((_, _) => SmartGuidGenerator.Default);
					property.ValueGenerated = ValueGenerated.Never;
				}
			}
		}
	}
}