﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Logging;

namespace DragonSpark.Extensions
{
	public static class AssemblyExtensions
	{
		public static IEnumerable<Type> GetValidTypes( this Assembly target )
		{
			try
			{
				var result = target.GetTypes();
				return result;
			}
			catch ( ReflectionTypeLoadException e )
			{
				var messages = string.Join( System.Environment.NewLine, e.LoaderExceptions.Select( x => string.Concat( "- ", x.Message ) ) );
				Log.Warning( string.Format( "Could not get types for assembly: {0}.  Messages: {1}{2}", target.GetName(), System.Environment.NewLine, messages ) );
				return e.Types.NotNull().ToArray();
			}
		}

		public static string GetRootNamespace( this Assembly target )
		{
			var root = target.FullName.Transform( x => x.Split( ',' ).FirstOrDefault().Transform( y => y.Replace( ".Client", string.Empty ) ) );
			var result = target.GetExportedTypes().Where( x => x.Namespace.StartsWith( root ) ).Select( x => x.Namespace ).OrderBy( x => x.Length ).FirstOrDefault();
			return result;
		}

		public static string GetAssemblyName( this Assembly assembly )
		{
			var result = assembly.FullName.Transform( x => x.Substring( 0, x.IndexOf( ',' ) ) );
			return result;
		}
	}
}
