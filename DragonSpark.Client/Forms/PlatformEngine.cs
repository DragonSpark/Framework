using DragonSpark.Application.Forms.Rendering;
using DragonSpark.Extensions;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms
{
	public class PlatformEngine : IPlatformEngine
	{
		public SizeRequest GetNativeSize( VisualElement view, double widthConstraint, double heightConstraint )
		{
			var result = widthConstraint > 0.0 && heightConstraint > 0.0 ? view.GetRenderer().Transform( x => x.GetDesiredSize( widthConstraint, heightConstraint ) ) : default(SizeRequest);
			return result;
		}

		public bool Supports3D
		{
			get { return true; }
		}
	}
}