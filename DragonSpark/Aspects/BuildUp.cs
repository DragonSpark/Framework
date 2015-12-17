using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;

namespace DragonSpark.Aspects
{
	[PSerializable]
	public class BuildUp : InstanceLevelAspect
	{
		[OnInstanceConstructedAdvice]
		public void OnInstanceConstructed()
		{
			Instance.BuildUp();
		}
	}
}