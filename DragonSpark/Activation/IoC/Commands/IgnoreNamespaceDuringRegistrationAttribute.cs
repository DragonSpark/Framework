using System;

namespace DragonSpark.Activation.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public sealed class IgnoreNamespaceDuringRegistrationAttribute : Attribute
	{
		readonly string ns;

		public IgnoreNamespaceDuringRegistrationAttribute( Type type ) : this( type.Namespace )
		{}

		public IgnoreNamespaceDuringRegistrationAttribute( string @namespace )
		{
			ns = @namespace;
		}

		public string Namespace
		{
			get { return ns; }
		}
	}
}