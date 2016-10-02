using DragonSpark.Runtime;
using DragonSpark.Testing.Objects;
using System.IO;
using System.Text;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class SerializerTests
	{
		const string Property = "Property2a258824-6489-4cd5-92e9-d6dd98d76002";
		readonly static string Expected = $@"<ClassWithProperty xmlns=""http://schemas.datacontract.org/2004/07/DragonSpark.Testing.Objects"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Property>{Property}</Property></ClassWithProperty>";

		[Theory, AutoData, AdditionalTypes( typeof(Serializer) )]
		public void Save( ISerializer sut, ClassWithProperty item )
		{
			item.Property = Property;
			Assert.IsType<Serializer>( sut );
			Assert.Equal( Expected, sut.Save( item ) );
		}

		[Theory, AutoData, AdditionalTypes( typeof(Serializer) )]
		public void Load( ISerializer sut )
		{
			Assert.IsType<Serializer>( sut );

			var stream = new MemoryStream( Encoding.UTF8.GetBytes( Expected ) );

			var loaded = Assert.IsType<ClassWithProperty>( sut.Load<ClassWithProperty>( stream ) );
			Assert.Equal( "Property2a258824-6489-4cd5-92e9-d6dd98d76002", loaded.Property );
		}
	}
}