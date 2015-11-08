using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing
{
	[TestClass]
	public class ListSupportTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyListIsSynchronizedProperly()
		{
			var source = new List<ListItem>
			             	{
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A91}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A92}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A93}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A94}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A95}" ) }
			             	};
			var target = new List<ListItem>
			             	{
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A99}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A97}" ) },
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A98}" ) },
			             		source[3],
			             		new ListItem { ID = new Guid( "{C8229F4B-06A0-4f42-92F4-F4F2213B4A96}" ) },
			             		source[2]
							};

			ListSupport.Synchronize( source, target );
			Assert.AreEqual( source.Count, target.Count );
			source.All( item => source.IndexOf( item ) == target.IndexOf( item ) );
			target.All( item => source.IndexOf( item ) == target.IndexOf( item ) );
		}
	}
}