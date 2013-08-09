using System;
using System.Collections.Generic;
using System.Windows;

namespace DragonSpark.Configuration
{
	/// <summary>
	/// The shared resource dictionary is a specialized resource dictionary
	/// that loads it content only once. If a second instance with the same source
	/// is created, it only merges the resources from the cache.
	/// </summary>
	public class SharedResourceDictionary : ResourceDictionary
	{
		/// <summary>
		/// Internal cache of loaded dictionaries 
		/// </summary>
		static readonly Dictionary<Uri, ResourceDictionary> SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

		/// <summary>
		/// Local member of the source uri
		/// </summary>
		Uri sourceUri;

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) to load resources from.
		/// </summary>
		public new Uri Source
		{
			get { return sourceUri; }
			set
			{
				sourceUri = value;

				if ( !SharedDictionaries.ContainsKey( value ) )
				{
					// If the dictionary is not yet loaded, load it by setting
					// the source of the base class
					base.Source = value;

					// add it to the cache
					SharedDictionaries.Add( value, this );
				}
				else
				{
					// If the dictionary is already loaded, get it from the cache
					MergedDictionaries.Add( SharedDictionaries[ value ] );
				}
			}
		}
	}
}


