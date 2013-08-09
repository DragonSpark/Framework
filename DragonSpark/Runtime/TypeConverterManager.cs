using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	class TypeConverterManager : IDisposable
	{
		readonly Type targetType;
		TypeDescriptionProvider descriptionProvider;

		public TypeConverterManager( Type targetType, Type converterType )
		{
			Contract.Requires( targetType != null );
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