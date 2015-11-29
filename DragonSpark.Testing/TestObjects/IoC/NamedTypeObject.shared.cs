using System;
using System.ComponentModel;
using System.Configuration;

namespace DragonSpark.Testing.TestObjects.IoC
{
	public class NamedTypeObject : INamedTypeObject
	{
		public string Name { get; set; }
		
		[TypeConverter( typeof(TypeNameConverter) )]
		public Type Type { get; set; }
	}
}