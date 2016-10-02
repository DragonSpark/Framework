namespace DragonSpark.Sources
{
	public class SuppliedSource<T> : AssignableSourceBase<T>
	{
		T reference;

		public SuppliedSource() {}

		public SuppliedSource( T reference )
		{
			Assign( reference );
		}

		public sealed override void Assign( T item ) => OnAssign( item );

		protected virtual void OnAssign( T item ) => reference = item;

		public override T Get() => reference;

		protected override void OnDispose() => reference = default(T);
	}
}