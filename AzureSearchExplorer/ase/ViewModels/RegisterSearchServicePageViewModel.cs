using ase.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Azure.Search;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.ViewModels
{
    public class RegisterSearchServicePageViewModel: BaseValidation 
    {
        private ISearchServiceManager _searchServiceManager;
        private INavigationService _navigationService;

        public RelayCommand CmdBack { get; set; }
        public RelayCommand CmdRegister { get; set; }
        public RelayCommand<string> CmdSetTipFor { get; set; }

        private void DoSetTipFor(string propertyName)
        {
            switch (propertyName)
            {
                case "ServiceName":
                    InputTip = "Service Name is the name of your service, for example, in the following " +
                               "address: http://acme.search.windows.net the service is acme only.";
                    break;
                case "ApiKey":
                    InputTip = "Your Admin API Key. You can find this on your Azure portal.";
                    break;
                case "ServiceAlias":
                    InputTip = "An friendly name for your service. This alias is used as identifier for you here.";
                    break;
                default:
                    break;
            }
        }

        private void DoBack()
        {
            _navigationService.GoBack();
        }

        private async Task DoSaveDataAsync()
        {
            BusyMessage = "Connecting to the informed service...";
            IsBusy = true;
            Validate();
            if (!IsValid) return;

            var response = new BoolResponse();
            response = await TryConnectingTask(ServiceName, ApiKey);
            if (response.Value)
            {
                BusyMessage = "Saving data...";
                response = await SaveDataTask();
            }
            BusyMessage = string.Empty;
            IsBusy = false;
            if (response.Value)
            {
                DoBack();
            }
            else
            {
               var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
               await dialog.ShowMessage(response.Response, "Attention");
            }
        }

        private async Task<BoolResponse> TryConnectingTask(string searchServiceName, string adminApiKey)
        {
            var result = await Task.Run(()=>
            {
                var response = new BoolResponse();
                try
                {
                    SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
                    var indexesNames = serviceClient.Indexes.ListNames();
                    response.Value = true;
                    response.Response = "Credentials are valid";
                }
                catch (CloudException e)
                {
                    response.Value = false;
                    response.Response = e.Message;
                }
                catch (Exception e)
                {
                    response.Value = false;
                    response.Response = e.Message;
                }
                return response;
            });
            return result;
        }

        private async Task<BoolResponse> SaveDataTask()
        {
           var result = await Task.Run(() =>
            {
                var s = new SearchService();
                s.Name = ServiceName;
                s.Alias = ServiceAlias;
                s.ApiKey = ApiKey;

                return _searchServiceManager.SaveValue(s);
            });

            return result;
        }


        private string _inputTip;

        public string InputTip
        {
            get { return _inputTip; }
            set { _inputTip = value; RaisePropertyChanged(); }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        private string _serviceName;

        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; RaisePropertyChanged(); }
        }

        private string _apiKey;

        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; RaisePropertyChanged(); }
        }

        private string _serviceAlias;

        public string ServiceAlias
        {
            get { return _serviceAlias; }
            set { _serviceAlias = value; RaisePropertyChanged(); }
        }


        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { _busyMessage = value; RaisePropertyChanged(); }
        }


        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                ValidationErrors[nameof(ServiceName)] = "required";
            }

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                ValidationErrors[nameof(ApiKey)] = "required";
            }

            if (string.IsNullOrWhiteSpace(ServiceAlias))
            {
                ValidationErrors[nameof(ServiceAlias)] = "required";
            }
        }

        private void ClearData()
        {
            ServiceName = string.Empty;
            ApiKey = string.Empty;
            ServiceAlias = string.Empty;
        }

        public RegisterSearchServicePageViewModel(ISearchServiceManager searchServiceManager, INavigationService navigationService)
        {
            _searchServiceManager = searchServiceManager;
            _navigationService = navigationService;
            CmdBack = new RelayCommand(DoBack);
            CmdRegister = new RelayCommand(async () => await DoSaveDataAsync(), () => !IsBusy && IsValid);
            CmdSetTipFor = new RelayCommand<string>(DoSetTipFor);

            ObserverCommands.Add(CmdRegister);            
        }
       
    }
}
