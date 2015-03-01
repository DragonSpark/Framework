using System;
using DragonSpark.Extensions;

namespace DragonSpark.Markup
{
/*
	public class TypeReferenceExtension : DynamicResourceExtension
	{
		public TypeReferenceExtension()
		{}

		public TypeReferenceExtension( object resourceKey ) : base( resourceKey )
		{}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var o = base.ProvideValue(serviceProvider);
			var value = o.As<TypeExtension>();
			var result = value.Type;
			return result;
		}
	}
*/

	public class GenericExtension : GenericTypeExtension
	{
		public GenericExtension( string typeName ) : base( typeName )
		{}

		public override object ProvideValue(System.IServiceProvider serviceProvider)
		{
			var type = (Type)base.ProvideValue(serviceProvider);
			var result = type.Transform( Activator.CreateInstance );
			return result;
		}
	}
}