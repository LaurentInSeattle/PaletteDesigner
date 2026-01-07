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
        typeof(MainWindow),
        typeof(ApplicationModelBase), // Top level model 
        [
            // Models 
            typeof(FileManagerModel),
            typeof(PaletteDesignerModel),
        ],
        [
           // Singletons
           typeof(ShellViewModel),

           typeof(LanguageViewModel),
           
           typeof(DesignViewModel),
           typeof(PaletteColorViewModel),
           typeof(PalettePreviewViewModel),
           typeof(ColorWheelViewModel),


           typeof(ImagingViewModel),

           // Disabled for now
           // typeof(SettingsViewModel),
           // typeof(MappingViewModel),
        ],
        [
            // Services 
            App.LoggerService,
            new Tuple<Type, Type>(typeof(IAnimationService), typeof(AnimationService)),
            new Tuple<Type, Type>(typeof(ILocalizer), typeof(LocalizerModel)),
            new Tuple<Type, Type>(typeof(IDialogService), typeof(DialogService)),
            new Tuple<Type, Type>(typeof(IDispatch), typeof(Dispatch)),
            new Tuple<Type, Type>(typeof(IProfiler), typeof(Profiler)),
            new Tuple<Type, Type>(typeof(IToaster), typeof(Toaster)),
            new Tuple<Type, Type>(typeof(IRandomizer), typeof(Randomizer)),
        ],
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
