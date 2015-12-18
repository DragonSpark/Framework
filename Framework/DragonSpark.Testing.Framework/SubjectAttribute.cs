using System;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method )]
	public class SubjectAttribute : Attribute
	{
		readonly string name;

		public SubjectAttribute( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}
	}
}