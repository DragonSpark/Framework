using DragonSpark.Application.Console.Markup;
using DragonSpark.Application.Console.Model;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Xaml;
using System.Xml;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Console
{
    public class ServiceLauncher : Launcher
    {
        const string StateFileName = "State.xaml";
        readonly IsolatedStorageFile _storage = IsolatedStorageFile.GetMachineStoreForAssembly();

        public override void Run( bool runWithDefaultConfiguration )
        {
            base.Run( runWithDefaultConfiguration );
            this.BuildUpOnce();
        }

        public ServiceLauncherState State
        {
            get
            {
                var result = _storage.FileExists( StateFileName ) ? Read() : ServiceLauncherState.Stopped;
                return result;
            }
            set
            {
                using ( var file = _storage.OpenFile( StateFileName, FileMode.Create, FileAccess.Write, FileShare.Read ) )
                {
                    XamlServices.Save( file, value );
                }
            }
        }

        ServiceLauncherState Read()
        {
            RETRY:
            try
            {
                using ( var file = _storage.OpenFile( StateFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
                {
                    var result = (ServiceLauncherState)XamlServices.Load( file );
                    return result;
                }
            }
            catch ( XmlException )
            {
                goto RETRY;
            }
        }

        public Type ServiceType { get; set; }

        protected ServiceHost Host
        {
            get { return _host ?? ( _host = new ServiceHost( ServiceType ) ); }
        }	ServiceHost _host;
        
        protected virtual void OnStarting()
        {
            Host.Open();
        }

        protected virtual void OnStopping()
        {
            Host.Close( TimeSpan.FromMilliseconds( 250 ) );
            Host.TryDispose();
        }

        [Verb( "/launch", Description = "Launches this service" )]
        public void Launch()
        {
            switch ( State )
            {
                case ServiceLauncherState.Stopped:
                    State = ServiceLauncherState.Starting;
                    var info = new ProcessStartInfo( System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace( ".vshost", string.Empty ), "/start" ) { CreateNoWindow = true, UseShellExecute = false };
                    System.Diagnostics.Process.Start( info );
                    do
                    {
                        Thread.Sleep( 250 );
                    }
                    while ( State != ServiceLauncherState.Started );
                    break;
            }
        }

        [Verb( "/start", Description = "Starts this service" )]
        public void Start()
        {
            OnStarting();

            var name = GetType().FullName;
            DragonSpark.Runtime.Logging.Information( string.Format( "Starting Service: {0}", name ) );

            State = ServiceLauncherState.Started;

            while ( State == ServiceLauncherState.Started )
            {
                Thread.Sleep( 250 );
            }

            try
            {
                OnStopping();
            }
            finally 
            {
                State = ServiceLauncherState.Stopped;
            }
        }

        [Verb( "/stop", Description = "Stops this service" )]
        public void Stop()
        {
            var name = GetType().FullName;
            DragonSpark.Runtime.Logging.Information( string.Format( "Stopping Service: {0}", name ) );
            State = ServiceLauncherState.Stopping;
            while ( State != ServiceLauncherState.Stopped )
            {
                Thread.Sleep( 250 );
            }
            DragonSpark.Runtime.Logging.Information( string.Format( "Service Stopped: {0}", name ) );
        }
    }

    public static class ConsoleExtensions
    {
        public static IEnumerable<MethodInfo> ConsoleMethods( this Type t )
        {
            var methods = t.GetMethods( BindingFlags.Public | BindingFlags.Static );
            return methods.Where( method => method.IsDefined( typeof(VerbAttribute), false ) );
        }

        public static MethodInfo ConsoleMethod( this Type t, string name )
        {
            return t.ConsoleMethods().FirstOrDefault( method => method.ConsoleVerb().VerbName == name );
        }

        public static IEnumerable<ArgumentAttribute> ConsoleArguments( this MethodInfo method )
        {
            return method.GetParameters().Select( parameter => parameter.ConsoleArgument() ).Where( currentAttribute => currentAttribute != null );
        }

        public static ParameterInfo ConsoleArgument( this MethodInfo method, ArgumentAttribute attribute )
        {
            return ( from parameter in method.GetParameters() let currentAttribute = parameter.ConsoleArgument() where currentAttribute.Name == attribute.Name select parameter ).FirstOrDefault();
        }

        public static ArgumentAttribute ConsoleArgument( this ParameterInfo parameter )
        {
            return parameter.IsDefined( typeof(ArgumentAttribute), false )
                       ? parameter.GetCustomAttributes( typeof(ArgumentAttribute), false )[ 0 ] as ArgumentAttribute
                       : null;
        }

        public static ArgumentAttribute ConsoleArgument( this MethodInfo method, string name )
        {
            return method.ConsoleArguments().FirstOrDefault( attribute => attribute.Name == name );
        }

        public static VerbAttribute ConsoleVerb( this MethodInfo method )
        {
            return method.IsDefined( typeof(VerbAttribute), false )
                       ? method.GetCustomAttributes( typeof(VerbAttribute), false )[ 0 ] as VerbAttribute
                       : null;
        }

        public static void Execute( this Type programType, string[] args )
        {
            PrintCopyright( programType );

            try
            {
                var launcher = Activator.CreateInstance<Launcher>( programType );
                launcher.Run();
                var model = new ProgramModel( programType );

                if ( args.Length == 0 )
                {
                    if ( model.DefaultVerb != null )
                    {
                        DragonSpark.Runtime.Logging.TryAndHandle( () => model.DefaultVerb.Method.Invoke( launcher, null ) );
                    }
                    else
                    {
                        PrintHelp( programType );
                    }
                }
                else
                {
                    var i = 0;
                    var verb = ( from v in model.Verbs where v.Name == args[ i ] select v ).SingleOrDefault();
                        //programType.ConsoleMethod(args[i]);
                    if ( verb == null )
                    {
                        // is there a default verb?
                        if ( model.DefaultVerb == null )
                        {
                            PrintHelp( programType );
                        }
                        else
                        {
                            verb = model.DefaultVerb;
                            //i++; // arg consumed, move to next argument
                            for ( ; i < args.Length; i++ )
                            {
                                var p = ( from a in verb.Arguments where a.Name == args[ i ] select a ).SingleOrDefault();
                                if ( p == null )
                                {
                                    throw new InvalidOperationException(
                                        string.Format( "Program command '{0}' does not take the argument '{1}'.",
                                                       verb.Name, args[ i ] ) );
                                    //if (verb.DefaultArgument == null)
                                    //{
                                    //    System.Console.WriteLine(string.Format("Unknown command: {0}", args[i]));
                                    //    System.Console.WriteLine();
                                    //    PrintHelp(programType);
                                    //    return;
                                    //}
                                    //else
                                    //{
                                    //    p = verb.DefaultArgument;
                                    //    if (!AddParameterToArguments(programType, p, args[i], argumentValues))
                                    //        return;
                                    //}
                                }
                                i++;
                                if ( !AddParameterToArguments( programType, p, args[ i ] ) )
                                {
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        i++; // arg consumed, move to next
                        if ( i < args.Length )
                        {
                            var p = ( from a in verb.Arguments where a.Name == args[ i ] select a ).SingleOrDefault();
                            if ( p != null )
                            {
                                for ( ; i < args.Length; i++ )
                                {
                                    p = ( from a in verb.Arguments where a.Name == args[ i ] select a ).SingleOrDefault();
                                    if ( p == null )
                                    {
                                        throw new InvalidOperationException(
                                            string.Format( "Program command '{0}' does not take the argument '{1}'.",
                                                           verb.Name, args[ i ] ) );
                                    }
                                    i++; // arg consumed, move to next
                                    if ( !AddParameterToArguments( programType, p, args[ i ] ) )
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException(
                                    string.Format( "Program command '{0}' does not take the argument '{1}'.", verb.Name,
                                                   args[ i ] ) );
                            }
                        }
                    }

                    if ( verb != null )
                    {
                        verb.Execute( launcher );
                    }
                }
            }
            /*catch ( TargetInvocationException e )
            {
                throw e.InnerException;
            }*/
            catch ( InvalidOperationException ioex )
            {
                System.Console.WriteLine( ioex.Message );
                System.Console.WriteLine();
                programType.PrintHelp();
            }
        }

        static bool AddParameterToArguments( Type programType, ArgumentModel argumentModel, string argumentValue )
        {
            // attempt to run program
            var converter = TypeDescriptor.GetConverter( argumentModel.Parameter.ParameterType );
            if ( converter.CanConvertFrom( typeof(string) ) )
            {
                try
                {
                    argumentModel.RuntimeValue = converter.ConvertFrom( argumentValue );
                }
                catch ( Exception )
                {
                    System.Console.WriteLine( string.Format( "Error: Value '{0}' is not valid for argument <{1}>.",
                                                             argumentValue, argumentModel.Name ) );
                    System.Console.WriteLine();
                    programType.PrintHelp();
                    return false;
                }
            }
            else
            {
                throw new ArgumentException(
                    string.Format( "Could not locate a TypeConverter capable of converting from System.String to {0}",
                                   argumentModel.Parameter.ParameterType.FullName ) );
            }

            return true;
        }

        public static T GetSingleAttribute<T>( this ICustomAttributeProvider info )
        {
            return (T)( from attr in info.GetCustomAttributes( typeof(T), false ) select attr ).SingleOrDefault();
        }

        public static void PrintCopyright( this Type programType )
        {
            var companyAttr = GetSingleAttribute<AssemblyCompanyAttribute>( programType.Assembly );
            var productAttr = GetSingleAttribute<AssemblyProductAttribute>( programType.Assembly );
            var versionAttr = GetSingleAttribute<AssemblyFileVersionAttribute>( programType.Assembly );

            if ( companyAttr != null )
            {
                System.Console.WriteLine( "{0} {1}. Version {2}", companyAttr.Company, productAttr.Product,
                                          versionAttr.Version );

                var copyrightAttr = GetSingleAttribute<AssemblyCopyrightAttribute>( programType.Assembly );
                System.Console.WriteLine( "{0}. All rights reserved.", copyrightAttr.Copyright );

                System.Console.WriteLine();
            }
        }

        public static void PrintHelp( this Type programType )
        {
            var model = new ProgramModel( programType );

            System.Console.WriteLine( string.Format( "Usage: {0} <command> [ <options> ]",
                                                     Path.GetFileName( programType.Assembly.Location ) ) );

            System.Console.WriteLine( "Commands:" );
            foreach ( VerbModel verb in model.Verbs )
            {
                System.Console.Write( "  {0}", verb.Name );

                foreach ( ArgumentModel arg in verb.Arguments )
                {
                    System.Console.Write( " " );

                    if ( arg.Default != null )
                    {
                        System.Console.Write( "[" );
                    }

                    System.Console.Write( "{0}", arg.Name );
                    if ( arg.Description != null )
                    {
                        System.Console.Write( " <{0}>", arg.Description );
                    }
                    else
                    {
                        System.Console.Write( " <{0}>", arg.ParameterName );
                    }

                    if ( arg.Default != null )
                    {
                        System.Console.Write( "]" );
                    }
                }
                System.Console.WriteLine();
                if ( verb.Description != null )
                {
                    string[] argumentNameList = ( from a in verb.Arguments select a.Name ).ToArray();
                    System.Console.WriteLine( "    {0}", string.Format( verb.Description, argumentNameList ) );
                }
                System.Console.WriteLine();
            }
        }
    }
}
