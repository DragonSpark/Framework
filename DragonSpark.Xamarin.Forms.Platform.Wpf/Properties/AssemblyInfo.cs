﻿using DragonSpark.Xamarin.Forms.Platform.Wpf.Infrastructure;
using DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using Xamarin.Forms;

[assembly:ThemeInfo( ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly )]

[assembly: AssemblyVersion("1.3.3.0")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyCompany("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyTitle("")]
[assembly: AssemblyTrademark("")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CompilationRelaxations(8)]
[assembly: InternalsVisibleTo("DragonSpark.Xamarin.Forms.ApplicationHost.Wpf.Testing")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: ComVisible(false)]
[assembly: Guid("50e5d817-4ac1-46c0-8e20-260fb6ad16af")]
// [assembly: TargetFramework("WindowsPhone,Version=v8.0", FrameworkDisplayName = "Windows Phone 8.0")]
[assembly: Xamarin.Forms.Dependency(typeof(Deserializer))]
[assembly: Xamarin.Forms.Dependency(typeof(ResourcesProvider))]
[assembly: ExportCell(typeof(Cell), typeof(TextCellRenderer))]
[assembly: ExportCell(typeof(ImageCell), typeof(ImageCellRenderer))]
[assembly: ExportCell(typeof(EntryCell), typeof(CellRenderer))]
[assembly: ExportCell(typeof(SwitchCell), typeof(CellRenderer))]
[assembly: ExportCell(typeof(ViewCell), typeof(CellRenderer))]
[assembly: ExportImageSourceHandler(typeof(FileImageSource), typeof(FileImageSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(StreamImageSource), typeof(StreamImagesourceHandler))]
[assembly: ExportImageSourceHandler(typeof(UriImageSource), typeof(ImageLoaderSourceHandler))]
[assembly: ExportRenderer(typeof(Button), typeof(ButtonRenderer))]
[assembly: ExportRenderer(typeof(ProgressBar), typeof(ProgressBarRenderer))]
[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(BoxView), typeof(BoxViewRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(EntryRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
[assembly: ExportRenderer(typeof(Label), typeof(LabelRenderer))]
[assembly: ExportRenderer(typeof(Image), typeof(ImageRenderer))]
[assembly: ExportRenderer(typeof(Slider), typeof(SliderRenderer))]
[assembly: ExportRenderer(typeof(WebView), typeof(WebViewRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Switch), typeof(SwitchRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]
[assembly: ExportRenderer(typeof(Stepper), typeof(StepperRenderer))]
[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer))]
[assembly: ExportRenderer(typeof(Frame), typeof(FrameRenderer))]
[assembly: ExportRenderer(typeof(NavigationMenu), typeof(NavigationMenuRenderer))]
[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.TableView), typeof(TableViewRenderer))]
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
[assembly: ExportRenderer(typeof(CarouselPage), typeof(CarouselPageRenderer))]
[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]
[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailRenderer))]
