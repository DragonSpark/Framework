using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonSpark.Windows.Entity
{
	public class InstallationEntry
	{
		[Key, Column( Order = 0 )]
		public Guid Id { get; set; }

		[CurrentTime]
		public DateTimeOffset Installed { get; set; }

		[NotMapped]
		public Version Version
		{
			get { return VersionStorage.With( x => new Version( x ) ); }
			set { VersionStorage = value.With( x => x.ToString() ); }
		}

		[Key, Column( Order = 1 ), LocalStorage]
		public string VersionStorage { get; set; }
	}
}