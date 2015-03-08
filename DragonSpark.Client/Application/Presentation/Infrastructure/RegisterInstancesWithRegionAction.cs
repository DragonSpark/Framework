using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    [ContentProperty( "Items" )]
    public class RegisterInstancesWithRegionAction : RegisterWithRegionActionBase
    {
        public Collection<object> Items
        {
            get { return items; }
        }	readonly Collection<object> items = new Collection<object>();

        protected internal override void Process( IUnityContainer container, IRegion region, Type viewType )
        {
            Items.Apply( x => Register( region, viewType, x ) );
        }
    }
}