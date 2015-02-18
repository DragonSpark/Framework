using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonSpark.Server.Legacy.Entity
{
    public class InstallationEntryMetadata
    {
        [Key, Column( Order = 0 )]
        public Guid Id { get; set; }

        [Key, Column( Order = 1 )]
        public string VersionStorage { get; set; }
    }
}