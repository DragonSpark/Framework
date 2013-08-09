using DragonSpark.Objects;
using Microsoft.LightSwitch.Security.ServerGenerated.Implementation;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Configuration;

namespace DragonSpark.Application.Communication.Security
{
	public class SecurityDataObjectContextFactory : Factory<string, SecurityDataObjectContext>
	{
		protected override SecurityDataObjectContext CreateItem( string source )
		{
			var entityContainer = source ?? "_IntrinsicData";
			/*var useWebConfiguration = true;
			if ( string.IsNullOrEmpty( entityContainer ) )
			{
				entityContainer = "SecurityData";
				useWebConfiguration = false;
			}*/
			var name = typeof(SecurityDataServiceImplementation).Assembly.GetName();
			const string str = "Microsoft.LightSwitch.RuntimeAPI.ServerGeneratedAPIDefinitions.PublicFramework.Resources.GeneratedArtifacts";
			var metadata = string.Format( CultureInfo.InvariantCulture, "res://{0}/{1}.SecurityData.csdl|res://{0}/{1}.SecurityData.ssdl|res://{0}/{1}.SecurityData.msl", new object[] { name.FullName, str } );
			var connectionString = GetEntityConnectionString( entityContainer, metadata, "System.Data.SqlClient", true );
			var result = new SecurityDataObjectContext( connectionString );
			return result;
		}

		static string GetEntityConnectionString( string connectionStringName, string metadata, string provider, bool useWebConfiguration )
		{
			var connectionString = useWebConfiguration ? GetConnectionStringSettings( connectionStringName ).ConnectionString : string.Empty;
			var builder = new EntityConnectionStringBuilder { Metadata = metadata, Provider = provider, ProviderConnectionString = connectionString };
			return builder.ConnectionString;
		}

		static ConnectionStringSettings GetConnectionStringSettings( string connectionStringName )
		{
			if ( HostEnvironment.RunningInAzure() )
			{
				try
				{
					return new ConnectionStringSettings( connectionStringName, RoleEnvironment.GetConfigurationSettingValue( "Microsoft.LightSwitch.ConnectionString." + connectionStringName ) );
				}
				catch ( RoleEnvironmentException )
				{}
			}
			var settings = WebConfigurationManager.ConnectionStrings[ connectionStringName ];
			if ( settings == null )
			{
				throw new InvalidOperationException( string.Format(CultureInfo.CurrentCulture, "ServerGeneratedResources.DataServiceImplementation_CouldNotFindConnectionString: {0}", connectionStringName ) );
			}
			return settings;
		}

		internal static class HostEnvironment
		{
			// Fields
			static readonly object AzureLock = new object();
			static bool RunningInAzureField;
			static bool RunningInAzureChecked;
		
			// Methods
			public static bool RunningInAzure()
			{
				if ( !RunningInAzureChecked )
				{
					lock ( AzureLock )
					{
						if ( !RunningInAzureChecked )
						{
							try
							{
								RunningInAzureField = RoleEnvironment.IsAvailable;
							}
							catch ( TypeInitializationException )
							{}
							RunningInAzureChecked = true;
						}
					}
				}
				return RunningInAzureField;
			}
		}
	}

	public static class ApplicationUserExtensions
	{
		public static void ApplyRoles( this ApplicationUser target, IEnumerable<string> roles = null )
		{
			var source = roles ?? target.Roles;
			var items = source.ToArray();
			target.Roles = items;
			target.Permissions = GetPermissions( items ).ToArray();
		}

		static IEnumerable<string> GetPermissions( IEnumerable<string> roles )
		{
			var result = GetAssignedRolePermissions( roles ).Select( rp => rp.PermissionId ).Distinct();
			return result;
		}

		static IEnumerable<RolePermission> GetAssignedRolePermissions(IEnumerable<string> roles )
		{
			using ( new TransactionScope( TransactionScopeOption.Suppress ) )
			{
				using ( var context = new SecurityDataObjectContextFactory().Create() )
				{
					var result = context.RolePermissions.Where( rp => roles.Contains( rp.RoleName ) ).ToArray();
					return result;
				}
			}
		}
	}
}