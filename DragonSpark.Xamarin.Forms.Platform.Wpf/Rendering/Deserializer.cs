using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using FileMode = Xamarin.Forms.FileMode;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	internal class Deserializer : IDeserializer
	{
		private const string propertyStoreFile = "PropertyStore.forms";
		public Task<IDictionary<string, object>> DeserializePropertiesAsync()
		{
			return Task.Run<IDictionary<string, object>>(delegate
			{
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = userStoreForApplication.OpenFile("PropertyStore.forms", System.IO.FileMode.OpenOrCreate))
					{
						using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(isolatedStorageFileStream, XmlDictionaryReaderQuotas.Max))
						{
							if (isolatedStorageFileStream.Length == 0L)
							{
								IDictionary<string, object> result = null;
								return result;
							}
							try
							{
								DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(Dictionary<string, object>));
								IDictionary<string, object> result = (IDictionary<string, object>)dataContractSerializer.ReadObject(xmlDictionaryReader);
								return result;
							}
							catch (Exception)
							{
							}
						}
					}
				}
				return null;
			});
		}
		public Task SerializePropertiesAsync(IDictionary<string, object> properties)
		{
			properties = new Dictionary<string, object>(properties);
			return Task.Run(delegate
			{
				bool flag = false;
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = userStoreForApplication.OpenFile("PropertyStore.forms.tmp", System.IO.FileMode.OpenOrCreate))
					{
						using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(isolatedStorageFileStream))
						{
							try
							{
								DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(Dictionary<string, object>));
								dataContractSerializer.WriteObject(xmlDictionaryWriter, properties);
								xmlDictionaryWriter.Flush();
								flag = true;
							}
							catch (Exception)
							{
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				using (IsolatedStorageFile userStoreForApplication2 = IsolatedStorageFile.GetUserStoreForApplication())
				{
					try
					{
						if (userStoreForApplication2.FileExists("PropertyStore.forms"))
						{
							userStoreForApplication2.DeleteFile("PropertyStore.forms");
						}
						userStoreForApplication2.MoveFile("PropertyStore.forms.tmp", "PropertyStore.forms");
					}
					catch (Exception)
					{
					}
				}
			});
		}
	}
}
