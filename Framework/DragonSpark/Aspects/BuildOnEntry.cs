using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace DragonSpark.Aspects
{
	[PSerializable]
	public class BuildOnEntry : OnMethodBoundaryAspect
	{
		public override void OnEntry( MethodExecutionArgs args )
		{
			base.OnEntry( args );
			args.Instance.BuildUp();
		}
	}
}