// ReSharper disable once CheckNamespace
namespace System.Windows.Markup
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class ContentPropertyAttribute : Attribute
	{
		public ContentPropertyAttribute( string name )
		{
			Name = name;
		}
		
		public string Name { get; }
	}
}