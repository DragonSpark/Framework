using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.TypeSystem
{
	public abstract class HostingAttribute : Attribute, IFactoryWithParameter
	{
		readonly Func<object, object> inner;

		protected HostingAttribute( [Required]Func<object, object> inner )
		{
			this.inner = inner;
		}

		public object Create( object parameter ) => inner( parameter );
	}
}