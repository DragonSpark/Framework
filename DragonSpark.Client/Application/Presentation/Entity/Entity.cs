using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DragonSpark.Application.Presentation.Interaction;

namespace DragonSpark.Application.Presentation.Entity
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Used to work around a limitation of WCF Ria Services." ), DataContract]
	public class Entity : System.ServiceModel.DomainServices.Client.Entity, IModifiable
	{
		// HACK: SuperHack until DataForm supports properties of type IChangeTracking:
		public void MarkAsModified()
		{
			Version___++;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Used to ensure conflict does not occur with another field name. " ), Display(AutoGenerateField = false)]
		public int Version___
		{
			get { return version___; }
			set
			{
				if ( version___ != value )
				{
					version___ = value;
					RaisePropertyChanged( "Version___" );
				}
			}
		}	int version___;
	}
}