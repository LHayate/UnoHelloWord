using System;
using System.Collections.Generic;
using System.Text;
using UnoHelloWord.Shared.Services.Navigation;

namespace UnoHelloWord.Shared.Helpers
{
    public class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService _navigationService;
        protected readonly IDialogService _dialogService;
        protected readonly NetworkAccess _networkAccess;
        protected readonly ISettingsService _settingsService;
        private INotificationsService _notificationsService;

        public bool isBusy = false;

        protected bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string title = string.Empty;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        protected BaseViewModel()
        {
            _navigationService = ViewModelLocator.Resolve<INavigationService>();
            _dialogService = ViewModelLocator.Resolve<IDialogService>();
            _networkAccess = Connectivity.NetworkAccess;
            _settingsService = ViewModelLocator.Resolve<ISettingsService>();
            _notificationsService = ViewModelLocator.Resolve<INotificationsService>();

            GlobalSetting.Instance.BaseIdentityEndpoint = _settingsService.IdentityEndpointBase;
            //GlobalSetting.Instance.BaseGatewayShoppingEndpoint = _settingsService.GatewayShoppingEndpointBase;
            //GlobalSetting.Instance.BaseGatewayMarketingEndpoint = _settingsService.GatewayMarketingEndpointBase;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public ICommand ReturnCommand => new Command(() => Return());

        private void Return()
        {
            _navigationService.ReturnModalToAsync(true);
        }

        public void SendNotifications(string title, string name, string body, string customData, string userIdProvider,
            string useridReceiver)
        {
            PushNotification push = new PushNotification()
            {
                Name = name,
                Body = body,
                CustomData = customData,
                Title = title,
                UserIdProvider = userIdProvider,
                UserIdReceiver = useridReceiver,
            };

            _notificationsService.Send(push);
        }
    }
}