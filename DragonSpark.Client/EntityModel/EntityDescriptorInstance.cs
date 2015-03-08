namespace Common.EntityModel
{
	public class EntityDescriptorInstance
	{
		readonly EntityDescriptor descriptor;
		readonly System.Windows.Ria.Entity instance;

		public EntityDescriptorInstance( EntityDescriptor descriptor, System.Windows.Ria.Entity instance )
		{
			this.descriptor = descriptor;
			this.instance = instance;
		}

		public EntityDescriptor Descriptor
		{
			get { return descriptor; }
		}

		public System.Windows.Ria.Entity Instance
		{
			get { return instance; }
		}
	}
}