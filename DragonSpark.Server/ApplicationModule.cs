using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Web;

namespace DragonSpark.Server
{
	public class ApplicationModule : IHttpModule
	{
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

		public virtual void Init( HttpApplication application )
		{
			Application = application;

			Application.BeginRequest += ( sender, args ) =>
			{
				Application.Context.Items["Testes"] = Application.Context;
				Application.Context.Items[ApplicationModulesKey] = Application.Modules;
				Application.Context.DisposeOnPipelineCompleted( new DisposeContext( Application.Context.Items ) );
			};
		}

		public virtual void Dispose()
		{}
	}
}