using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Summary description for XmlSerializationPolicyTests
	/// </summary>
	[TestClass]
	public class XmlSerializationPolicyTests
	{
		[TestMethod, DeploymentItem( "DragonSpark.Testing/Unity/Extensions/Serialization/Resources/NamedObjectFile.xml" ), Ignore]
		public void VerifyNamedObjectSerializedProperlyFromFileXml()
		{
			var target = new Configuration.Serialization().Instance.GetInstance<INamedObject>( "FileBased" );
			Assert.AreEqual( "Named Object from File", target.Name );
		}

		[TestMethod, Ignore]
		public void VerifyNamedObjectSerializedProperlyFromAssemblyResourceXml()
		{
			var target = new Configuration.Serialization().Instance.GetInstance<INamedObject>( "AssemblyResourceBased" );
			Assert.AreEqual( "Named Object from Assembly-based resource", target.Name );
		}
 
		[TestMethod]
		public void VerifyNamedObjectSerializedProperlyFromTypeResourceXml()
		{
			var target = new Configuration.Serialization().Instance.GetInstance<INamedObject>( "TypeResourceBased" );
			Assert.AreEqual( "Named Object from Type-based Resource", target.Name );
		}
	}
}
