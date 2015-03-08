using System;
using System.Reflection;
using System.Linq;

namespace DragonSpark.Application.Communication.Channels
{
    public abstract class ProxyBase
    {
        readonly object @async;

        protected ProxyBase( object async )
        {
            this.@async = async;
        }

        protected ProxyBase()
        {}

        protected object Invoke( MethodBase method, params object[] parameters )
        {
            var type = async.GetType();
            var begin = type.GetMethod( string.Format( "Begin{0}", method.Name ) );
            var end = type.GetMethod( string.Format( "End{0}", method.Name ) );
            var r = (IAsyncResult)begin.Invoke( async, parameters.Concat( new object[]{ null, null } ).ToArray() );
            var result = end.Invoke( async, new object[]{ r } );
            return result;
        }
    }
}