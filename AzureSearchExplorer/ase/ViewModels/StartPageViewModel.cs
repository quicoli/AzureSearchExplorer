using ase.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.ViewModels
{
    public class StartPageViewModel: ViewModelBase
    {
        private ISearchServiceManager _searchServiceManager;
        private INavigationService _navigationService;


        public RelayCommand CmdLoadData { get; set; }
        public RelayCommand CmdRegisterService { get; set; }

        private void DoRegisterService()
        {
            _navigationService.NavigateTo(PageConst.RegisterSearchServicePage);
        }

        

        private async Task DoLoadDataAsync()
        {
            IsBusy = true;
            await LoadDataTask();
            RaisePropertyChanged(nameof(Services));
            IsBusy = false;
        }

        private async Task LoadDataTask()
        {
            await Task.Run(() =>
            {
                var services = _searchServiceManager.GetAllItems();
                _services = new ObservableCollection<SearchService>(services);
            });
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
                RaisePropertyChanged(nameof(IsBusy));
                CmdRegisterService.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SearchService> _services;

        public ObservableCollection<SearchService> Services
        {
            get { return _services; }
            set { _services = value; RaisePropertyChanged("Services");}
        }

        private SearchService _selectedService;

        public SearchService SelectedService
        {
            get { return _selectedService; }
            set { _selectedService = value; RaisePropertyChanged(nameof(SelectedService));}
        }


        public StartPageViewModel(ISearchServiceManager searchServiceManager, INavigationService navigationService)
        {
            _searchServiceManager = searchServiceManager;
            _navigationService = navigationService;

            Services = new ObservableCollection<SearchService>();
            CmdLoadData = new RelayCommand(async () => await DoLoadDataAsync());
            CmdRegisterService = new RelayCommand(DoRegisterService, () => !IsBusy);
        }






    }
}
