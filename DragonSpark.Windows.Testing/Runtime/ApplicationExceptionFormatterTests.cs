using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Security.Principal;
using System.Threading;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ApplicationExceptionFormatterTests
	{
		[Theory, DragonSpark.Testing.Framework.Setup.ConfiguredMoqAutoData]
		public void FormatException( [Frozen]AssemblyInformation information, Exception error, ApplicationExceptionFormatter sut )
		{
			var message = sut.Format( error );
			var fullName = error.GetType().FullName;
			var sections = new[] { $@"Exception occured in application {information.Title} ({information.Product}).
[Version: {information.Version}]

Exception of type '{fullName}' was thrown.
{fullName}
Details:
==============================================
An exception of type '{fullName}' occurred and was caught.
----------------------------------------------------------------", $@"Type : {error.GetType().AssemblyQualifiedName}
Message : {error.Message}
Source : {error.Source}
Help link : {error.HelpLink}
Data : System.Collections.ListDictionaryInternal
TargetSite : 
HResult : {error.HResult}
Stack Trace : The stack trace is unavailable.
Additional Info:

MachineName : {Environment.MachineName}", $@"FullName : {typeof(ExceptionPolicy).Assembly.FullName}
AppDomainName : {AppDomain.CurrentDomain.FriendlyName}
ThreadIdentity : {Thread.CurrentPrincipal.Identity.Name}
WindowsIdentity : {WindowsIdentity.GetCurrent().Name}" };
			sections.Each( s => Assert.Contains( s, message ) );
			
		}
	}
}