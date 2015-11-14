using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DevelopersWin.VoteReporter.Entity
{
    public class Vote : VoteBase
	{
		[Required]
		public virtual VoteGroup Group { get; set; }

		[LocalStorage, NotMapped]
		public Uri Location
		{
			get { return LocationStorage.Transform( s => new Uri( s ) ); }
			set { LocationStorage = value.Transform( uri => uri.ToString() ); }
		}	string LocationStorage { get; set; }

		[Collection]
		public virtual ICollection<Record> Records { get; set; }

		public Record Latest => Records.OrderByDescending( record => record.Created ).First();
	}
}