using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Markup;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    [ContentProperty( "Definitions" )]
    public class SubmitEntitySetSourceOperation : SubmitSourceOperation<EntitySetCollectionViewSource>
    {
        protected override OperationBase ResolveOperation()
        {
            var result = Definitions.ContainsKey( ContextChecked.EntityType ) ? Definitions[ ContextChecked.EntityType ].Invoke( ContextChecked.DomainContext ) : base.ResolveOperation();
            return result;
        }

        public Dictionary<Type,InvokeOperationDefinition> Definitions
        {
            get { return definitions; }
            set { SetProperty( ref definitions, value, () => Definitions ); }
        }	Dictionary<Type,InvokeOperationDefinition> definitions = new Dictionary<Type, InvokeOperationDefinition>();

        /*public Dictionary<Type,InvokeOperationDefinition> Definitions
		{
			get { return GetValue( DefinitionsProperty ).To<Dictionary<Type,InvokeOperationDefinition>>(); }
			set { this.SetProperty( DefinitionsProperty, value ); }
		}	public static readonly DependencyProperty DefinitionsProperty = DependencyProperty.Register( "Definitions", typeof(Dictionary<Type,InvokeOperationDefinition>), typeof(SubmitEntitySetSourceOperation), new PropertyMetadata( new Dictionary<Type,InvokeOperationDefinition>() ) );*/
    }
}