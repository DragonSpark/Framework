using System;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Infrastructure
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportImageSourceHandlerAttribute : HandlerAttribute
	{
		public ExportImageSourceHandlerAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}
