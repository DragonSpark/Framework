using System;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportRendererAttribute : HandlerAttribute
	{
		public ExportRendererAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}
