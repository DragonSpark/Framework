using System;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;

namespace DragonSpark.Application.Presentation.Entity
{
    public interface IViewSupport
    {
        DataFormCommandButtonsVisibility DetermineVisibility( DomainContext context, Type entityType );
    }
}