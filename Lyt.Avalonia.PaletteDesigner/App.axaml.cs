namespace Lyt.Avalonia.PaletteDesigner;

public partial class App : ApplicationBase
{
    public const string Organization = "Lyt";
    public const string Application = "PaletteDesigner";
    public const string RootNamespace = "Lyt.Avalonia.PaletteDesigner";
    public const string AssemblyName = "Lyt.Avalonia.PaletteDesigner";
    public const string AssetsFolder = "Assets";

    public App() : base(
        App.Organization,
        App.Application,
        App.RootNamespace,
        InitializeHosting,
        GetModelTypes,
        singleInstanceRequested: false,
        splashImageUri: null,
        appSplashWindow: new SplashWindow()
        )
    {
        // This should be empty, use the OnStartup override
        Instance = this;
        Debug.WriteLine("App Instance created");
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public static App Instance { get; private set; }
#pragma warning restore CS8618 

    private static Tuple<Type, Type> LoggerService =>
            Debugger.IsAttached ?
                new Tuple<Type, Type>(typeof(ILogger), typeof(LogViewerWindow)) :
                new Tuple<Type, Type>(typeof(ILogger), typeof(Logger));

    public bool RestartRequired { get; set; }

    public static List<Type> GetModelTypes()
        => [typeof(FileManagerModel), typeof(PaletteDesignerModel)];

    public static IHost InitializeHosting()
    {
        IServiceCollection? registeredServices = null;
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_0, services) =>
            {
                // Register the app
                _ = services.AddSingleton<IApplicationBase>(App.Instance);

                // Always Main Window 
                _ = services.AddSingleton<Window, MainWindow>();

                // The Application Model, also  a singleton, no need here to also add it without the inferface  
                _ = services.AddSingleton<IApplicationModel, ApplicationModelBase>(); // Top level model

                // Models 
                _ = services.AddSingleton<FileManagerModel>();
                _ = services.AddSingleton<PaletteDesignerModel>();

                // Singletons, they do not need an interface. 
                //
                // Shell 
                _ = services.AddSingleton<ShellViewModel>();

                // Views and ViewModels from the main view selector            
                // Singletons
                _ = services.AddSingleton<DesignViewModel>();
                _ = services.AddSingleton<PaletteColorViewModel>();
                _ = services.AddSingleton<PalettePreviewViewModel>();
                _ = services.AddSingleton<ColorWheelViewModel>();

                _ = services.AddSingleton<ImagingViewModel>();
                _ = services.AddSingleton<SettingsViewModel>();
                _ = services.AddSingleton<WizardViewModel>();
                _ = services.AddSingleton<LanguageViewModel>();
                _ = services.AddSingleton<LanguageToolbarViewModel>();

                // Services 
                 _ = services.AddSingleton<ILogger, BasicLogger>();
                // _ = services.AddSingleton<ILogger, LogViewerWindow>();
                _ = services.AddSingleton<IFocuser, Focuser>();
                _ = services.AddSingleton<IAnimationService, AnimationService>();
                _ = services.AddSingleton<ILocalizer, LocalizerModel>();
                _ = services.AddSingleton<IDialogService, DialogService>();
                _ = services.AddSingleton<IDispatch, Dispatch>();
                _ = services.AddSingleton<IProfiler, Profiler>();
                _ = services.AddSingleton<IToaster, Toaster>();
                _ = services.AddSingleton<IRandomizer, Randomizer>();

                registeredServices = services;
            }).Build();

        return host;
    }

    protected override async Task OnStartupBegin()
    {
        ViewModel.TypeInitialize(ApplicationBase.AppHost);

        var logger = App.GetRequiredService<ILogger>();
        logger.Debug("OnStartupBegin begins");

        // This needs to complete before all models are initialized.
        var fileManager = App.GetRequiredService<FileManagerModel>();
        await fileManager.Configure(
            new FileManagerConfiguration(
                App.Organization, App.Application, App.RootNamespace, App.AssemblyName, App.AssetsFolder));

        // The localizer needs the File Manager, do not change the order.
        var localizer = App.GetRequiredService<ILocalizer>();
        await localizer.Configure(
            new LocalizerConfiguration
            {
                AssemblyName = App.AssemblyName,
                Languages =
                [
                    "en-US", "hu-HU",
                    "fr-FR", "it-IT", "es-ES", "de-DE",
                    "uk-UA", "bg-BG", "el-GR", "hy-AM",
                    "jp-JP", "ko-KO", "zh-CN", "zh-TW",
                    "hi-IN", "bn-BD"
                ],
                // Use default for all other config parameters 
            });

        logger.Debug("OnStartupBegin complete");
    }

    protected override Task OnShutdownComplete()
    {
        var logger = App.GetRequiredService<ILogger>();
        logger.Debug("On Shutdown Complete");

        if (this.RestartRequired)
        {
            logger.Debug("On Shutdown Complete: Restart Required");
            var process = Process.GetCurrentProcess();
            if ((process is not null) && (process.MainModule is not null))
            {
                Process.Start(process.MainModule.FileName);
            }
        }

        return Task.CompletedTask;
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);
}
