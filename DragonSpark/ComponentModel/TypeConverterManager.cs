using DragonSpark.Extensions;
using System;
using System.ComponentModel;

namespace DragonSpark.ComponentModel
{
	class TypeConverterManager : IDisposable
	{
		readonly Type targetType;
		TypeDescriptionProvider descriptionProvider;

		public TypeConverterManager( Type targetType, Type converterType )
		{
			this.targetType = targetType;
			descriptionProvider = converterType.Transform( x => TypeDescriptor.AddAttributes( targetType, new Attribute[] { new TypeConverterAttribute( x ) } ) );
		}

		void IDisposable.Dispose()
		{
			if ( descriptionProvider != null )
			{
				TypeDescriptor.RemoveProvider( descriptionProvider, targetType );
				descriptionProvider = null;
			}
		}
	}
}