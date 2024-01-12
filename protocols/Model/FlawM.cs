using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.IO.Ports;
using System.Windows.Input;
using System.Diagnostics;


namespace protocols.Model
{
    public class FlawM : INotifyPropertyChanged
    {
        private int _flawNumber;
        private string _flawText;
        private int _flawLevel;
        private List<string> _flawsPaths = new List<string>();
        private bool _isTextBold;
        private bool _noPhotos;
        private List<int> _comboLevelList = new List<int>();

        public int FlawNumber
        {
            get { return _flawNumber; }
            set
            {
                _flawNumber = value;
                OnPropertyChanged();
            }
        }
        public string FlawText
        {
            get { return _flawText; }
            set
            {
                _flawText = value;
                OnPropertyChanged();
            }
        }
        public int FlawLevel
        {
            get { return _flawLevel; }
            set
            {
                _flawLevel = value;
                OnPropertyChanged();
            }
        }
        public List<string> FlawsPaths
        {
            get { return _flawsPaths; }
            set
            {
                _flawsPaths = value;
                OnPropertyChanged();
            }
        }
        public bool IsBold
        {
            get { return _isTextBold; }
            set
            {
                _isTextBold = value;
                OnPropertyChanged();
            }
        }
        public bool NoPhotos
        {
            get { return _noPhotos; }
            set
            {
                _noPhotos = value;
                OnPropertyChanged();
            }
        }
        public List<int> ComboLevelList
        {
            get { return _comboLevelList; }
            set
            {
                _comboLevelList = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                Debug.WriteLine("propertyChanged");
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        
    }

}