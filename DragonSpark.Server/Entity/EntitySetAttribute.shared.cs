using System;

namespace DragonSpark.Application.Communication.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Not possible." ), AttributeUsage(AttributeTargets.Class)]
	public class EntitySetAttribute : Attribute
	{
		public Type EntityType { get; set; }

		public string Title { get; set; }

		public string ItemName { get; set; }

		public string ItemNamePlural { get; set; }

		public string DisplayNamePath { get; set; }

		/*public string QueryName { get; set; }*/

		/*public bool EnableFiltering { get; set; }*/

		public bool IsRoot { get; set; }

		public string AuthorizedRoles { get; set; }

		public int Order { get; set; }
	}
}