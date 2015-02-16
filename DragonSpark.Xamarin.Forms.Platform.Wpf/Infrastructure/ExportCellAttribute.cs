using System;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Infrastructure
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportCellAttribute : HandlerAttribute
	{
		public ExportCellAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}
