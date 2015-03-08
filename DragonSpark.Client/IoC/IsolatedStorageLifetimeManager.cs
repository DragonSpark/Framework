using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Serialization;
using SLaB.Utilities.Xaml.Serializer;

namespace DragonSpark.IoC
{
	public class IsolatedStorageLifetimeManager : KeyBasedLifetimeManager
	{
		public IsolatedStorageLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = Reference.Transform( x => x.Target ) ?? FromStore();
			return result;
		}

		object FromStore()
		{
			using ( var store = IsolatedStorageFile.GetUserStoreForApplication() )
			{
				var result = store.FileExists( Key ) ? Resolve( store ) : null;
				Reference = result.Transform( x => new WeakReference( x ) );
				return result;
			}
		}

		object Resolve( IsolatedStorageFile store )
		{
			try
			{
				using ( var stream = store.OpenFile( Key, FileMode.Open, FileAccess.Read ) )
				{
					var result = XamlSerializationHelper.Load( stream ).Transform( x => x.WithDefaults() );
					result.Null( () => Delete( store ) );
					return result;
				}
			}
			catch ( IsolatedStorageException )
			{}
			catch ( XamlParseException )
			{}
			return null;
		}

		public IsolatedStorageLifetimeDisposeAction DisposeAction { get; set; }

		protected override void Dispose( bool disposing )
		{
			switch ( DisposeAction )
			{
				case IsolatedStorageLifetimeDisposeAction.Remove:
					base.Dispose( disposing );
					break;
				case IsolatedStorageLifetimeDisposeAction.Save:
					Reference.Transform( x => x.Target ).NotNull( SetValue );
					break;
			}
		}

		public override void RemoveValue() 
		{ 
			using ( var store = IsolatedStorageFile.GetUserStoreForApplication() )
			{
				Delete( store );
			}
		}

		void Delete( IsolatedStorageFile store )
		{
			if ( store.FileExists( Key ) )
			{
				store.DeleteFile( Key );
			}
		}

		WeakReference Reference { get; set; }

		public override void SetValue( object newValue )
		{
			using ( var store = IsolatedStorageFile.GetUserStoreForApplication() )
			{
				using ( var stream = store.OpenFile( Key, FileMode.Create, FileAccess.Write ) )
				{
					var data = new XamlSerializer().Serialize( newValue );

					using ( var streamWriter = new StreamWriter( stream ) )
					{
						streamWriter.Write( data );
					}
				}
			}
			Reference = new WeakReference( newValue );
		}
	}
}
