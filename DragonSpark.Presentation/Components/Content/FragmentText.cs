using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using NetFabric.Hyperlinq;
using System.Text;

namespace DragonSpark.Presentation.Components.Content
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/dotnet/aspnetcore/issues/16218#issuecomment-552052798
	/// </summary>
	public sealed class FragmentText : ISelect<RenderFragment, string>
	{
		public static FragmentText Default { get; } = new FragmentText();

		FragmentText() {}

		public string Get(RenderFragment parameter)
		{
#pragma warning disable BL0006 // Do not use RenderTree types
			var content = new StringBuilder();
			var builder = new RenderTreeBuilder();

			parameter(builder);

			foreach (var frame in builder.GetFrames()
			                             .Array.AsValueEnumerable()
			                             .Where(x => x.FrameType == RenderTreeFrameType.Markup
			                                         || x.FrameType == RenderTreeFrameType.Text)
			                             .Select(x => x.MarkupContent.Trim())
			                             .Where(x => !string.IsNullOrEmpty(x)))
			{
				content.Append(frame);
			}

			var result = content.ToString().Trim();

			return result;
#pragma warning restore BL0006 // Do not use RenderTree types
		}
	}
}