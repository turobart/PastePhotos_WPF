using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using protocols.View;
using protocols.ViewModel;
using protocols.Model;

namespace protocols.ViewModel
{
    class MainViewModel
    {
        public MainWindowVM MainWindowVM { get; set; }
        public FlawVM FlawVM { get; set; }
    }
}
