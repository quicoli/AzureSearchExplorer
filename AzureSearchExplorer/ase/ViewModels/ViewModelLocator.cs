using ase.Core;
using ase.Design;
using ase.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(PageConst.RegisterSearchServicePage, typeof(RegisterSearchServicePage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            var dlg = new DialogService();
            SimpleIoc.Default.Register<IDialogService>(() => dlg);


            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ISearchServiceManager, SearchServiceDesignManager>();
            }
            else
            {
                var ss = new SearchServiceManager(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite"), new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT());
                SimpleIoc.Default.Register<ISearchServiceManager>(()=>ss);
            }
            
            SimpleIoc.Default.Register<StartPageViewModel>();
            SimpleIoc.Default.Register<RegisterSearchServicePageViewModel>();

        }


        /// <summary>
        /// Gets the StartPage view model.
        /// </summary>
        /// <value>
        /// The StartPage view model.
        /// </value>
        public StartPageViewModel StartPageInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StartPageViewModel>();
            }
        }

        /// <summary>
        /// Gets the RegisterSearchServicePage view model.
        /// </summary>
        public RegisterSearchServicePageViewModel RegisterSearchServicePageInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterSearchServicePageViewModel>();
            }
        }

        /// <summary>
        /// The cleanup.
        /// </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        
    }
   
}
