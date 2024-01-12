using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using protocols.Model;

namespace protocols.ViewModel
{
    public class FlawVM : INotifyPropertyChanged
    {
        public ObservableCollection<FlawM> _flaws = new ObservableCollection<FlawM>();

        public ObservableCollection<FlawM> Flaws
        {
            get { return _flaws; }
            set 
            { 
                _flaws = value;
                OnPropertyChanged();
            }
        }

        public FlawVM()
        {
 
        }

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
