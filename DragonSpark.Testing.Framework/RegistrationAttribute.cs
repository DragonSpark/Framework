using System;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationAttribute : Attribute
	{
		readonly Type to;
		readonly Type @from;

		protected RegistrationAttribute( Type from, Type to )
		{
			this.from = @from;
			this.to = to;
		}

		public Type To
		{
			get { return to; }
		}

		public Type From
		{
			get { return @from; }
		}
	}
}