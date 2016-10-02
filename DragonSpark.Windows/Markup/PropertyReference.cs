using System;
using System.Reflection;
using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
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