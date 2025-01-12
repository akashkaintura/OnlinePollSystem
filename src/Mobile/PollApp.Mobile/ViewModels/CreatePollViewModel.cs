using System.Windows.Input;
using PollApp.Mobile.Models;
using PollApp.Mobile.Services.Interfaces;

namespace PollApp.Mobile.ViewModels
{
    public class CreatePollViewModel : BaseViewModel
    {
        private readonly IPollService _pollService;
        
        private string _title;
        private string _description;
        private List<string> _options = new();
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now.AddDays(7);

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public List<string> Options
        {
            get => _options;
            set => SetProperty(ref _options, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public ICommand CreatePollCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand RemoveOptionCommand { get; }

        public CreatePollViewModel(IPollService pollService)
        {
            _pollService = pollService;
            Title = "Create New Poll";

            CreatePollCommand = new Command(async () => await CreatePoll());
            AddOptionCommand = new Command(AddOption);
            RemoveOptionCommand = new Command<string>(RemoveOption);

            // Start with two default options
            Options.Add("");
            Options.Add("");
        }

        private void AddOption()
        {
            if (Options.Count < 10)
            {
                Options.Add("");
            }
        }

        private void RemoveOption(string option)
        {
            if (Options.Count > 2)
            {
                Options.Remove(option);
            }
        }

        private async Task CreatePoll()
        {
            if (IsBusy) return;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(Title))
            {
                await HandleError(new Exception("Poll title is required"));
                return;
            }

            var validOptions = Options.Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
            if (validOptions.Count < 2)
            {
                await HandleError(new Exception("At least two options are required"));
                return;
            }

            try
            {
                IsBusy = true;

                var newPoll = new Poll
                {
                    Title = Title,
                    Description = Description,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    IsActive = true,
                    Options = validOptions.Select(o => new PollOption 
                    { 
                        Text = o 
                    }).ToList()
                };

                var createdPoll = await _pollService.CreatePollAsync(newPoll);
                
                // Navigate back or to poll details
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await HandleError(ex);
            }
            finally
            {
                IsBusy = false;