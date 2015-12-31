using PostSharp.Patterns.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.Modularity
{
	/// <summary>
	/// Defines the metadata that describes a module.
	/// </summary>
	[Serializable]
	public class ModuleInfo : IModuleCatalogItem
	{
		/// <summary>
		/// Initializes a new empty instance of <see cref="ModuleInfo"/>.
		/// </summary>
		public ModuleInfo()
			: this(null, null, new string[0])
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleInfo"/>.
		/// </summary>
		/// <param name="name">The module's name.</param>
		/// <param name="type">The module <see cref="Type"/>'s AssemblyQualifiedName.</param>
		/// <param name="dependsOn">The modules this instance depends on.</param>
		/// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="dependsOn"/> is <see langword="null"/>.</exception>
		public ModuleInfo(string name, string type, [Required]params string[] dependsOn)
		{
			this.ModuleName = name;
			this.ModuleType = type;
			this.DependsOn = new Collection<string>(dependsOn.ToList());
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleInfo"/>.
		/// </summary>
		/// <param name="name">The module's name.</param>
		/// <param name="type">The module's type.</param>
		public ModuleInfo(string name, string type) : this(name, type, new string[0])
		{}

		public virtual bool IsAvailable => true;

		/// <summary>
		/// Gets or sets the name of the module.
		/// </summary>
		/// <value>The name of the module.</value>
		public string ModuleName { get; set; }

		/// <summary>
		/// Gets or sets the module <see cref="Type"/>'s AssemblyQualifiedName.
		/// </summary>
		/// <value>The type of the module.</value>
		public string ModuleType { get; set; }

		/// <summary>
		/// Gets or sets the list of modules that this module depends upon.
		/// </summary>
		/// <value>The list of modules that this module depends upon.</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The setter is here to work around a Silverlight issue with setting properties from within Xaml.")]
		public Collection<string> DependsOn { get; set; }

		/// <summary>
		/// Gets or sets the state of the <see cref="ModuleInfo"/> with regards to the module loading and initialization process.
		/// </summary>
		public ModuleState State { get; set; }
	}
}
