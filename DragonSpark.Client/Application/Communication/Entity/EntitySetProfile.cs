using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;

namespace DragonSpark.Application.Communication.Entity
{
	[ContentProperty( "OperationDescriptors" )]
	public class EntitySetProfile : IEntitySetProfile
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type EntityType { get; set; }

		public string Title { get; set; }
		public string ItemName { get; set; }
		public string ItemNamePlural { get; set; }
		public string DisplayNamePath { get; set; }
		
		public bool IsRoot { get; set; }

		public int Order { get; set; }

		[TypeConverter( typeof(StringArrayConverter) )]
		public IEnumerable<string> AuthorizedRoles { get; set; }

		IEnumerable<IEntitySetOperationProfile> IEntitySetProfile.Operations
		{
			get { return OperationDescriptors; }
		}

		public ObservableCollection<EntitySetOperationDescriptor> OperationDescriptors
		{
			get { return operationDescriptors ?? ( operationDescriptors = new ObservableCollection<EntitySetOperationDescriptor>() ); }
		}	ObservableCollection<EntitySetOperationDescriptor> operationDescriptors;
	}
}