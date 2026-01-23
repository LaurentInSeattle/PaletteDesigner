namespace Lyt.Avalonia.PaletteDesigner.Shell;

using static ApplicationMessagingExtensions;
using static ViewActivationMessage;

public sealed partial class ShellViewModel : ViewModel<ShellView>, IRecipient<LanguageChangedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    public bool mainToolbarIsVisible;

    [ObservableProperty]
    public bool dumpIsVisible;

    private ViewSelector<ActivatedView>? viewSelector;

    #region To please the XAML viewer 

#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor.
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
        this.Subscribe<LanguageChangedMessage>();
    }

    // Language agnostic (for now) 
    public void Receive(LanguageChangedMessage message) {}

    public override void OnViewLoaded()
    {
        this.Logger.Debug("OnViewLoaded begins");

        base.OnViewLoaded();
        if (this.View is null)
        {
            throw new Exception("Failed to startup...");
        }

        // Select default language 
        string preferredLanguage = this.paletteDesignerModel.Language;
        this.Logger.Debug("Language: " + preferredLanguage);
        this.Localizer.SelectLanguage(preferredLanguage);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(preferredLanguage);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(preferredLanguage);

        this.Logger.Debug("OnViewLoaded language loaded");

        // Create all persistent views and bind them 
        this.SetupWorkflow();
        this.Logger.Debug("OnViewLoaded SetupWorkflow complete");

        // Debug: dump button visibility
        this.DumpIsVisible = false;
        // this.DumpIsVisible = Debugger.IsAttached;

        // Ready 
        this.toaster.Host = this.View.ToasterHost;
        if (true)
        {
            this.toaster.Show(
                this.Localize("Shell.Ready"), this.Localize("Shell.Greetings"),
                1_600, InformationLevel.Info);
        }

        Schedule.OnUiThread(100, this.ActivateInitialView, DispatcherPriority.Background);

        this.Logger.Debug("OnViewLoaded complete");
    }

    private void ActivateInitialView()
    {
        if (this.paletteDesignerModel.IsFirstRun)
        {
            Select(ActivatedView.Language);

            // Save that we have run once as "fire and forget"
            this.paletteDesignerModel.IsFirstRun = false;
            _ = this.paletteDesignerModel.Save(); 
        }
        else
        {
            Select(ActivatedView.Design);
        }

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

        // Maybe... in future we want toolbars for some views 
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

        SetupNoToolbar<DesignViewModel, DesignView>(ActivatedView.Design, view.DesignButton);
        SetupNoToolbar<ImagingViewModel, ImagingView>(ActivatedView.Imaging, view.ImagingButton);
        SetupNoToolbar<WizardViewModel, WizardView>(ActivatedView.Wizard, view.WizardButton);
        SetupNoToolbar<LanguageViewModel, LanguageView>(ActivatedView.Language, view.FlagButton);

        // Color mappings and Settings disabled for now
        // SetupNoToolbar<MappingViewModel, MappingView>(ActivatedView.Mapping, view.MappingButton);
        // SetupNoToolbar<SettingsViewModel, SettingsView>(ActivatedView.Settings, view.SettingsButton);

        // Needs to be kept alive as a class member, or else callbacks will die (and wont work) 
        this.viewSelector =
            new ViewSelector<ActivatedView>(
                this.View.ShellViewContent,
                this.View.ShellViewToolbar,
                this.View.SelectionGroup,
                selectableViews,
                this.OnViewSelected);
    }

    private void OnViewSelected(ActivatedView activatedView)
    {
        // Nothing for now 

        if (this.viewSelector is null)
        {
            throw new Exception("No view selector");
        }
    }

#pragma warning disable CA1822 
    // Mark members as static
    // Cannot be static because of [RelayCommand]

    [RelayCommand]
    public void OnDesign() => Select(ActivatedView.Design);

    [RelayCommand]
    public void OnImaging() => Select(ActivatedView.Imaging);

    [RelayCommand]
    public void OnWizard() => Select(ActivatedView.Wizard);

    [RelayCommand]
    public void OnLanguage() => Select(ActivatedView.Language);

    [RelayCommand]
    public void OnDebug() => this.paletteDesignerModel.Dump();

    [RelayCommand]
    public void OnClose() => OnExit();

    // Color mappings and Settings disabled for now
    //[RelayCommand]
    //public void OnMapping() => Select(ActivatedView.Mapping);

    //[RelayCommand]
    //public void OnSettings() => Select(ActivatedView.Settings);

    private static async void OnExit()
    {
        var application = App.GetRequiredService<IApplicationBase>();
        await application.Shutdown();
    }

#pragma warning restore CA1822

    /// <summary> Invoked when closing from the application Close X button </summary>
    /// <returns> True to close immediately </returns>
    public async Task<bool> CanCloseAsync()
    {
        await this.paletteDesignerModel.Save();
        return true;
    }
}
