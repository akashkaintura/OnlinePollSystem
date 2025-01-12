using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PollApp.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Command _loadDataCommand;
        public Command LoadDataCommand => _loadDataCommand ??= 
            new Command(async () => await LoadData());

        protected virtual async Task LoadData()
        {
            try 
            {
                IsBusy = true;
                // Implement data loading logic in derived classes
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

        protected virtual async Task HandleError(Exception ex)
        {
            // Centralized error handling
            await Shell.Current.DisplayAlert(
                "Error", 
                ex.Message, 
                "OK"
            );
        }
    }
}