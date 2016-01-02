using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.Configuration;

namespace DragonSpark.Windows.Modularity
{
	/// <summary>
	/// A collection of <see cref="ModuleDependencyConfigurationElement"/>.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
	public class ModuleDependencyCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
		/// </summary>
		public ModuleDependencyCollection()
		{}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
		/// </summary>
		/// <param name="dependencies">An array of <see cref="ModuleDependencyConfigurationElement"/> with initial list of dependencies.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ModuleDependencyCollection( [Required]ModuleDependencyConfigurationElement[] dependencies)
		{
			dependencies.Each( BaseAdd );
		}

		///<summary>
		///Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection" />.
		///</summary>
		///<value>
		///The <see cref="T:System.Configuration.ConfigurationElementCollectionType" /> of this collection.
		///</value>
		public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

		///<summary>
		///Gets the name used to identify this collection of elements in the configuration file when overridden in a derived class.
		///</summary>
		///<value>
		///The name of the collection; otherwise, an empty string.
		///</value>
		protected override string ElementName => "dependency";

		/// <summary>
		/// Gets the <see cref="ModuleDependencyConfigurationElement"/> located at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the element in the collection.</param>
		/// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
		public ModuleDependencyConfigurationElement this[int index] => (ModuleDependencyConfigurationElement)base.BaseGet(index);

		/// <summary>
		/// Creates a new <see cref="ModuleDependencyConfigurationElement"/>.
		/// </summary>
		/// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
		protected override ConfigurationElement CreateNewElement() => new ModuleDependencyConfigurationElement();

		///<summary>
		///Gets the element key for a specified configuration element when overridden in a derived class.
		///</summary>
		///<param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for. </param>
		///<returns>
		///An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
		///</returns>
		protected override object GetElementKey(ConfigurationElement element) => ((ModuleDependencyConfigurationElement)element).ModuleName;
	}
}