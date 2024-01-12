using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using protocols.Model;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using NPOI.XWPF.UserModel;
using NPOI.XWPF.Model;
using protocols.ViewModel;

namespace protocols.ViewModel
{
    

    class MainWindowVM : INotifyPropertyChanged
    {
        //private Stream dataStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("protocols.Resources.projectData.json");
        private readonly Style listStyle = null;
        private int maxFlawLevelInProtocol = 3;
        //private List<string> flawsText = new List<string>();
        private List<int> flawsLevels = new List<int>();
        private List<string> flawPhotos = new List<string>();
        
        public MainWindowVM()
        {
            /*using (StreamReader r = new StreamReader(dataStream, Encoding.Default, true))
            {
                string json = r.ReadToEnd();
                dynamic projectData = JsonConvert.DeserializeObject(json);
                var protocolWorkType = projectData.protocolDefault;
                maxFlawLevelInProtocol = protocolWorkType.maxFlawLevel;
                flawsText = protocolWorkType.flawsTopText.ToObject<List<string>>();
            }
            for (int i = 1; i < maxFlawLevelInProtocol + 1; i++)
            {
                flawsLevels.Add(i);
            }*/
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
