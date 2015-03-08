using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml;

namespace DragonSpark.Application.Communication
{
	class ApplyCyclicDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
	{
		readonly Int32 _maxItemsInObjectGraph;
		readonly bool _ignoreExtensionDataObject;
		readonly bool _preserveObjectReferences;

		public ApplyCyclicDataContractSerializerOperationBehavior( OperationDescription operationDescription,
		                                                           Int32 maxItemsInObjectGraph, bool ignoreExtensionDataObject,
		                                                           bool preserveObjectReferences )
			: base( operationDescription )
		{
			_maxItemsInObjectGraph = maxItemsInObjectGraph;
			_ignoreExtensionDataObject = ignoreExtensionDataObject;
			_preserveObjectReferences = preserveObjectReferences;
		}

		public override XmlObjectSerializer CreateSerializer( Type type, String name, String ns, IList<Type> knownTypes )
		{
			return  new DataContractSerializer( type, name, ns, knownTypes, _maxItemsInObjectGraph, _ignoreExtensionDataObject, _preserveObjectReferences, null /*dataContractSurrogate*/ );
		}

		public override XmlObjectSerializer CreateSerializer( Type type, XmlDictionaryString name, XmlDictionaryString ns,
		                                                      IList<Type> knownTypes )
		{
			return new DataContractSerializer( type, name, ns, knownTypes, _maxItemsInObjectGraph, _ignoreExtensionDataObject, _preserveObjectReferences, null /*dataContractSurrogate*/ );
		}
	}
}