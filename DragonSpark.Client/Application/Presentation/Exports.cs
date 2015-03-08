using System;
using DragonSpark.Extensions;
using Microsoft.VisualStudio.ExtensibilityHosting;

namespace DragonSpark.Application.Presentation
{
	public static class Exports
	{
		readonly static VsExportProvisionScope LocalScope = new VsExportProvisionOuterScope( new Guid( "92E1D6FA-CAD7-400f-914D-E265294841B4" ), new VsExportProvisionScope( new Guid( "229C0B13-97A2-41BE-B96D-3CDDB9E8E389" ) ) );
		
		public static TItem Locate<TItem>() where TItem : class
		{
			TItem result;
			VsExportProviderService.TryGetExportedValue( LocalScope, out result );
			return result;
		}

		public static TResult With<TService,TResult>( Func<TService,TResult> action ) where TService : class
		{
			var result = Locate<TService>().Transform( action );
			return result;
		}
	}
}