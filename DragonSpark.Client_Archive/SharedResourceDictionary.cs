using System;
using System.Collections.Generic;
using System.Windows;

namespace DragonSpark.Markup
{
	public class SharedResourceDictionary : ResourceDictionary
	{
		static readonly Dictionary<Uri, ResourceDictionary> SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

		Uri sourceUri;

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


