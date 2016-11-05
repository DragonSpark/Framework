using DragonSpark.Testing.Objects;
using DragonSpark.Windows.Runtime.Data;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime.Data
{
	public class DataSerializerTests
	{
		[Theory, AutoData]
		public void Verify( string message )
		{
			var instance = new ClassWithProperty { Property = message };
			var stylesheet = DocumentFactory.Default.Get( "<?xml version=\"1.0\"?>\r\n<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">\r\n    <xsl:template match=\"node()|@*\">\r\n      <xsl:copy>\r\n        <xsl:apply-templates select=\"node()|@*\"/>\r\n      </xsl:copy>\r\n    </xsl:template>\r\n</xsl:stylesheet>" );
			var data = DocumentFactory.Default.Get( Serializer.Default.Save( instance ) );

			var general = DataSerializer.Default.Get( new DataTransformParameter( stylesheet, data ) );
			var casted = Assert.IsType<ClassWithProperty>( general );

			var typed = DataSerializer<ClassWithProperty>.Default.Get( new DataTransformParameter( stylesheet, data ) );
			Assert.Equal( message, typed.Property );
			Assert.Equal( casted.Property, typed.Property );
		}
	}
}