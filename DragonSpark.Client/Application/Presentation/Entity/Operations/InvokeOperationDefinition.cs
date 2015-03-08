using System;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    [ContentProperty( "Parameters" )]
    public class InvokeOperationDefinition : ViewAwareObject
    {
        public Type EntityType
        {
            get { return entityType; }
            set { SetProperty( ref entityType, value, () => EntityType ); }
        }	Type entityType;

        public string MethodName
        {
            get { return methodName; }
            set { SetProperty( ref methodName, value, () => MethodName ); }
        }	string methodName;

        public ViewAwareCollection<MethodParameter> Parameters
        {
            get { return parameters; }
        }	readonly ViewAwareCollection<MethodParameter> parameters = new ViewAwareCollection<MethodParameter>();

        public Type ReturnType
        {
            get { return returnType; }
            set { SetProperty( ref returnType, value, () => ReturnType ); }
        }	Type returnType = typeof(Type);

        public bool HasSideEffects
        {
            get { return hasSideEffects; }
            set { SetProperty( ref hasSideEffects, value, () => HasSideEffects ); }
        }	bool hasSideEffects = true;

        public InvokeOperation Invoke( DomainContext context, Action<InvokeOperation> callback = null, object userState = null )
        {
            var result = context.InvokeOperation( MethodName, ReturnType, Parameters.ToDictionary( x => x.ParameterName, x => x.Value ), HasSideEffects, callback, userState );
            return result;
        }
    }
}