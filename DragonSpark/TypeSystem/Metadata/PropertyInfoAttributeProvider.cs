using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class PropertyInfoAttributeProvider : MethodInfoAttributeProvider
	{
		public PropertyInfoAttributeProvider( PropertyInfo property ) : base( property, property.GetMethod ) {}
	}
}