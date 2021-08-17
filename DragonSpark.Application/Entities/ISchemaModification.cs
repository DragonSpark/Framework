using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public interface ISchemaModification : IAlteration<ModelBuilder> {}

	public class SchemaModification : Alteration<ModelBuilder>, ISchemaModification
	{
		public SchemaModification(params IInitializer[] initializers) : base(initializers.Alter) {}
	}
}