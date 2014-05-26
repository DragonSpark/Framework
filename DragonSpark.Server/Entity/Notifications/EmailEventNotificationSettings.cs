namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public class EmailEventNotificationSettings
	{
		public string SmtpServerHost { get; set; }

		public int ServerPort { get; set; }

		public string FromAddress { get; set; }

		public string ToAddress { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }
	}
}