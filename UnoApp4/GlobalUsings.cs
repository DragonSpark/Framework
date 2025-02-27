global using System;
global using System.Collections.Immutable;
global using System.IO;
global using System.Net;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.UI.Xaml.Controls;
global using Uno.Extensions.Navigation;
global using Uno.Extensions.Serialization;
global using UnoApp4.DataContracts;
global using UnoApp4.DataContracts.Serialization;
global using UnoApp4.Models;
global using UnoApp4.Presentation;
global using UnoApp4.Services.Caching;
global using UnoApp4.Services.Endpoints;
global using Windows.Networking.Connectivity;
global using Windows.Storage;

[assembly: Uno.Extensions.Reactive.Config.BindableGenerationTool(3)]
