using System.ComponentModel.DataAnnotations;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public class ApplicationRequestNotification : Notification
	{
		public string Action { get; set; }

		[IoCDefault]
		public PersonInformation From { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Wrapper property around a storage field." ), NotMapped]
		[Display( AutoGenerateField = false )]
		public string[] RequestIds
		{
			get { return RequestIdsStorage.ToStringArray(); }
			set { RequestIdsStorage = string.Join( ";", value ); }
		}
		[Display( AutoGenerateField = false )]
		public string RequestIdsStorage { get; set; }
	}
}