using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects.Alteration
{
	sealed class Adapter<T> : IAlteration
	{
		readonly IAlteration<T> alteration;

		public Adapter( IAlteration<T> alteration )
		{
			this.alteration = alteration;
		}

		public object Alter( object parameter ) => parameter is T ? alteration.Get( (T)parameter ) : parameter;
	}
}