using System;
using System.ComponentModel;

namespace DragonSpark.Application.Presentation.View
{
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class ModelActionAttribute : DescriptionAttribute
	{
		public ModelActionAttribute( string description ) : base( description )
		{}
	}
}
