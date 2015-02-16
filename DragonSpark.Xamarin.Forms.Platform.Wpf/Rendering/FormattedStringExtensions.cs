using System.Collections.Generic;
using System.Windows.Documents;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public static class FormattedStringExtensions
	{
		public static IEnumerable<Inline> ToInlines(this FormattedString formattedString)
		{
			foreach (global::Xamarin.Forms.Span current in formattedString.Spans)
			{
				yield return current.ToRun();
			}
			yield break;
		}
		public static Run ToRun(this global::Xamarin.Forms.Span span)
		{
			Run run = new Run
			{
				Text = span.Text
			};
			if (span.ForegroundColor != Color.Default)
			{
				run.Foreground = span.ForegroundColor.ToBrush();
			}
			if (!span.IsDefault())
			{
#pragma warning disable 618
				run.ApplyFont(span.Font);
#pragma warning restore 618
			}
			return run;
		}
	}
}
