using DragonSpark.TypeSystem;
using System;
using System.Reflection;
using System.Windows;

namespace DragonSpark.Windows.Legacy.Markup
{
	public struct PropertyReference
	{
		public static PropertyReference New( MemberInfo member ) => new PropertyReference( member.DeclaringType, member.GetMemberType(), member.Name );
		public static PropertyReference New( DependencyProperty property ) => new PropertyReference( property.OwnerType, property.PropertyType, property.Name );

		public PropertyReference( Type declaringType, Type propertyType, string propertyName )
		{
			DeclaringType = declaringType;
			PropertyType = propertyType;
			PropertyName = propertyName;
		}

		public Type DeclaringType { get; }
		public Type PropertyType { get; }
		public string PropertyName { get; }
	}
}