namespace DragonSpark.Runtime.Environment
{
	sealed class LocateGuard<T> : AssignedGuard<T>
	{
		public static LocateGuard<T> Default { get; } = new LocateGuard<T>();

		LocateGuard()
			: base(x => $"Could not locate an external/environmental component type for {x}.  Please ensure there is a primary assembly registered with an applied attribute of type DragonSpark.Runtime.Environment.HostingAttribute, and that there is a corresponding assembly either named <PrimaryAssemblyName>.Environment for environmental-specific components. Please also ensure that the component libraries contains one public type that implements or is of the requested type.") {}
	}
}