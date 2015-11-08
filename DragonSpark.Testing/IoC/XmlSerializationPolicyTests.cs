using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	[TestClass]
	public class XmlSerializationPolicyTests
	{
		[TestMethod]
		public void VerifyNamedObjectSerializedProperlyFromTypeResourceXml()
		{
			var target = new Configuration.Serialization().Instance.GetInstance<INamedObject>( "TypeResourceBased" );
			Assert.AreEqual( "Named Object from Type-based Resource", target.Name );
		}
	}
}
