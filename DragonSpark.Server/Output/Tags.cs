using DragonSpark.Application.Model;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Output;

sealed class Tags : ITags
{
	public static Tags Default { get; } = new();

	Tags() {}

	public ValueTask Get(Stop<ComposeTagsInput> parameter)
	{
		var ((subject, key, results), _) = parameter;

		if (key is IUserOutputKey k && subject is IUserIdentity u)
		{
			results.Add(k.Get(u.Get()));
		}
		else
			switch (results.Count)
			{
				case 0:
					results.Add(key.Get());
					break;
			}

		return ValueTask.CompletedTask;
	}
}