using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text
{
	public sealed class NullOrEmpty : IAlteration<string>
	{
		public static NullOrEmpty Default { get; } = new NullOrEmpty();

		NullOrEmpty() {}

		public string Get(string parameter) => parameter ?? string.Empty;
	}
}