using DragonSpark.Text;
using Markdig;
using Markdig.Renderers;
using SmartFormat;
using System.IO;
using System.Text;

namespace DragonSpark.Application.Messaging
{
	public class MarkdownEmailTemplate<T> : IFormatter<T> where T : notnull
	{
		readonly string _template;

		protected MarkdownEmailTemplate(byte[] data) : this(Encoding.UTF8.GetString(data)) {}

		protected MarkdownEmailTemplate(string template) => _template = template;

		public string Get(T parameter)
		{
			var       content  = Smart.Format(_template, parameter);
			var       pipeline = new MarkdownPipelineBuilder().Build();
			var       markdown = Markdown.Parse(content, pipeline);
			using var writer   = new StringWriter();
			var       renderer = new HtmlRenderer(writer);

			pipeline.Setup(renderer);
			renderer.Render(markdown);

			var result = writer.ToString();
			return result;
		}
	}
}