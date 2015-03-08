using System;

namespace DragonSpark.Objects.Synchronization
{
	[AttributeUsage( AttributeTargets.Property,	AllowMultiple = true )]
	public sealed class SynchronizationAttribute : Attribute
	{
		readonly Type targetType;
		readonly string expression;

		public SynchronizationAttribute( Type targetType )
		{
			this.targetType = targetType;
		}

		public SynchronizationAttribute( Type targetType, string expression )
		{
			this.targetType = targetType;
			this.expression = expression;
		}

		public Type TargetType
		{
			get { return targetType; }
		}

		public string Expression
		{
			get { return expression; }
		}

		public Type TypeConverterType { get; set; }
	}
}