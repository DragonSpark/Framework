using System.Runtime.InteropServices;

namespace DragonSpark.Sources
{
	public class SuppliedSource<T> : AssignableSourceBase<T>
	{
		T reference;

		public SuppliedSource() {}

		public SuppliedSource( T reference = default(T) )
		{
			Assign( reference );
		}

		public sealed override void Assign( [Optional]T item ) => OnAssign( item );

		protected virtual void OnAssign( T item = default(T) ) => reference = item;

		public override T Get() => reference;

		protected override void OnDispose() => reference = default(T);
	}
}