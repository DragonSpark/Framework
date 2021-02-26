namespace DragonSpark.Application.Messaging
{
	public readonly struct Message
	{
		public Message(string to, string title, string body)
		{
			To    = to;
			Title = title;
			Body  = body;
		}

		public string To { get; }

		public string Title { get; }

		public string Body { get; }

		public void Deconstruct(out string to, out string title, out string body)
		{
			to    = To;
			title = Title;
			body  = Body;
		}
	}
}