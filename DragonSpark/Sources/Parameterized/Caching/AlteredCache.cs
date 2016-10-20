namespace DragonSpark.Sources.Parameterized.Caching
{
	public class AlteredCache<TInstance, TValue> : DecoratedCache<TInstance, TValue>
	{
		readonly Alter<TInstance> alteration;

		public AlteredCache( ICache<TInstance, TValue> inner, Alter<TInstance> alteration ) : base( inner )
		{
			this.alteration = alteration;
		}

		public override bool Contains( TInstance instance ) => base.Contains( alteration( instance ) );

		public override bool Remove( TInstance instance ) => base.Remove( alteration( instance ) );

		public override void Set( TInstance instance, TValue value ) => base.Set( alteration( instance ), value );

		public override TValue Get( TInstance parameter ) => base.Get( alteration( parameter )  );
	}
}