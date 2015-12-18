using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace DragonSpark.IoC.Commands
{
	public class StrategyReference
	{
		public Type StrategyType { get; set; }

		public UnityBuildStage Stage { get; set; }
	}
}