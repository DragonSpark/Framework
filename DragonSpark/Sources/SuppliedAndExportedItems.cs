namespace DragonSpark.Sources
{
	public class SuppliedAndExportedItems<T> : CompositeItemSource<T>
	{
		public SuppliedAndExportedItems( params T[] alterations ) : base( new ItemSource<T>( alterations ), ExportSource<T>.Default ) {}
	}
}