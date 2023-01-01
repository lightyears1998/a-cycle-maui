﻿using ACycle.Services;

namespace ACycleMaui.Services
{
    public interface INavigationService : IService
    {
        public Task NavigateToAsync(string route);
    }
}
