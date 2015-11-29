using System;
using System.IO;

namespace DragonSpark.Application
{
	public interface IResourceManager
	{
		Stream GetStream( Uri relativeUri, string assemblyName);
	}
}