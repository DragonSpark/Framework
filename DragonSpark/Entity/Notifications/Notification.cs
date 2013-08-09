using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	[KnownType("KnownTypes")]
	public partial class Notification : INotification
	{
		static Type[] KnownTypes()
		{
			 return typeof(Notification).GetKnownTypes();
		}
		
		[NewGuidDefaultValue, Key]
		public Guid Id { get; set; }

		public string UserName { get; set; }

		[DefaultPropertyValue( true )]
		public bool SendEmail { get; set; }

		[DefaultPropertyValue( true )]
		public bool IsActive { get; set; }

		public bool IsHidden { get; set; }

		public bool IsImportant { get; set; }

		[StringLength( 1024 )]
		public string Title { get; set; }

		[StringLength( 4096 )]
		public string Message { get; set; }

		[CurrentTimeDefault]
		public DateTime Created { get; set; }

		[NotMapped]
		public Uri ImageSource
		{
			get
			{
				var uri = ImageSourceStorage.Transform( x => new Uri( x ) );
				return uri;
			}
			set { ImageSourceStorage = value.Transform( x => x.ToString() ); }
		}
		[Ignore]
		public string ImageSourceStorage { get; set; }
	}
}