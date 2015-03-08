using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.LightSwitch.Security.ClientGenerated.Implementation.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	[DomainIdentifier("AuthenticationService")]
    public class AuthenticationDomainContext : System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationDomainContextBase
    {
        // Methods
        public AuthenticationDomainContext( Type type ) : base( new WebDomainClient<IAuthenticationServiceContract>( new Uri( Resolve( type ), UriKind.Relative ) ) )
        {}

	    protected static string Resolve( Type type )
	    {
		    var result = string.Concat( type.Namespace.Replace( '.', '-' ), "-", type.FromMetadata<DomainIdentifierAttribute, string>( x => x.Name ), ".svc" );
		    return result;
	    }

	    /*public AuthenticationDomainContext( DomainClient domainClient ) : base( domainClient )
        {}

        public AuthenticationDomainContext( Uri serviceUri ) : base( new WebDomainClient<IAuthenticationServiceContract>( serviceUri ) )
        {}*/

        protected override EntityContainer CreateEntityContainer()
        {
            return new AuthenticationEntityContainer();
        }

        public EntityQuery<AuthenticationInfo> GetAuthenticationInfoQuery()
        {
            return CreateQuery<AuthenticationInfo>( "GetAuthenticationInfo", null, false, false );
        }

        public EntityQuery<User> GetUserQuery()
        {
            ValidateMethod( "GetUserQuery", null );
            return CreateQuery<User>( "GetUser", null, false, false );
        }

        public EntityQuery<User> LoginQuery( string userName, string password, bool isPersistent, string customData )
        {
            var dictionary2 = new Dictionary<string, object> { { "userName", userName }, { "password", password }, { "isPersistent", isPersistent }, { "customData", customData } };
            var parameters = dictionary2;
            ValidateMethod( "LoginQuery", parameters );
            var result = CreateQuery<User>( "Login", parameters, false, false );
            return result;
        }

        /// <summary>
        /// Gets an EntityQuery instance that can be used to load <see cref="User"/> entity instances using the 'Logout' query.
        /// </summary>
        /// <returns>An EntityQuery that can be loaded to retrieve <see cref="User"/> entity instances.</returns>
        public EntityQuery<User> LogoutQuery()
        {
            ValidateMethod( "LogoutQuery", null );
            return CreateQuery<User>( "Logout", null, true, false );
        }

        // Nested Types
        internal sealed class AuthenticationEntityContainer : EntityContainer
        {
            // Methods
            public AuthenticationEntityContainer()
            {
                CreateEntitySet<User>( EntitySetOperations.Edit );
                CreateEntitySet<IdentityClaim>( EntitySetOperations.None );
                CreateEntitySet<AuthenticationInfo>( EntitySetOperations.None );
            }
        }
    }

    [DataContract( Namespace = "http://schemas.datacontract.org/2004/07/DragonSpark.Application.Communication.Security" )]
    public sealed partial class IdentityClaim : System.ServiceModel.DomainServices.Client.Entity
    {
        Guid _id;

        string _issuer;

        string _originalIssuer;

        string _type;

        string _userName;

        string _value;

        string _valueType;

        #region Extensibility Method Definitions
        /// <summary>
        /// This method is invoked from the constructor once initialization is complete and
        /// can be used for further object setup.
        /// </summary>
        partial void OnCreated();

        partial void OnIdChanging( Guid value );
        partial void OnIdChanged();
        partial void OnIssuerChanging( string value );
        partial void OnIssuerChanged();
        partial void OnOriginalIssuerChanging( string value );
        partial void OnOriginalIssuerChanged();
        partial void OnTypeChanging( string value );
        partial void OnTypeChanged();
        partial void OnUserNameChanging( string value );
        partial void OnUserNameChanged();
        partial void OnValueChanging( string value );
        partial void OnValueChanged();
        partial void OnValueTypeChanging( string value );
        partial void OnValueTypeChanged();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityClaim"/> class.
        /// </summary>
        public IdentityClaim()
        {
            OnCreated();
        }

        /// <summary>
        /// Gets or sets the 'Id' value.
        /// </summary>
        [DataMember, Editable( false, AllowInitialValue = true ), Key, NewGuidDefaultValue, RoundtripOriginal]
        public Guid Id
        {
            get { return _id; }
            set
            {
                if ( ( _id != value ) )
                {
                    OnIdChanging( value );
                    ValidateProperty( "Id", value );
                    _id = value;
                    RaisePropertyChanged( "Id" );
                    OnIdChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'Issuer' value.
        /// </summary>
        [DataMember]
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                if ( ( _issuer != value ) )
                {
                    OnIssuerChanging( value );
                    RaiseDataMemberChanging( "Issuer" );
                    ValidateProperty( "Issuer", value );
                    _issuer = value;
                    RaiseDataMemberChanged( "Issuer" );
                    OnIssuerChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'OriginalIssuer' value.
        /// </summary>
        [DataMember]
        public string OriginalIssuer
        {
            get { return _originalIssuer; }
            set
            {
                if ( ( _originalIssuer != value ) )
                {
                    OnOriginalIssuerChanging( value );
                    RaiseDataMemberChanging( "OriginalIssuer" );
                    ValidateProperty( "OriginalIssuer", value );
                    _originalIssuer = value;
                    RaiseDataMemberChanged( "OriginalIssuer" );
                    OnOriginalIssuerChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'Type' value.
        /// </summary>
        [DataMember]
        public string Type
        {
            get { return _type; }
            set
            {
                if ( ( _type != value ) )
                {
                    OnTypeChanging( value );
                    RaiseDataMemberChanging( "Type" );
                    ValidateProperty( "Type", value );
                    _type = value;
                    RaiseDataMemberChanged( "Type" );
                    OnTypeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'UserName' value.
        /// </summary>
        [DataMember, StringLength( 128 )]
        
        public string UserName
        {
            get { return _userName; }
            set
            {
                if ( ( _userName != value ) )
                {
                    OnUserNameChanging( value );
                    RaiseDataMemberChanging( "UserName" );
                    ValidateProperty( "UserName", value );
                    _userName = value;
                    RaiseDataMemberChanged( "UserName" );
                    OnUserNameChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'Value' value.
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return _value; }
            set
            {
                if ( ( _value != value ) )
                {
                    OnValueChanging( value );
                    RaiseDataMemberChanging( "Value" );
                    ValidateProperty( "Value", value );
                    _value = value;
                    RaiseDataMemberChanged( "Value" );
                    OnValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the 'ValueType' value.
        /// </summary>
        [DataMember]
        public string ValueType
        {
            get { return _valueType; }
            set
            {
                if ( ( _valueType != value ) )
                {
                    OnValueTypeChanging( value );
                    RaiseDataMemberChanging( "ValueType" );
                    ValidateProperty( "ValueType", value );
                    _valueType = value;
                    RaiseDataMemberChanged( "ValueType" );
                    OnValueTypeChanged();
                }
            }
        }

        /// <summary>
        /// Computes a value from the key fields that uniquely identifies this entity instance.
        /// </summary>
        /// <returns>An object instance that uniquely identifies this entity instance.</returns>
        public override object GetIdentity()
        {
            return _id;
        }
    }
}
