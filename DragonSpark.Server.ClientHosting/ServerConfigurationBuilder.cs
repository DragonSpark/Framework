using DragonSpark.Objects;

namespace DragonSpark.Server.ClientHosting
{
	public class ServerConfigurationBuilder : Factory<ServerConfiguration>
	{
		readonly HubNameBuilder hubNameBuilder;
		readonly EntityServicesBuilder servicesBuilder;

		public ServerConfigurationBuilder( HubNameBuilder hubNameBuilder, EntityServicesBuilder servicesBuilder )
		{
			this.hubNameBuilder = hubNameBuilder;
			this.servicesBuilder = servicesBuilder;
		}

		protected override ServerConfiguration CreateItem( object parameter )
		{
			var result = new ServerConfiguration
			{
				Hubs = hubNameBuilder.Create(),
				EntityServices = servicesBuilder.Create(),
			};
			return result;
		}
	}
}