﻿using System;
using System.IO;

namespace DragonSpark.Server.Legacy.Io
{
	public interface IWorkspace : IDisposable
	{
		void Copy( string sourceFilePath );

		string Write( string fileName, string contents );

        string Save( string fileName, Stream stream );

		DirectoryInfo Directory { get; }
	}
}