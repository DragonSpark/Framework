using DragonSpark.Application.Client.Forms.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xaml;
using Xamarin.Forms;
using FileMode = System.IO.FileMode;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	class Deserializer : IDeserializer
	{
		const string PropertyStoreFile = "PropertyStore.forms";

		public Task<IDictionary<string, object>> DeserializePropertiesAsync()
		{
			return Task.Run( () =>
			{
				using ( var store = PlatformServices.DetermineStore() )
				{
					var result = store.FileExists( PropertyStoreFile ) ? Load( store ) : null;
					return result;
				}
			} );
		}

		static IDictionary<string, object> Load( IsolatedStorageFile store )
		{
			using ( var stream = store.OpenFile( PropertyStoreFile, FileMode.OpenOrCreate ) )
			{
				var result = stream.Length > 0 ? (IDictionary<string, object>)XamlServices.Load( stream ) : null;
				return result;
			}
		}

		public Task SerializePropertiesAsync( IDictionary<string, object> properties )
		{
			return Task.Run( () =>
			{
				var target = new Dictionary<string, object>( properties );
				var temp = string.Format( "{0}.tmp", PropertyStoreFile );
				if ( Save( target, temp ) )
				{
					using ( var store = PlatformServices.DetermineStore() )
					{
						try
						{
							if ( store.FileExists( PropertyStoreFile ) )
							{
								store.DeleteFile( PropertyStoreFile );
							}
							store.MoveFile( temp, PropertyStoreFile );
						}
						catch ( Exception e )
						{
							Trace.WriteLine( e );
						}
					}
				}
			} );
		}

		static bool Save( IDictionary<string, object> properties, string temp )
		{
			using ( var store = PlatformServices.DetermineStore() )
			{
				using ( var stream = store.OpenFile( temp, FileMode.OpenOrCreate ) )
				{
					try
					{
						XamlServices.Save( new StreamWriter( stream ), properties );
						return true;
					}
					catch ( Exception e )
					{
						Trace.WriteLine( e );
						return false;
					}
				}
			}
		}
	}
}
