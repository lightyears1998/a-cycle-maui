namespace ACycle.ViewModels
{
    public interface IViewModelBase : IQueryAttributable
    {
        bool IsInitialized { get; set; }

        bool IsBusy { get; set; }

        Task InitializeAsync();

        void OnViewAppearing();

        void OnViewDisappearing();

        void OnViewNavigatedFrom(NavigatedFromEventArgs args);

        void OnViewNavigatingFrom(NavigatingFromEventArgs args);
    }
}
