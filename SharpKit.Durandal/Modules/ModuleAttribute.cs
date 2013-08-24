using SharpKit.JavaScript;
using System;

namespace SharpKit.Durandal.Modules
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
	public class ModuleAttribute : JsTypeAttribute
	{
		public ModuleAttribute() : this( LifetimeMode.Singleton )
		{}

		public ModuleAttribute( LifetimeMode mode ) : base( JsMode.Prototype )
		{
			Name = "instance";
			LifetimeMode = mode;
		}

		public LifetimeMode LifetimeMode { get; set; }
	}
}