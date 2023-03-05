using CommunityToolkit.Mvvm.ComponentModel;

namespace ACycle.ViewModels
{
    public class ViewModelBase : ObservableObject, IViewModelBase, IDisposable
    {
        private bool _disposed = false;
        private readonly SemaphoreSlim _isBusyLock = new(1, 1);
        private bool _isBusy = false;
        private bool _isInitialized = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        { }

        public async Task IsBusyFor(Func<Task> unitOfWork)
        {
            await _isBusyLock.WaitAsync();

            try
            {
                IsBusy = true;
                await unitOfWork();
            }
            finally
            {
                IsBusy = false;
                _isBusyLock.Release();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            if (disposing)
            {
                _isBusyLock.Dispose();
            }
        }

        public virtual void OnViewAppearing()
        {
        }

        public virtual void OnViewDisappearing()
        {
        }

        public virtual void OnViewNavigatedFrom(NavigatedFromEventArgs args)
        {
        }

        public virtual void OnViewNavigatingFrom(NavigatingFromEventArgs args)
        {
        }

        public virtual void OnViewNavigatedTo(NavigatedToEventArgs args)
        {
        }
    }
}
