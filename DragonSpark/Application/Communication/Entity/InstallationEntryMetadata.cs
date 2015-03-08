using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Communication.Entity
{
    public class InstallationEntryMetadata
    {
        [Key, Column( Order = 0 )]
        public Guid Id { get; set; }

        [Key, Column( Order = 1 )]
        public string VersionStorage { get; set; }
    }
}