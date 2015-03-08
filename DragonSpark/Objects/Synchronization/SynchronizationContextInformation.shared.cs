using System;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationContextInformation
	{
		readonly SynchronizationContainerContext context;
		readonly PropertyContext source;
		readonly PropertyContext target;
		readonly Type converterType;

		internal SynchronizationContextInformation( SynchronizationContainerContext context, PropertyContext source, PropertyContext target, Type converterType )
		{
			this.context = context;
			this.source = source;
			this.target = target;
			this.converterType = converterType;
		}

		public Type ConverterType
		{
			get { return converterType; }
		}

		public PropertyContext Target
		{
			get { return target; }
		}

		public PropertyContext Source
		{
			get { return source; }
		}

		public SynchronizationContainerContext Context
		{
			get { return context; }
		}
	}
}