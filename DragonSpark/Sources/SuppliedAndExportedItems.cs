namespace DragonSpark.Sources
{
	public class SuppliedAndExportedItems<T> : CompositeItemSource<T>
	{
		public SuppliedAndExportedItems( params T[] configurators ) : base( new ItemSource<T>( configurators ), ExportSource<T>.Default ) {}
	}
}