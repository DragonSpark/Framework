using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Web;

namespace DragonSpark.Server
{
	public class ApplicationModule : IHttpModule
	{
		static readonly object Locker = new object();

		public const string ApplicationModulesKey = "application.Modules";

		protected HttpApplication Application { get; set; }

		class DisposeContext : IDisposable
		{
			readonly IDictionary dictionary;

			public DisposeContext( IDictionary dictionary )
			{
				this.dictionary = dictionary;
			}

			public void Dispose()
			{
				dictionary.With( x => x.Keys.Cast<object>().Where( y => x[y] is IDisposable ).ToArray().Apply( y =>
				{
					x[y].TryDispose();
					x.Remove( y );
				} ) );
			}
		}

		void IHttpModule.Init( HttpApplication application )
		{
			lock ( Locker )
			{
				Initialize( application );
			}
		}

		protected virtual void Initialize( HttpApplication application )
		{
			Application = application;

			Application.BeginRequest += ( sender, args ) =>
			{
				Application.Context.Items[ApplicationModulesKey] = Application.Modules;
				Application.Context.DisposeOnPipelineCompleted( new DisposeContext( Application.Context.Items ) );
			};
		}

		public virtual void Dispose()
		{}
	}
}