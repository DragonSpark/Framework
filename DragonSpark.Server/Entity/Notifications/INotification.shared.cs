using System;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface INotification
	{
		Guid Id { get; set; }

		string UserName { get; set; }

		bool IsActive { get; set; }

		bool IsHidden { get; set; }
		bool IsImportant { get; set; }

		string Title { get; set; }

		string Message { get; set; }

		DateTime Created { get; set; }

		Uri ImageSource { get; set; }
	}
}