using System;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
// 	[Serializable]
	public class DynamicModuleInfo : ModuleInfo
	{
		public DynamicModuleInfo()
		{}

		public DynamicModuleInfo( string name, string type, params string[] dependsOn ) : base( name, type, dependsOn )
		{}

		public DynamicModuleInfo( string name, string type ) : base( name, type )
		{}

		public override bool IsAvailable => InitializationMode == InitializationMode.WhenAvailable;

		/// <summary>
		/// Specifies on which stage the Module will be initialized.
		/// </summary>
		public InitializationMode InitializationMode { get; set; }

		/// <summary>
		/// Reference to the location of the module assembly.
		/// <example>The following are examples of valid <see cref="ModuleInfo.Ref"/> values:
		/// file://c:/MyProject/Modules/MyModule.dll for a loose DLL in WPF.
		/// </example>
		/// </summary>
		public string Ref { get; set; }
	}
}