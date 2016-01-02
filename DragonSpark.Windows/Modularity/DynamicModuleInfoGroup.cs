using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
	public class DynamicModuleInfoGroup : ModuleInfoGroup
	{
		/// <summary>
		/// Gets or sets the <see cref="DynamicModuleInfo.InitializationMode"/> for the whole group. Any <see cref="ModuleInfo"/> classes that are
		/// added after setting this value will also get this <see cref="InitializationMode"/>.
		/// </summary>
		/// <see cref="DynamicModuleInfo.InitializationMode"/>
		/// <value>The initialization mode.</value>
		public InitializationMode InitializationMode { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="DynamicModuleInfo.Ref"/> value for the whole group. Any <see cref="DynamicModuleInfo"/> classes that are
		/// added after setting this value will also get this <see cref="Ref"/>.
		/// 
		/// The ref value will also be used by the <see cref="IModuleManager"/> to determine which  <see cref="IModuleTypeLoader"/> to use. 
		/// For example, using an "file://" prefix with a valid URL will cause the ModuleTypeLoader to be used
		/// (Only available in the desktop version of CAL).
		/// </summary>
		/// <see cref="DynamicModuleInfo.Ref"/>
		/// <value>The ref value that will be used.</value>
		public string Ref { get; set; }

		protected override void ForwardValues( ModuleInfo moduleInfo )
		{
			base.ForwardValues( moduleInfo );

			var dynamicModuleInfo = moduleInfo as DynamicModuleInfo;
			if ( dynamicModuleInfo != null )
			{
				if (dynamicModuleInfo.Ref == null)
				{
					dynamicModuleInfo.Ref = this.Ref;
				}

				if (dynamicModuleInfo.InitializationMode == InitializationMode.WhenAvailable && this.InitializationMode != InitializationMode.WhenAvailable)
				{
					dynamicModuleInfo.InitializationMode = this.InitializationMode;
				}
			}
		}
	}
}