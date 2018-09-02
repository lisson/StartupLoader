using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ReactiveUI;
using System.Configuration;
using System.Collections.Specialized;
using NLog;

namespace StartupLoader.Models
{
    class Loader : ReactiveObject
    {
        private AppSettingsCollection _apps;
        private ObservableCollection<ApplicationStatus> _completed;
        private Queue processes_queue;
        private Logger logger;
        private RegistryManager RegMan;

        public AppSettingsCollection ApplicationCollection
        {
            get { return _apps;  }
        }

        public ObservableCollection<ApplicationStatus> Completed
        {
            get { return _completed; }
            set { this.RaiseAndSetIfChanged(ref _completed, value); }
        }

        public Loader()
        {
            // Load config
            AppSettingsSection section = (AppSettingsSection)ConfigurationManager.GetSection("Section");
            _apps = (AppSettingsCollection)section.Applications;
            processes_queue = new Queue();
            _completed = new ObservableCollection<ApplicationStatus>();
            logger = NLog.LogManager.GetCurrentClassLogger();
            RegMan = new RegistryManager(@"SOFTWARE");
            logger.Debug("Loader init complete.");
        }

        public void load_programs()
        {
            logger.Debug("List of applications to be loaded:");
            foreach (AppSetting s in _apps)
            {
                // I can't seem to remove items from ConfigurationElementCollection
                // So I pushed them all to a queue first
                processes_queue.Enqueue(s);
                logger.Debug(s.path + " " + s.arguments);
            }
            load_p();
        }

        private void Cleanup()
        {
            RegMan.Cleanup();
        }

        private void load_p()
        {
            if (processes_queue.Count > 0)
            {
                ProcessStartInfo p = new ProcessStartInfo();
                AppSetting s = (AppSetting)processes_queue.Dequeue();
                ApplicationStatus ps1 = new ApplicationStatus(s.label, s.path);
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    _completed.Add(ps1);
                });
                p.FileName = s.path;
                p.Arguments = s.arguments;
                Process process = new Process();
                process.StartInfo = p;
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) =>
                {
                    if (process.ExitCode == 0)
                    {
                        Console.Out.WriteLine("Completed " + s.path);
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ApplicationStatus temp = _completed.FirstOrDefault(x => x.Name == ps1.Name);
                            if (temp != null)
                            {
                                temp.State = state.SUCCESS;
                                RegMan.WriteValue(temp.Label, "DONE");
                            }
                        });
                        logger.Debug(s.path + " " + s.arguments + " completed.");
                        load_p();
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ps1.State = state.FAILED;
                            RegMan.WriteValue(ps1.Label, "DONE");
                        });
                        logger.Debug(s.path + " " + s.arguments + " exited with status " + process.ExitCode);
                        Cleanup();
                    }
                };
                logger.Debug("Starting " + s.path + " " + s.arguments);
                ps1.State = state.RUNNING;
                process.Start();
                RegMan.WriteValue(s.label, "RUNNING");
            }
            else
            {
                logger.Debug("StartupLoader finished loading all applications.");
                Cleanup();
            }
        }
    }
}
