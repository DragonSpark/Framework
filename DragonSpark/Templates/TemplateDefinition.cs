using System;
using System.Collections.ObjectModel;

namespace DragonSpark.Templates
{
	public class TemplateDefinition : Collection<Type>
	{
		public string TemplateTypeName { get; set; }
	}
}