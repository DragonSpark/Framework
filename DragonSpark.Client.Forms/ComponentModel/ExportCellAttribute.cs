using System;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.ComponentModel
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportCellAttribute : HandlerAttribute
	{
		public ExportCellAttribute(Type handler, Type target) : base(handler, target)
		{
		}
	}
}
