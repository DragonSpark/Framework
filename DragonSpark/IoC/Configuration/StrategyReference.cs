using System;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.IoC.Configuration
{
	public class StrategyReference
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type StrategyType { get; set; }

		public UnityBuildStage Stage { get; set; }
	}
}