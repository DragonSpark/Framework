using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text;

public sealed class FirstWord : IAlteration<string>
{
	public static FirstWord Default { get; } = new();

	FirstWord() {}

	public string Get(string parameter)
	{
		if (!parameter.IsNullOrEmpty())
		{
			for (var i = 1; i < parameter.Length; i++)
			{
				if (char.IsUpper(parameter[i]))
				{
					return parameter[..i];
				}
			}
		}

		return parameter;
	}
}