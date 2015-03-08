using System;
using System.Net.Mail;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public class EventNotification
	{
		public MailAddress ToAddress { get; set; }

		public string Subject { get; set; }

		public string Message { get; set; }

		public Uri ViewLocation { get; set; }
	}
}