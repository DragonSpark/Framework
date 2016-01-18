namespace System
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false )]
	public sealed class SerializableAttribute : Attribute {}

	[AttributeUsage( AttributeTargets.Field ), Runtime.InteropServices.ComVisible( true )]
	public sealed class NonSerializedAttribute : Attribute
	{
		public NonSerializedAttribute() { }
	}
}