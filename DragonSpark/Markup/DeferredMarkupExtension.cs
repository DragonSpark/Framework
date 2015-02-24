//-----------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Windows.Markup;

// Based on/Credit: http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
namespace DragonSpark.Application.Markup
{
	public abstract class DeferredMarkupExtension : MarkupExtension
	{
		protected DeferredMarkupExtension()
		{
			builders = new Lazy<IMarkupTargetValueSetterBuilder[]>( ResolveBuilders );
		}

		public override sealed object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IProvideValueTarget>()
				.Transform( target => Builders.Select( builder => builder.Create<IMarkupTargetValueSetter>( serviceProvider ) ).NotNull().FirstOrDefault()
					.Transform( setter => BeginProvideValue( serviceProvider, setter ) ) );
			return result;
		}

		protected abstract object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter );

		protected IMarkupTargetValueSetterBuilder[] Builders
		{
			get { return builders.Value; }
		}	readonly Lazy<IMarkupTargetValueSetterBuilder[]> builders;

		protected virtual IMarkupTargetValueSetterBuilder[] ResolveBuilders()
		{
			var result = new IMarkupTargetValueSetterBuilder[]
			{
				PropertyInfoMarkupTargetValueSetterBuilder.Instance,
				DependencyPropertyMarkupTargetValueSetterBuilder.Instance,
				FieldInfoMarkupTargetValueSetterBuilder.Instance
			};
			return result;
		}
	}
}
