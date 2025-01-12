using System.Collections.ObjectModel;
using System.Windows.Input;
using PollApp.Mobile.Models;
using PollApp.Mobile.Services.Interfaces;

namespace PollApp.Mobile.ViewModels
{
    public class PollListViewModel : BaseViewModel
    {
        private readonly IPollService _pollService;
        private ObservableCollection<Poll> _polls;
        private Poll _selectedPoll;

        public ObservableCollection<Poll> Polls
        {
            get => _polls;
            set => SetProperty(ref _polls, value);
        }

        public Poll SelectedPoll
        {
            get => _selectedPoll;
            set => SetProperty(ref _selectedPoll, value);
        }

        public ICommand LoadPollsCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateToPollDetailCommand { get; }

        public PollListViewModel(IPollService pollService)
        {
            _pollService = pollService;
            Title = "Active Polls";

            LoadPollsCommand = new Command(async () => await LoadPolls());
            RefreshCommand = new Command(async () => await RefreshPolls());
            NavigateToPollDetailCommand = new Command<Poll>(async (poll) => await NavigateToPollDetail(poll));

            // Automatically load polls when view model is created
            LoadPollsCommand.Execute(null);
        }

        private async Task LoadPolls()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var polls = await _pollService.GetActivePollsAsync();
                Polls = new ObservableCollection<Poll>(polls);
            }
            catch (Exception ex)
            {
                await HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshPolls()
        {
            Polls.Clear();
            await LoadPolls();
        }

        private async Task NavigateToPollDetail(Poll poll)
        {
            if (poll == null) return;

            var route = $"//polldetail?pollId={poll.Id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}