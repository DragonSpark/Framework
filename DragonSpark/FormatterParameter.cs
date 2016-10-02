using System;

namespace DragonSpark
{
	public struct FormatterParameter
	{
		public FormatterParameter( object instance, string format = null, IFormatProvider provider = null )
		{
			Instance = instance;
			Format = format;
			Provider = provider;
		}

		public object Instance { get; }
		public string Format { get; }
		public IFormatProvider Provider { get; }
	}
}