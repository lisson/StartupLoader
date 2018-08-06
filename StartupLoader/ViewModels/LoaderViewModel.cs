using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using ReactiveUI;
using StartupLoader.Models;
using System.Collections;

namespace StartupLoader.ViewModels
{
    class LoaderViewModel: ReactiveObject
    {
        private Loader loader;
        private ObservableCollection<ApplicationStatus> _apps;
        
        public LoaderViewModel(Loader _loader)
        {
            loader = _loader;
            _apps = _loader.Completed;
            loader.load_programs();
        }

        public ObservableCollection<ApplicationStatus> Apps
        {
            get { return _apps; }
            set { this.RaiseAndSetIfChanged(ref _apps, value); }
        }

        public AppSettingsCollection appCollection
        {
            get { return loader.ApplicationCollection;  }
        }
    }
}
