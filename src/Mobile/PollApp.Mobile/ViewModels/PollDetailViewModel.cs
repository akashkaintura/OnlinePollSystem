using System.Collections.ObjectModel;
using System.Windows.Input;
using PollApp.Mobile.Models;
using PollApp.Mobile.Services.Interfaces;

namespace PollApp.Mobile.ViewModels
{
    [QueryProperty(nameof(PollId), "pollId")]
    public class PollDetailViewModel : BaseViewModel
    {
        private readonly IPollService _pollService;
        private Poll _poll;
        private PollOption _selectedOption;
        private VoteResult _voteResult;

        public Poll Poll
        {
            get => _poll;
            set => SetProperty(ref _poll, value);
        }

        public PollOption SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }

        public VoteResult VoteResult
        {
            get => _voteResult;
            set => SetProperty(ref _voteResult, value);
        }

        public int PollId { get; set; }

        public ICommand LoadPollCommand { get; }
        public ICommand VoteCommand { get; }

        public PollDetailViewModel(IPollService pollService)
        {
            _pollService = pollService;
            LoadPollCommand = new Command(async () => await LoadPoll());
            VoteCommand = new Command(async () => await SubmitVote());
        }

        private async Task LoadPoll()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Poll = await _pollService.GetPollByIdAsync(PollId);
                Title = Poll.Title;
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

        private async Task SubmitVote()
        {
            if (SelectedOption == null) return;

            try
            {
                IsBusy = true;
                VoteResult = await _pollService.VoteOnPollAsync(Poll.Id, SelectedOption.Id);
                
                // Refresh poll after voting
                await LoadPoll();
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
    }
}