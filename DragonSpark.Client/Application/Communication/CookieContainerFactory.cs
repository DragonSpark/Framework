using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Browser;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication
{
    [Singleton]
    public class CookieContainerFactory : Factory<Uri, CookieContainer>
    {
        public static CookieContainerFactory Instance
        {
            get { return InstanceField; }
        }	static readonly CookieContainerFactory InstanceField = new CookieContainerFactory();

        protected override CookieContainer CreateItem( Uri source )
        {
            var result = new CookieContainer();
            var reset = new ManualResetEvent( false );
            Threading.Application.Execute( () =>
            {
                var strings = HtmlPage.Document.Cookies.Split( new[] { ";" }, StringSplitOptions.RemoveEmptyEntries ).ToArray();
                var array = strings.Select( x => x.Split( new[] { "=" }, StringSplitOptions.RemoveEmptyEntries ) ).ToArray();
                var cookies = array.Select( x =>
                {
                    var value = string.Join( "=", x.Skip( 1 ) );
                    var cookie = new Cookie( x[0].Trim(), value );
                    return cookie;
                } ).ToArray();
                var uri = new UriBuilder( source ) { Path = string.Empty }.Uri;
                cookies.Apply( x => result.Add( uri, (Cookie)x ) );
                reset.Set();
            } );
            reset.WaitOne();
            return result;
        }
    }
}