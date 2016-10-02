using System;

namespace DragonSpark.TypeSystem
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class AssemblyPartsAttribute : Attribute
	{
		public const string Default = "{0}.Parts.*";

		public AssemblyPartsAttribute() : this( Default ) {}

		public AssemblyPartsAttribute( string query )
		{
			Query = query;
		}

		public string Query { get; }
	}
}