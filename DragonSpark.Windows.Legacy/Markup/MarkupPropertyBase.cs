namespace DragonSpark.Windows.Legacy.Markup
{
	public abstract class MarkupPropertyBase : IMarkupProperty
	{
		protected MarkupPropertyBase( PropertyReference reference )
		{
			Reference = reference;
		}

		public PropertyReference Reference { get; }

		public object GetValue() => OnGetValue();

		protected abstract object OnGetValue();

		public object SetValue( object value ) => Apply( value );

		protected abstract object Apply( object value );
	}
}