// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.ServiceModel.DomainServices.Client" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "DragonSpark.Application.Communication.Security.ClaimsMapperBase`1.#DragonSpark.Application.Communication.Security.IClaimsMapper.Map(System.IdentityModel.Claims.Claim,System.ServiceModel.DomainServices.Server.ApplicationServices.IUser)" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "DragonSpark.Application.Communication.Entity.Notifications.NotificationProcessorBase`1.#DragonSpark.Application.Communication.Entity.Notifications.INotificationProcessor.Process(DragonSpark.Application.Communication.Entity.Notifications.INotification)" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "DragonSpark.Application.Communication.ClientChannelFactory`1.#.cctor()" )]
