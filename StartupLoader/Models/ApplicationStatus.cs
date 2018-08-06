using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupLoader.Models
{
    public enum state { WAIT, SUCCESS, RUNNING, FAILED }

    class ApplicationStatus : ReactiveObject
    {
        private String _label;
        private String _name;
        private state _State;

        public ApplicationStatus(String l, String n)
        {
            _label = l;
            _name = n;
            _State = state.WAIT;
        }

        public String Label
        {
            get { return _label; }
            set { this.RaiseAndSetIfChanged(ref _label, value); }
        }

        public String Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public state State
        {
            get { return _State; }
            set { this.RaiseAndSetIfChanged(ref _State, value); }
        }
    }
}
