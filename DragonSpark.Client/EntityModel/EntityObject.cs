using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Common.EntityModel
{
	[DataContract]
	public class EntityObject : System.Windows.Ria.Entity
	{
		DisplayNameProvider Provider
		{
			get { return provider ?? ( provider = new DisplayNameProvider( this ) ); }
		}	DisplayNameProvider provider;

		public override string ToString()
		{
			return Provider.DisplayName;
		}
	}
}