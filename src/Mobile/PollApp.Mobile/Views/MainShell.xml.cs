using PollApp.Mobile.ViewModels;

namespace PollApp.Mobile;

public partial class MainShell : Shell
{
    private readonly MainShellViewModel _viewModel;

    public MainShell(MainShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
}