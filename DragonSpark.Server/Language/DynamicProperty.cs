using System;

namespace DragonSpark.Runtime
{
	public class DynamicProperty
	{
		#region Fields
		private readonly string name;

		private readonly Type type;
		#endregion

		#region Properties
		public string Name
		{
			get { return name; }
		}

		public Type Type
		{
			get { return type; }
		}
		#endregion

		public DynamicProperty(string name, Type type)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (type == null)
				throw new ArgumentNullException("type");
			this.name = name;
			this.type = type;
		}
	}
}