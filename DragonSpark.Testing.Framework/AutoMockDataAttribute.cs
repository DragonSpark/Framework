using System;
using System.Linq;
using DragonSpark.Extensions;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework
{
	public class AutoMockDataAttribute : CustomizedAutoDataAttribute
	{
		public AutoMockDataAttribute( params Type[] cutomizationTypes ) : base( typeof(AutoMoqCustomization).Append( cutomizationTypes ).ToArray() )
		{}
	}
}