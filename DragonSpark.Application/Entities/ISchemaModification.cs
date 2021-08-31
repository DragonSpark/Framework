using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public readonly record struct SchemaInput(Type ContextType, ModelBuilder Subject);

	public interface ISchemaModification : ISelect<SchemaInput, ModelBuilder> {}

	public class SchemaModification : ISchemaModification
	{
		readonly IModifySchema[] _initializers;

		public SchemaModification(params IModifySchema[] initializers) => _initializers = initializers;

		public ModelBuilder Get(SchemaInput parameter)
		{
			var (contextType, result) = parameter;

			for (byte i = 0; i < _initializers.Length; i++)
			{
				result = _initializers[i].Get(contextType)?.Get(result) ?? result;
			}

			return result;
		}
	}
}