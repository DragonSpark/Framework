using System;
using System.ServiceModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication
{
	public static class ClientChannelFactory<TChannel>
	{
		static Func<string, TChannel> RegisteredFactory;

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Will fix in later version.  Used for T4-proxies in Silverlight." )]
        public static Func<string, TChannel> DefaultFactory
		{
			get { return DefaultFactoryField; }
		}	static readonly Func<string, TChannel> DefaultFactoryField = x => new ChannelFactory<TChannel>( x ).CreateChannel();

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Will fix in later version.  Used for T4-proxies in Silverlight." )]
        public static void Register( Func<string, TChannel> factory )
		{
			RegisteredFactory = factory;
		}
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Will fix in later version.  Used for T4-proxies in Silverlight." )]
		public static TChannel Create( string name = null )
		{
			var factory = RegisteredFactory ?? DefaultFactory;
			var result = factory( name.NullIfEmpty() ?? "*" );
			return result;
		}
	}
}