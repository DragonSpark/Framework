using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Runtime.InteropServices;

namespace DragonSpark.Aspects.Alteration
{
	sealed class Adapter<T> : IAlteration
	{
		readonly IAlteration<T> alteration;

		public Adapter( IAlteration<T> alteration )
		{
			this.alteration = alteration;
		}

		public object Alter( [Optional]object parameter )
		{
			var altered = alteration.Get( parameter.As<T>() );
			var result = altered.IsAssigned() ? altered : parameter;
			return result;
		}
	}
}