using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Data
{
	sealed class Mask : IAlteration<string>
	{
		public static Mask Default { get; } = new Mask();

		Mask() : this(new string('.', 3), '*') {}

		readonly string _spot;
		readonly char   _mask;

		public Mask(string spot, char mask)
		{
			_spot = spot;
			_mask = mask;
		}

		public string Get(string parameter)
		{
			var length = parameter.Length;
			if (length >= 3)
			{
				var cap  = length > 12 ? 3 : length > 6 ? 2 : 1;
				var result = $"{parameter[..cap]}{_spot}{parameter[^cap..]}";
				return result;
			}

			return new string(_mask, length);
		}
	}
}