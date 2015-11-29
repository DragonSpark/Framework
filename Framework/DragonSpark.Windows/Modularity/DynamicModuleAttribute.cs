using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
	public class DynamicModuleAttribute : ModuleAttribute
	{
		/// <summary>
		/// Gets or sets the value indicating whether the module should be loaded OnDemand.
		/// </summary>
		/// When <see langword="false"/> (default value), it indicates the module should be loaded as soon as it's dependencies are satisfied.
		/// Otherwise you should explicitily load this module via the <see cref="ModuleManager"/>.
		public bool OnDemand { get; set; }
	}
}