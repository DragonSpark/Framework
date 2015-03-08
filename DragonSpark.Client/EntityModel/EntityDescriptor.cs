using System;
using System.ComponentModel;
// using System.Windows.Ria;
using Common.Configuration;

namespace Common.EntityModel
{
	public class EntityDescriptor
	{
		public string Title { get; set; }

		public string ItemName { get; set; }

		public string ItemNamePlural { get; set; }

		public string Description { get; set; }

		[TypeConverter( typeof(TypeNameConverter) )]
		public Type Type { get; set; }

		public IEntityService Service { get; set; }
	}
}