using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Channels
{
    /// <summary>
    /// Can Generate Types
    /// It also haveinternal cache to not generate multiple times the same type
    /// </summary>
    public class TypeGenerator
    {
        TypeGenerator()
        {}

        public static TypeGenerator Instance
        {
            get { return InstanceField; }
        }   static readonly TypeGenerator InstanceField = new TypeGenerator();

        /// <summary>
        /// internal cache for already generated types
        /// </summary>
        readonly Dictionary<Type, Type> asyncTypeCache = new Dictionary<Type, Type>();

        readonly Dictionary<Type, Type> proxyTypeCache = new Dictionary<Type, Type>();

        /// <summary>
        /// provides a cache for the modules
        /// </summary>
        readonly Dictionary<string, ModuleBuilder> moduleBuilderCache = new Dictionary<string, ModuleBuilder>();

        /// <summary>
        /// Generates the Async version of the TSync type.
        /// the generate type repects the AsyncPattern and it is already decorated with attributes for WCF operations
        /// </summary>
        /// <typeparam name="TSync">The Sync version of type</typeparam>
        /// <returns>A type that is the Async version of the TSync type, that implements the AsyncPattern for WCF</returns>
        public Type GenerateAsyncInterfaceFor<TSync>() where TSync : class
        {
            var syncType = typeof(TSync);

            if ( !asyncTypeCache.ContainsKey( syncType ) )
            {
                if ( !syncType.IsInterface )
                {
                    throw new InvalidOperationException( "Only interface type could be transformed" );
                }

                var asynchAssemblyName = DetermineAssemblyName( syncType );

                var attributes = ( syncType.IsPublic ? TypeAttributes.Public : 0 ) | TypeAttributes.Abstract | TypeAttributes.Interface;

                var typeBuilder = GetModuleBuilder( asynchAssemblyName ).DefineType( string.Format( "{0}.Async.{1}", syncType.Namespace, syncType.Name ), attributes );

                foreach ( var method in syncType.GetAllInterfaceMethods() )
                {
                    AddBeginAsynchVersionForMethod( typeBuilder, method, @"http://tempuri.org" );
                    AddEndAsynchVersionForMethod( typeBuilder, method );
                }

                var serviceContractConstructor = typeof(ServiceContractAttribute).GetConstructor( new Type[0] );

                var properties = new[] { "ConfigurationName", "CallbackContract" }.Select( x => typeof(ServiceContractAttribute).GetProperty( x ) ).ToArray();

                var values = new object[]
                    {
                        syncType.FromMetadata<ServiceContractAttribute,string>( x => x.ConfigurationName ) ?? syncType.FullName,
                        syncType.FromMetadata<ServiceContractAttribute,Type>( x => x.CallbackContract ),
                    };
                var builder = new CustomAttributeBuilder( serviceContractConstructor, new object[0], properties, values );

                typeBuilder.SetCustomAttribute( builder );

                var asyncType = typeBuilder.CreateType();

                asyncTypeCache.Add( syncType, asyncType );
                return asyncType;
            }

            return asyncTypeCache[ syncType ];
        }

        static string DetermineAssemblyName( Type syncType )
        {
            return string.Format("{0}.Async", syncType.Namespace);
        }

        public Type GenerateProxyTypeFor<TSync>() where TSync : class
        {
            var syncType = typeof(TSync);

            var result = proxyTypeCache.Ensure( syncType, CreateProxy<TSync> );

            return result;
        }

        Type CreateProxy<TSync>( Type targetType ) where TSync : class
        {
            if ( !targetType.IsInterface )
            {
                throw new InvalidOperationException( "Only interface type could be transformed" );
            }

            var asynchAssemblyName = DetermineAssemblyName( targetType );

            var name = string.Format( "{0}.Async.{1}Proxy", targetType.Namespace, targetType.Name.TrimStart( new[] { 'I' } ) );

            var typeBuilder = GetModuleBuilder( asynchAssemblyName ).DefineType( name, TypeAttributes.Public, typeof(ProxyBase), new[] { typeof(TSync) } );

            var asyncType = GenerateAsyncInterfaceFor<TSync>();

            CreateProxyConstructor( typeBuilder, asyncType );

            foreach ( var method in targetType.GetAllInterfaceMethods() )
            {
                AddProxyImplementation( typeBuilder, method );
            }

            var result = typeBuilder.CreateType();
            return result;
        }

        static void CreateProxyConstructor( TypeBuilder typeBuilder, Type asyncType )
        {
            const MethodAttributes methodAttributes = MethodAttributes.Public| MethodAttributes.HideBySig;
            MethodBuilder method = typeBuilder.DefineMethod( ".ctor", methodAttributes );
            // Preparing Reflection instances
            var ctor1 = typeof(ProxyBase).GetConstructor( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(Object) }, null );
            // Setting return type
            method.SetReturnType( typeof(void) );
            // Adding parameters
            method.SetParameters( asyncType );
            // Parameter async
            var async = method.DefineParameter( 1, ParameterAttributes.None, "async" );
            var gen = method.GetILGenerator();
            // Writing body
            gen.Emit( OpCodes.Ldarg_0 );
            gen.Emit( OpCodes.Ldarg_1 );
            gen.Emit( OpCodes.Call, ctor1 );
            gen.Emit( OpCodes.Nop );
            gen.Emit( OpCodes.Nop );
            gen.Emit( OpCodes.Nop );
            gen.Emit( OpCodes.Ret );
        }

        void AddProxyImplementation( TypeBuilder typeBuilder, MethodInfo @reference )
        {
            // Declaring method builder
            // Method attributes
            const MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot;
            
            var method = typeBuilder.DefineMethod( @reference.Name, methodAttributes );
            
            // Preparing Reflection instances
            var method1 = typeof(MethodBase).GetMethod( "GetCurrentMethod", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] {}, null );
            var method2 = typeof(ProxyBase).GetMethod( "Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(MethodBase), typeof(Object[]) }, null );
            
            // Setting return type
            method.SetReturnType( @reference.ReturnType );
            
            // Adding parameters
            var parameterTypes = @reference.GetParameters().Select( x => x.ParameterType ).ToArray();
            
            method.SetParameters( parameterTypes );
            
            var count = 0;
            parameterTypes.Apply( x => method.DefineParameter( ++count, ParameterAttributes.None, x.Name ) );
            
            var gen = method.GetILGenerator();
           
            if ( @reference.ReturnType == typeof(void) )
            {
                gen.DeclareLocal(typeof(Object[]));
                gen.DeclareLocal(typeof(Object[]));
                
                gen.Emit(OpCodes.Nop);
                gen.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
                gen.Emit(OpCodes.Newarr, typeof(object));

                gen.Emit(OpCodes.Stloc_1);

                count = 0;
                parameterTypes.Apply( x =>
                {
                    gen.Emit(OpCodes.Ldloc_1);
                    gen.Emit(OpCodes.Ldc_I4, count++ );
                    gen.Emit(OpCodes.Ldarg, count );
                    gen.Emit(OpCodes.Box, x );
                    gen.Emit(OpCodes.Stelem_Ref);
                } );
                
                gen.Emit(OpCodes.Ldloc_1);
                gen.Emit(OpCodes.Stloc_0);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call,method1);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Call,method2);
                gen.Emit(OpCodes.Pop);
                gen.Emit(OpCodes.Ret);
            }
            else
            {
                // Preparing locals
                gen.DeclareLocal(typeof(Object[]));
                gen.DeclareLocal(typeof(String));
                gen.DeclareLocal(typeof(String));
                gen.DeclareLocal(typeof(Object[]));

                // Preparing labels
                Label label54 =  gen.DefineLabel();
                // Writing body

                gen.Emit(OpCodes.Nop);
                gen.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
                gen.Emit(OpCodes.Newarr,typeof(object));
                gen.Emit(OpCodes.Stloc_3);

                count = 0;
                parameterTypes.Apply( x =>
                {
                    gen.Emit(OpCodes.Ldloc_3);
                    gen.Emit(OpCodes.Ldc_I4, count++ );
                    gen.Emit(OpCodes.Ldarg, count );
                    gen.Emit(OpCodes.Box, x );
                    gen.Emit(OpCodes.Stelem_Ref);
                } );

                gen.Emit(OpCodes.Ldloc_3);
                gen.Emit(OpCodes.Stloc_0);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call,method1);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Call,method2);
                gen.Emit(OpCodes.Castclass,typeof(string));
                gen.Emit(OpCodes.Stloc_1);
                gen.Emit(OpCodes.Ldloc_1);
                gen.Emit(OpCodes.Stloc_2);
                gen.Emit(OpCodes.Br_S,label54);
                gen.MarkLabel(label54);
                gen.Emit(OpCodes.Ldloc_2);
                gen.Emit(OpCodes.Ret);

            }
        }

        /// <summary>
        /// Creates a End verison of a sync method, that implements the AsyncPattern
        /// </summary>
        /// <param name="typeBuilder">the tipebuilder where the type is being building</param>
        /// <param name="method">information about the sync version of the method</param>
        private void AddEndAsynchVersionForMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            var endMethodName = string.Format("End{0}", method.Name);

            var parameters = new
                {
                    Type = typeof(IAsyncResult),
                    Name = "asyncResult",
                    Attributes = ParameterAttributes.None,
                }.ToEnumerable().ToList();

            var methodBuilder =
                typeBuilder
                .DefineMethod(
                    endMethodName,
                    method.Attributes,
                    method.CallingConvention,
                    method.ReturnType,
                    parameters.Select(x => x.Type).ToArray());

            for (int i = 0; i < parameters.Count(); i++)
            {
                var parameter = parameters[i];
                methodBuilder.DefineParameter(i + 1, parameter.Attributes, parameter.Name);             
            }
        }

        /// <summary>
        /// Creates a Begin verison of a sync method, that implements the AsyncPattern
        /// </summary>
        /// <param name="typeBuilder">the tipebuilder where the type is being building</param>
        /// <param name="method">information about the sync version of the method</param>
        private void AddBeginAsynchVersionForMethod(TypeBuilder typeBuilder, MethodInfo method, string nameSpace)
        {
            string beginMethodName = string.Format("Begin{0}", method.Name);

            var parametersTypeList = method.GetParameters().Select(x => x.ParameterType).ToList();
            var parametersNameList = method.GetParameters().Select(x => x.Name).ToList();
            var parametersAttributeList = method.GetParameters().Select(x => x.Attributes).ToList();

            parametersTypeList.Add(typeof(AsyncCallback));
            parametersAttributeList.Add(ParameterAttributes.None);
            parametersNameList.Add("callBack");

            parametersTypeList.Add(typeof(object));
            parametersAttributeList.Add(ParameterAttributes.None);
            parametersNameList.Add("statusObject");

            var methodBuilder = 
                typeBuilder
                .DefineMethod(
                    beginMethodName,
                    method.Attributes,
                    method.CallingConvention,
                    typeof(IAsyncResult),
                    parametersTypeList.ToArray());

            for (int i = 0; i < parametersTypeList.Count(); i++)
            {
                methodBuilder.DefineParameter(i + 1, parametersAttributeList[i], parametersNameList[i]);
            }

            var operationContractConstructor = typeof(OperationContractAttribute).GetConstructor(new Type[0]);
            var asynchPatternProperty = typeof(OperationContractAttribute).GetProperty("AsyncPattern");

            var actionProperty = typeof(OperationContractAttribute).GetProperty("Action");
            var actionValue = string.Format("{0}/{1}/{2}", nameSpace, method.DeclaringType.Name, method.Name);

            var replyActionProperty = typeof(OperationContractAttribute).GetProperty("ReplyAction");
            var replyActionValue = string.Format("{0}/{1}/{2}Response", nameSpace, method.DeclaringType.Name, method.Name);

            var attribuiteBuilder = 
                new CustomAttributeBuilder(
                    operationContractConstructor, 
                    new object[0],
                    new[] { asynchPatternProperty, actionProperty, replyActionProperty },
                    new object[] { true, actionValue, replyActionValue });

            

            methodBuilder.SetCustomAttribute(attribuiteBuilder);
        }

        /// <summary>
        /// provides a ModelBuilder with the required assembly name
        /// </summary>
        /// <param name="requiredAssemblyName">the assembly name for where the type will be generated in</param>
        /// <returns>a model builder</returns>
        /// <remarks>in this version the model builder is not cached, it could be interesting to generate all the types in the same assembly by caching the model builder</remarks>
        private ModuleBuilder GetModuleBuilder(string requiredAssemblyName)
        {
            if (moduleBuilderCache.ContainsKey(requiredAssemblyName))
            {
                return moduleBuilderCache[requiredAssemblyName];
            }

            AssemblyName assemblyName = new AssemblyName(requiredAssemblyName);
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
            
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            moduleBuilderCache[requiredAssemblyName] = moduleBuilder;

            return moduleBuilder;
        }
    }
}
