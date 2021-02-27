using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Data
{
	sealed class Mask : IAlteration<string>
	{
		public static Mask Default { get; } = new Mask();

		Mask() : this('.', '*') {}

		readonly char _spot, _mask;

		public Mask(char spot, char mask)
		{
			_spot = spot;
			_mask = mask;
		}

		public string Get(string parameter)
		{
			var length = parameter.Length;
			if (length >= 3)
			{
				var cap  = length > 6 ? 2 : 1;
				var caps = (parameter[..cap], parameter[^cap..]);
				var result =
					$"{caps.Item1}{new string(_spot, length - caps.Item1.Length - caps.Item2.Length)}{caps.Item2}";
				return result;
			}

			return new string(_mask, length);
		}
	}
}