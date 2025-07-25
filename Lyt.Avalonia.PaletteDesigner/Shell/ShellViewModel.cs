namespace Lyt.Avalonia.PaletteDesigner.Shell;

using static ViewActivationMessage;
using static MessagingExtensions;
using Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class ShellViewModel : ViewModel<ShellView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    public bool mainToolbarIsVisible;

    private ViewSelector<ActivatedView>? viewSelector;
    
    // private bool isFirstActivation;

    #region To please the XAML viewer 

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Should never be executed 
    public ShellViewModel()
    {
    }
#pragma warning restore CS8618 

    #endregion To please the XAML viewer 

    public ShellViewModel(PaletteDesignerModel paletteDesignerModel, IToaster toaster)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.toaster = toaster;

        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    private void OnLanguageChanged(LanguageChangedMessage message)
    {
    }

    public override void OnViewLoaded()
    {
        this.Logger.Debug("OnViewLoaded begins");

        base.OnViewLoaded();
        if (this.View is null)
        {
            throw new Exception("Failed to startup...");
        }

        // Select default language 
        string preferredLanguage = "en-US" ; // this.astroPicModel.Language;
        this.Logger.Debug("Language: " + preferredLanguage);
        this.Localizer.SelectLanguage(preferredLanguage);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(preferredLanguage);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(preferredLanguage);

        this.Logger.Debug("OnViewLoaded language loaded");

        // Create all statics views and bind them 
        this.SetupWorkflow();
        this.Logger.Debug("OnViewLoaded SetupWorkflow complete");

        // Ready 
        this.toaster.Host = this.View.ToasterHost;
        if (true)
        {
            this.toaster.Show(
                this.Localize("Shell.Ready"), this.Localize("Shell.Greetings"),
                1_600, InformationLevel.Info);
        }

        // Delay a bit the launch of the gallery so that there is time to ping 
        // this.Logger.Debug("OnViewLoaded: Internet connected: " + this.astroPicModel.IsInternetConnected);
        Schedule.OnUiThread(100, this.ActivateInitialView, DispatcherPriority.Background);

        this.Logger.Debug("OnViewLoaded complete");
    }

    private void ActivateInitialView()
    {
        // this.isFirstActivation = true;

        if (this.paletteDesignerModel.IsFirstRun)
        {
            // Select(ActivatedView.Language);
        }

        Select(ActivatedView.Design);
        this.MainToolbarIsVisible = true; 
        this.Logger.Debug("OnViewLoaded OnViewActivation complete");
    }

    private void SetupWorkflow()
    {
        if (this.View is not ShellView view)
        {
            throw new Exception("No view: Failed to startup...");
        }

        var selectableViews = new List<SelectableView<ActivatedView>>();

        // SOON 
        //void Setup<TViewModel, TControl, TToolbarViewModel, TToolbarControl>(
        //        ActivatedView activatedView, Control control)
        //    where TViewModel : ViewModel<TControl>
        //    where TControl : Control, IView, new()
        //    where TToolbarViewModel : ViewModel<TToolbarControl>
        //    where TToolbarControl : Control, IView, new()
        //{
        //    var vm = App.GetRequiredService<TViewModel>();
        //    vm.CreateViewAndBind();
        //    var vmToolbar = App.GetRequiredService<TToolbarViewModel>();
        //    vmToolbar.CreateViewAndBind();
        //    selectableViews.Add(
        //        new SelectableView<ActivatedView>(activatedView, vm, control, vmToolbar));
        //}

        void SetupNoToolbar<TViewModel, TControl>(
                ActivatedView activatedView, Control control)
            where TViewModel : ViewModel<TControl>
            where TControl : Control, IView, new()
        {
            var vm = App.GetRequiredService<TViewModel>();
            vm.CreateViewAndBind();
            selectableViews.Add(
                new SelectableView<ActivatedView>(activatedView, vm, control));
        }

        SetupNoToolbar<DesignViewModel, DesignView>(
            ActivatedView.Design, view.DesignButton);
        SetupNoToolbar<PalettePreviewViewModel, PalettePreviewView>(
            ActivatedView.Settings, view.SettingsButton);
        SetupNoToolbar<ColorWheelViewModel, ColorWheelView>(
            ActivatedView.Language, view.FlagButton);

        //Setup<CollectionViewModel, CollectionView, CollectionToolbarViewModel, CollectionToolbarView>(
        //    ActivatedView.Collection, view.CollectionButton);

        //Setup<IntroViewModel, IntroView, IntroToolbarViewModel, IntroToolbarView>(
        //    ActivatedView.Intro, view.IntroButton);

        //Setup<LanguageViewModel, LanguageView, LanguageToolbarViewModel, LanguageToolbarView>(
        //    ActivatedView.Language, view.FlagButton);

        //Setup<SettingsViewModel, SettingsView, SettingsToolbarViewModel, SettingsToolbarView>(
        //    ActivatedView.Settings, view.SettingsButton);

        // Needs to be kept alive as a class member, or else callbacks will die (and wont work) 
        this.viewSelector =
            new ViewSelector<ActivatedView>(
                this.Messenger,
                this.View.ShellViewContent,
                this.View.ShellViewToolbar,
                this.View.SelectionGroup,
                selectableViews,
                this.OnViewSelected);
    }

    private void OnViewSelected(ActivatedView activatedView)
    {
        if (this.viewSelector is null)
        {
            throw new Exception("No view selector");
        }

        var newViewModel = this.viewSelector.CurrentPrimaryViewModel;
        //if (newViewModel is not null)
        //{
        //    bool mainToolbarIsHidden =
        //        this.astroPicModel.IsFirstRun || newViewModel is IntroViewModel;
        //    this.MainToolbarIsVisible = !mainToolbarIsHidden;
        //    if (this.isFirstActivation)
        //    {
        //        this.Profiler.MemorySnapshot(newViewModel.ViewBase!.GetType().Name + ":  Activated");
        //    }
        //}

        // this.isFirstActivation = false;
    }

#pragma warning disable CA1822 // Mark members as static

    [RelayCommand]
    public void OnDesign() => Select(ActivatedView.Design);

    [RelayCommand]
    public void OnSettings() => Select(ActivatedView.Settings);

    [RelayCommand]
    public void OnLanguage() => Select(ActivatedView.Language);

    [RelayCommand]
    public void OnClose() => OnExit();

    [RelayCommand]
    public void OnDebug()
    {
        this.paletteDesignerModel.Dump();
    }

    private static async void OnExit()
    {
        var application = App.GetRequiredService<IApplicationBase>();
        await application.Shutdown();
    }

#pragma warning restore CA1822
}
