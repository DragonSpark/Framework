using System;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Infrastructure
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportRendererAttribute : HandlerAttribute
	{
		public ExportRendererAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}
