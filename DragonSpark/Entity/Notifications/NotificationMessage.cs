using System.Collections.Generic;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
    public class NotificationMessage<TNotification> : NotificationMessage where TNotification : ApplicationRequestNotification, new()
	{
		public NotificationMessage( ApplicationUser to, string body, ApplicationUser replacementTarget = null ) : this( null, to, body, replacementTarget )
		{}

		public NotificationMessage( IEnumerable<ApplicationUser> to, string body, ApplicationUser replacementTarget = null ) : this( null, to, body, replacementTarget )
		{}

		public NotificationMessage( ApplicationUser from, ApplicationUser to, string body, ApplicationUser replacementTarget = null ) : this( from, to.ToEnumerable(), body, replacementTarget )
		{}

		public NotificationMessage( ApplicationUser from, IEnumerable<ApplicationUser> to, string body, ApplicationUser replacementTarget = null ) : base( from, to, body, new TNotification { Message = body }.WithDefaults(), replacementTarget )
		{}
	}


    public class NotificationMessage
    {
        readonly IEnumerable<ApplicationUser> to;
        readonly string body;
        readonly ApplicationRequestNotification requestNotification;
        readonly ApplicationUser replacementTarget;
        readonly ApplicationUser @from;

        public NotificationMessage( IEnumerable<ApplicationUser> to, string body, ApplicationRequestNotification requestNotification, ApplicationUser replacementTarget = null ) : this( null, to, body, requestNotification, replacementTarget )
        {}

        public NotificationMessage( ApplicationUser to, string body, ApplicationRequestNotification requestNotification, ApplicationUser replacementTarget = null ) : this( null, to, body, requestNotification, replacementTarget )
        {}

        public NotificationMessage( ApplicationUser from, ApplicationUser to, string body, ApplicationRequestNotification requestNotification, ApplicationUser replacementTarget = null ) : this( from, (IEnumerable<ApplicationUser>)to.ToEnumerable(  ), body, requestNotification, replacementTarget )
        {}

        public NotificationMessage( ApplicationUser from, IEnumerable<ApplicationUser> to, string body, ApplicationRequestNotification requestNotification, ApplicationUser replacementTarget = null )
        {
            this.to = to;
            this.body = body;
            this.requestNotification = requestNotification;
            this.replacementTarget = replacementTarget;
            this.@from = from;
        }

        public IEnumerable<ApplicationUser> To
        {
            get { return to; }
        }

        public string Body
        {
            get { return body; }
        }

        public ApplicationRequestNotification RequestNotification
        {
            get { return requestNotification; }
        }

        public ApplicationUser ReplacementTarget
        {
            get { return replacementTarget; }
        }

        public ApplicationUser From
        {
            get { return from; }
        }
    }
}