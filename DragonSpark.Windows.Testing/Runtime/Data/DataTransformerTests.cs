using DragonSpark.Windows.Runtime.Data;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime.Data
{
	public class DataTransformerTests
	{
		[Fact]
		public void Verify()
		{
			var stylesheet = DocumentFactory.Default.Get( "<?xml version=\"1.0\"?>\r\n<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">\r\n  <xsl:template match=\"/hello-world\">\r\n    <HTML>\r\n      <HEAD>\r\n        <TITLE></TITLE>\r\n      </HEAD>\r\n      <BODY>\r\n        <H1>\r\n          <xsl:value-of select=\"greeting\"/>\r\n        </H1>\r\n        <xsl:apply-templates select=\"greeter\"/>\r\n      </BODY>\r\n    </HTML>\r\n  </xsl:template>\r\n  <xsl:template match=\"greeter\">\r\n    <DIV>from <I><xsl:value-of select=\".\"/></I></DIV>\r\n  </xsl:template>\r\n</xsl:stylesheet>" );
			var data = DocumentFactory.Default.Get( "<?xml version=\"1.0\"?>\r\n<?xml-stylesheet type=\"text/xsl\" href=\"hello.xsl\"?>\r\n<hello-world>   <greeter>An XSLT Programmer</greeter>   <greeting>Hello, World!</greeting></hello-world>" );
			
			var parameter = new DataTransformParameter( stylesheet, data );
			var actual = DataTransformer.Default.Get( parameter );
			Assert.Equal( "<HTML>\r\n  <HEAD>\r\n    <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <TITLE></TITLE>\r\n  </HEAD>\r\n  <BODY>\r\n    <H1>Hello, World!</H1>\r\n    <DIV>from <I>An XSLT Programmer</I></DIV>\r\n  </BODY>\r\n</HTML>", actual );
		}
	}
}