using ACycleMaui.ViewModels;

namespace ACycleMaui.Views
{
    public class ContentPageBase : ContentPage
    {
        protected override async void OnAppearing()
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
