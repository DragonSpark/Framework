using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Summary description for XmlSerializationPolicyTests
	/// </summary>
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
