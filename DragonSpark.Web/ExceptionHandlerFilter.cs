using DragonSpark.Extensions;
using DragonSpark.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace DragonSpark.Web
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public sealed class ExceptionHandlerFilter : ExceptionFilterAttribute
	{
		static readonly IDictionary<Type, HttpStatusCode> Defaults = new Dictionary<Type, HttpStatusCode>
			{
				{typeof(NotImplementedException), HttpStatusCode.NotImplemented},
				{typeof(ArgumentNullException), HttpStatusCode.BadRequest},
				{typeof(InvalidOperationException), HttpStatusCode.InternalServerError},
				{typeof(ArgumentException), HttpStatusCode.BadRequest},
				{typeof(NotSupportedException), HttpStatusCode.BadRequest}
			};

		public ExceptionHandlerFilter( IDictionary<Type, HttpStatusCode> mappings = null, string policyName = EnterpriseLibraryExceptionHandler.DefaultExceptionPolicy )
		{
			Mappings = mappings ?? Defaults;
			PolicyName = policyName;
		}

		public IDictionary<Type, HttpStatusCode> Mappings { get; set; }

		public string PolicyName { get; set; }

		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope" )]
		public override void OnException( HttpActionExecutedContext context )
		{
			context.Exception.NotNull( x =>
			{
				Exception error;
				ExceptionPolicy.HandleException( x, PolicyName, out error );

				var code = DetermineCode( error );
				context.Response = new HttpResponseMessage( code ) { Content = new StringContent( error.Message ) };
			} );
		}

		HttpStatusCode DetermineCode( Exception exception )
		{
			var result = exception.AsTo<HttpException, HttpStatusCode?>( x => (HttpStatusCode)x.GetHttpCode() ) ?? DetermineFromMapping( exception );
			return result;
		}

		HttpStatusCode DetermineFromMapping( Exception exception )
		{
			var result = Mappings.Keys.FirstOrDefault( x => x.IsInstanceOfType( exception ) ).Transform( x => x.Transform( y => Mappings[y], () => HttpStatusCode.InternalServerError ) );
			return result;
		}
	}
}