﻿using ACycle.ViewModels;

namespace ACycle.Views
{
    public class ContentPageBase : ContentPage
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IViewModelBase model && !model.IsInitialized)
            {
                if (!model.IsInitialized)
                {
                    model.IsInitialized = true;
                    await model.InitializeAsync();
                }

                model.OnViewAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is IViewModelBase model)
            {
                model.OnViewDisappearing();
            }
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);

            if (BindingContext is IViewModelBase model)
            {
                model.OnViewNavigatedFrom(args);
            }
        }

        protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
        {
            base.OnNavigatingFrom(args);

            if (BindingContext is IViewModelBase model)
            {
                model.OnViewNavigatingFrom(args);
            }
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (BindingContext is IViewModelBase model)
            {
                model.OnViewNavigatedTo(args);
            }
        }
    }
}
