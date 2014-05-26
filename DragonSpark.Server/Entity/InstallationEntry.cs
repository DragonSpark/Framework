using DragonSpark.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Entity
{
    [MetadataType( typeof(InstallationEntryMetadata) )]
    public class InstallationEntry
    {
        public Guid Id { get; set; }

        public string VersionStorage { get; set; }

        public DateTime InstallationDate { get; set; }

        public Version Version
        {
            get { return version ?? ( version = VersionStorage.Transform( x => new Version( x ) ) ); }
        }	Version version;
    }
}