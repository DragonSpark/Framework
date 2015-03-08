using System;

namespace DragonSpark.Application.Assemblies
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false )]
	public sealed class SupportDescriptionAttribute : Attribute
	{
		readonly string description;

		public SupportDescriptionAttribute( string description )
		{
			this.description = description;
		}

		public string Description
		{
			get { return description; }
		}
	}
}