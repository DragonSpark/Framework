using DragonSpark.Model.Results;

namespace DragonSpark.Diagnostics.Logging
{
	public readonly struct ScalarProperty : IResult<string>
	{
		readonly IFormats _formats;

		public ScalarProperty(string key, IFormats formats, object instance)
		{
			Key      = key;
			_formats = formats;
			Instance = instance;
		}

		public string Key { get; }

		public object Instance { get; }

		public string Get() => _formats.Get(Key);
	}
}