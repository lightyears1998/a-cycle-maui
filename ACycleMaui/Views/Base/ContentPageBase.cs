using ACycleMaui.ViewModels;

namespace ACycleMaui.Views
{
    public class ContentPageBase : ContentPage
    {
        protected virtual async void OnApprearing()
        {
            base.OnAppearing();

            if (BindingContext is IViewModelBase model && !model.IsInitialized)
            {
                model.IsInitialized = true;
                await model.InitializeAsync();
            }
        }
    }
}
