using DragonSpark.Extensions;
using System;
using System.Runtime.InteropServices.Automation;

namespace DragonSpark.Application.Communication.SystemServices
{
    public class ServiceInstallation : IDisposable
    {
        readonly string _servicePath;
        readonly dynamic _shell = AutomationFactory.CreateObject( "WScript.Shell" );

        public ServiceInstallation( string servicePath )
        {
            _servicePath = servicePath;
        }

        public void Start()
        {
            Invoke();
        }

        void Invoke( ServiceLauncherAction? action = null )
        {
            var command = string.Concat( _servicePath, action.Transform( x => string.Format( " /{0}", x.ToString().ToLower() ) ) );
            _shell.Run( command, 0, true );
        }

        public void Dispose()
        {
            Invoke( ServiceLauncherAction.Stop );
        }
    }
}
