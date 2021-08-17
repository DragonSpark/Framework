using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public sealed class DefaultSchemaModification : Alteration<ModelBuilder>, ISchemaModification
	{
		public static DefaultSchemaModification Default { get; } = new DefaultSchemaModification();

		DefaultSchemaModification() : base(new SchemaModification()) {}
	}
}