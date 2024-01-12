using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using protocols.ViewModel;
using protocols.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using protocols;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace protocols.View
{
    public partial class FlawV : UserControl, INotifyPropertyChanged
    {
        public FlawV()
        {
            InitializeComponent();
        }

        private void Flaw_OnMouseMove(object sender, MouseEventArgs e)
        {
            
            Grid currentFlaw = sender as Grid;
            if (currentFlaw == null)
                return;
            if ((e.Source as ComboBox) != null)
                return;
            if ((e.Source as TextBox) != null)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                base.OnMouseMove(e);
                var dragTarget = (e.OriginalSource as FrameworkElement)?.DataContext as FlawM;
                if (dragTarget != null)
                {
                    int currentFlawIndex = dragTarget.FlawNumber - 1;
                    DataObject data = new DataObject();
                    data.SetData("Int", currentFlawIndex);
                    data.SetData(dragTarget);
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Wybierz zdjęcia do usterki";
            openFileDialog.Filter = "Zdjęcia (*.png; *.jpeg; *jpg)| *.png; *.jpeg; *.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                //flawsPaths = new List<string>();
                string photoFilename = null;
                string pathTo = null;
                foreach (string photoPath in openFileDialog.FileNames)
                {
                    photoFilename = System.IO.Path.GetFileName(photoPath);
                    pathTo = System.IO.Path.Combine(@"D:\PastePhotos\photo_test\to", photoFilename);
                    //FlawsPaths.Add(pathTo);
                    File.Move(photoPath, pathTo);
                }
            }
            
        }

        private void PhotoViewer_Click(object sender, RoutedEventArgs e)
        {
        //Debug.WriteLine(FlawsPaths.Count);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
