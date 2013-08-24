using SharpKit.JavaScript;
using System;

namespace SharpKit.Durandal.Modules
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public class ModuleRegistration : JsEmbeddedResourceAttribute
	{
		readonly Type type;

		public ModuleRegistration( Type type, string filename ) : base( filename )
		{
			this.type = type;
		}

		public Type Type
		{
			get { return type; }
		}
	}
}