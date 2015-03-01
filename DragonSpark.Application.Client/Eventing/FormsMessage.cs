
namespace DragonSpark.Application.Client.Eventing
{
	public class FormsMessage<TSender, TArgument>
	{
		readonly TSender sender;
		readonly TArgument argument;

		public FormsMessage( TSender sender, TArgument argument )
		{
			this.sender = sender;
			this.argument = argument;
		}

		public TSender Sender
		{
			get { return sender; }
		}

		public TArgument Argument
		{
			get { return argument; }
		}
	}
}