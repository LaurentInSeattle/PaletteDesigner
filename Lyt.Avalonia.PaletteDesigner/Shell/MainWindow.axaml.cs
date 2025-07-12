namespace Lyt.Avalonia.PaletteDesigner.Shell;

public partial class MainWindow : Window
{
    private bool isShutdownRequested;

    private bool isShutdownComplete;

    public MainWindow()
    {
        this.InitializeComponent();

        this.Closing += this.OnMainWindowClosing;
        this.Loaded += (s, e) => { Dispatch.OnUiThread(this.OnLoadedOnUi); }; 
    }

    private void OnLoadedOnUi()
    {
        var vm = App.GetRequiredService<ShellViewModel>();
        vm.CreateViewAndBind();
        this.Content = vm.View;
    }
    
    private void OnMainWindowClosing(object? sender, CancelEventArgs e)
    {
        if (!this.isShutdownComplete)
        {
            e.Cancel = true;
        }

        if (!this.isShutdownRequested)
        {
            this.isShutdownRequested = true;
            Schedule.OnUiThread(50,
                async () =>
                {
                    var application = App.GetRequiredService<IApplicationBase>();
                    await application.Shutdown();
                    this.isShutdownComplete = true;
                    this.Close();
                }, DispatcherPriority.Normal);
        }
    }
}
