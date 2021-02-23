namespace DragonSpark.Application.Connections
{
	public sealed class UserConnection
	{
		public UserConnection(string user, string connection)
		{
			User       = user;
			Connection = connection;
		}

		public string User { get; }

		public string Connection { get; }

		public void Deconstruct(out string user, out string connection)
		{
			user       = User;
			connection = Connection;
		}
	}
}