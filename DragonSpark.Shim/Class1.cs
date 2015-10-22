namespace System.Windows.Markup
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property )]
	public sealed class AmbientAttribute : Attribute
	{}

	[AttributeUsage( AttributeTargets.Class )]
    public sealed class ContentPropertyAttribute : Attribute
    {
		public ContentPropertyAttribute()
        {}
        
        public ContentPropertyAttribute( string name )
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}

namespace System
{
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate,
		Inherited = false )]
    public sealed class SerializableAttribute : Attribute
    {
    }
}
