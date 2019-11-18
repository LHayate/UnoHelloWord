using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnoHelloWord.Shared.Helpers;

namespace UnoHelloWord.Shared.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task ReturnToAsync(bool animation);
        Task NavigateToAsync<TViewModel>(object[] parameter) where TViewModel : BaseViewModel;

        Task NavigateModalToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task ReturnModalToAsync(bool animation);
        Task NavigateModalToAsync<TViewModel>(object[] parameter) where TViewModel : BaseViewModel;
        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();
    }
}