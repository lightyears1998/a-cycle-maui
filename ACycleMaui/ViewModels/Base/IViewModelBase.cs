﻿namespace ACycleMaui.ViewModels
{
    public interface IViewModelBase : IQueryAttributable
    {
        public bool IsInitialized { get; set; }

        public bool IsBusy { get; set; }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}