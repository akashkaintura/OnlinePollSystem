using PollApp.Mobile.Models;
using PollApp.Mobile.ViewModels;

namespace PollApp.Mobile.Views;

public partial class PollListPage : ContentPage
{
    private readonly PollListViewModel _viewModel;

    public PollListPage(PollListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private async void OnPollSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedPoll = e.CurrentSelection.FirstOrDefault() as Poll;
        if (selectedPoll != null)
        {
            await _viewModel.NavigateToPollDetailCommand.ExecuteAsync(selectedPoll);
        }
    }

    private async void OnCreatePollClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//create-poll");
    }
}