using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class DisableConcurrency : ICommand<ModelBuilder>
{
	public static DisableConcurrency Default { get; } = new();

	DisableConcurrency() {}
	
	public void Execute(ModelBuilder parameter)
	{
		foreach (var entity in parameter.Model.GetEntityTypes())
		{
			foreach (var property in entity.GetProperties())
			{
				if (property.IsConcurrencyToken)
				{
					property.IsConcurrencyToken = false;
					property.ValueGenerated     = ValueGenerated.Never;
				}
			}
		}
	}
}