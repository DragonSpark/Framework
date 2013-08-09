namespace DragonSpark.Objects
{
	public class ObjectResolvedEventArgs : ObjectCreatingEventArgs
	{
		readonly object target;

		public ObjectResolvedEventArgs( object source, object target ) : base( source )
		{
			this.target = target;
		}

		public object Target
		{
			get { return target; }
		}
	}
}