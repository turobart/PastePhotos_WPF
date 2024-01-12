using System.Windows;
using System.Windows.Input;
using NPOI.XWPF.UserModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using protocols.View;
using protocols.ViewModel;
using protocols.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;
using System.Reflection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.CSharp;
using System.Xml;
using System.Xml.Serialization;

namespace protocols
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Style listStyle = null;
        private int maxFlawLevelInProtocol = 3;
        private List<string> flawPhotos = new List<string>();
        private List<int> flawsLevels = new List<int>();
        private Stream dataStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("protocols.Resources.projectData.json");
        private List<string> flawsText = new List<string>();
        public ObservableCollection<FlawM> Flaws { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Flaws = new ObservableCollection<FlawM>(new List<FlawM> { });
            
            this.DataContext = this;

            using (StreamReader r = new StreamReader(dataStream, Encoding.Default, true))
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
            }

            listStyle = new Style(typeof(ListBoxItem));
            flawsPanel.ItemContainerStyle = listStyle;
        }
        private void CreateNewTest_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        /*private void SortTest()
        {
            Flaws = new ObservableCollection<FlawV>(Flaws.OrderBy(i => i.FlawLevel));
            //var sortedQuery = Flaws.OrderBy(x => x.MyProperty);
            Debug.WriteLine(Flaws);
        }*/
        private void CreateNewTest_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("protocols.g.resources.pattern.docx");
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("protocols.Resources.pattern.docx");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()[0];

            //List<string> flawPhotos = new List<string>();

            int numberOfPhotos = 0;

            //SortTest();

            Debug.WriteLine(stream);
            
            XWPFDocument doc = new XWPFDocument(stream);

            XWPFNumbering numbering = doc.CreateNumbering();
            string abstractNumId = numbering.AddAbstractNum();
            string numId = numbering.AddNum(abstractNumId);

            XWPFParagraph p0 = doc.CreateParagraph();
            XWPFRun r00 = p0.CreateRun();
            r00.SetText("Zalecenia");
            r00.IsBold = true;
            r00.FontFamily = "Times New Roman";
            r00.FontSize = 12;
            p0.SpacingAfter = 300;

            foreach(int flawLevel in flawsLevels)
            {
                WriteFlawLevelList(doc, flawLevel, numId);
            }

            XWPFParagraph pageBreakPara = doc.CreateParagraph();
            XWPFRun runPageBreak = pageBreakPara.CreateRun();
            runPageBreak.AddBreak(BreakType.PAGE);

            //Debug.WriteLine(doc.CreateHeaderFooterPolicy().GetType());
            //NPOI.XWPF.Model.XWPFHeaderFooterPolicy headerFooterPolicy = doc.GetHeaderFooterPolicy();
            //if (headerFooterPolicy == null) headerFooterPolicy = doc.CreateHeaderFooterPolicy();

            ////// create header start
            //XWPFHeader headerr = headerFooterPolicy.CreateHeader(NPOI.XWPF.Model.XWPFHeaderFooterPolicy.FIRST);
            //XWPFHeader header = headerFooterPolicy.CreateHeader(NPOI.XWPF.Model.XWPFHeaderFooterPolicy.DEFAULT);

            //XWPFParagraph paragraph = header.CreateParagraph();
            //paragraph.Alignment = ParagraphAlignment.CENTER;

            //XWPFRun run = paragraph.CreateRun();
            //run.SetText("Header");

            //NPOI.XWPF.Model.XWPFHeaderFooterPolicy policy = doc.GetHeaderFooterPolicy();

            //XWPFHeader header = policy.GetDefaultHeader();

            //XWPFParagraph paragraph = doc.CreateParagraph();
            //paragraph.Alignment = ParagraphAlignment.CENTER;
            //XWPFRun run = paragraph.CreateRun();
            //run.SetText("Header");

            var hfPolicy = doc.CreateHeaderFooterPolicy();
            XWPFHeader header = hfPolicy.CreateHeader(XWPFHeaderFooterPolicy.DEFAULT);
            XWPFParagraph paragraph = header.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;

            XWPFRun run = paragraph.CreateRun();
            run.SetText("Header");

            numberOfPhotos = flawPhotos.Count;
            int numberOfRows = (int)Math.Ceiling((double)numberOfPhotos / 2)*2;
            //Debug.WriteLine(numberOfPhotos);


            XWPFTable photoTable = doc.CreateTable(numberOfRows, 2);
            var photoTablelayout = photoTable.GetCTTbl().tblPr.AddNewTblLayout();
            photoTablelayout.type =  NPOI.OpenXmlFormats.Wordprocessing.ST_TblLayoutType.@fixed;
            photoTable.Width = 5000;

            var widthEmus = (int)(400.0 * 9525);
            var heightEmus = (int)(300.0 * 9525);

            int photoIndex = 0;
            
            foreach (string flawPhoto in flawPhotos)
            {
                Debug.WriteLine(photoIndex);
                XWPFTableCell c1 = photoTable.GetRow(photoIndex - photoIndex % 2).GetCell(photoIndex % 2);
                XWPFParagraph p1 = c1.AddParagraph();   //don't use doc.CreateParagraph
                XWPFRun r2 = p1.CreateRun();

                using (FileStream picData = new FileStream(flawPhoto, FileMode.Open, FileAccess.Read))
                {
                    r2.AddPicture(picData, (int)PictureType.PNG, "image1", widthEmus, heightEmus);
                }

                photoIndex++;
            }

            FileStream sw = new FileStream("D:\\PastePhotos\\testDoc.docx", FileMode.Create);
            doc.Write(sw);
            sw.Close();
        }
        private void AddFlaw_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddFlaw_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int flawNumber = Flaws.Count + 1;
            FlawM tempFlaw = new FlawM { FlawNumber = flawNumber, FlawText = flawNumber.ToString(), IsBold = false, ComboLevelList = flawsLevels };
            Flaws.Add(tempFlaw);
            Debug.WriteLine(tempFlaw.FlawNumber);
            Debug.WriteLine(tempFlaw.ComboLevelList.Count);
        }
        private void WriteFlawLevelList(XWPFDocument document, int flawLevel, string numId)
        {
            Debug.WriteLine("flawsText.Count " + flawsText.Count);
            
            XWPFParagraph newLevelPara = document.CreateParagraph();
            XWPFRun newRunTitle = newLevelPara.CreateRun();
            newRunTitle.SetText(flawsText[flawLevel-1]);
            newRunTitle.IsBold = true;
            newRunTitle.FontFamily = "Times New Roman";
            newRunTitle.FontSize = 12;
            newRunTitle.AddBreak(BreakClear.NONE);
            if (flawsText.Count > 4)
            {
                XWPFRun newRunBracket = newLevelPara.CreateRun();
                newRunBracket.SetText(flawsText[flawLevel+2]);
                newRunBracket.IsBold = false;
                newRunBracket.FontFamily = "Times New Roman";
                newRunBracket.FontSize = 12;
            }

            int flawCount = 0;
            foreach (FlawM item in Flaws)
            {
                if (item.FlawLevel == flawLevel)
                {
                    flawCount++;
                    flawPhotos.AddRange(item.FlawsPaths);
                    XWPFParagraph newLevelParaList = document.CreateParagraph();
                    XWPFRun newLevelList = newLevelParaList.CreateRun();
                    newLevelList.SetText(item.FlawText);
                    newLevelList.IsBold = item.IsBold;
                    newLevelParaList.SetNumID(numId, "1");
                }
            }
            if (flawCount == 0)
            {
                XWPFParagraph newParaEmpty = document.CreateParagraph();
                XWPFRun newEmpty = newParaEmpty.CreateRun();
                newEmpty.SetText("Brak zaleceń remontowych");
            }
        }
        /*private void SaveProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }*/
        /*private void SaveProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FlawSaveList));
            TextWriter writer = new StreamWriter("test_save_test.xml");

            FlawSaveList fsl = new FlawSaveList();

            fsl.AllFlaws = Flaws.ToList();

            serializer.Serialize(writer, fsl);
            writer.Close();

            //Debug.WriteLine(Assembly.GetEntryAssembly().Location);
        }*/

        private void Flaw_DragOver(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e.Effects);
            if (!e.Data.GetDataPresent(typeof(FlawV)) && !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
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

        private void Flaw_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FlawM)))
            {
                int flawIndex = (int)e.Data.GetData("Int");
                FlawM droppedFlaw = (FlawM)e.Data.GetData(typeof(FlawM));
                Debug.WriteLine("Level: " + flawIndex);

                var droptarget = (e.OriginalSource as FrameworkElement)?.DataContext as FlawM;
                if (droptarget != null)
                {
                    int dropTargetIndex = Flaws.IndexOf(droptarget);
                    Debug.WriteLine("RemoveAt: " + flawIndex);
                    Debug.WriteLine("Insert: " + dropTargetIndex);
                    if (flawIndex > dropTargetIndex)
                    {
                        Flaws.RemoveAt(flawIndex);
                        Flaws.Insert(dropTargetIndex, droppedFlaw);
                    }
                    else if (flawIndex < dropTargetIndex)
                    {
                        Flaws.Insert(dropTargetIndex + 1, droppedFlaw);
                        Flaws.RemoveAt(flawIndex);
                    }
                    else
                    {
                        return;
                    }

                    foreach (FlawM element in Flaws)
                    {
                        element.FlawNumber = Flaws.IndexOf(element) + 1;
                    }
                }
            }
        }

        private void Flaw_Drag(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FlawV)))
            {
                int flawIndex = (int)e.Data.GetData("Int");
                FlawV droppedFlaw = (FlawV)e.Data.GetData(typeof(FlawV));

                var droptarget = (e.OriginalSource as FrameworkElement)?.DataContext as FlawV;
                if (droptarget != null)
                {
                    
                }
            }
        }

        private void Flaw_DragEter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FlawV)))
            {
                int flawIndex = (int)e.Data.GetData("Int");
                FlawV droppedFlaw = (FlawV)e.Data.GetData(typeof(FlawV));

                var droptarget = (e.OriginalSource as FrameworkElement)?.DataContext as FlawV;
                if (droptarget != null)
                {

                }
            }
        }

        private void RemoveFlaw(object sender, MouseEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
        }

        private void RemoveFlaw_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RemoveFlaw_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var flawToRemove = flawsPanel.SelectedItem as FlawM;
            Flaws.Remove(flawToRemove);
            foreach (FlawM element in Flaws)
            {
                element.FlawNumber = Flaws.IndexOf(element) + 1;
            }
        }
        private void InsertFlaw_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void InsertFlaw_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int instertFlawAt = flawsPanel.SelectedIndex;
            FlawM newFlaw = new FlawM { FlawNumber = instertFlawAt + 1, FlawText = "", IsBold = false };
            Flaws.Insert(instertFlawAt, newFlaw);
            foreach (FlawM element in Flaws)
            {
                element.FlawNumber = Flaws.IndexOf(element) + 1;
            }
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

    public class FlawSaveList
    {
        [XmlArrayAttribute("flaws")]
        public List<FlawVM> AllFlaws;
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand CreateTestDocx = new RoutedUICommand("CreateTestDocx", "CreateTestDocx", typeof(CustomCommands));
        public static readonly RoutedUICommand AddFlaw = new RoutedUICommand("AddFlaw", "AddFlaw", typeof(CustomCommands));
        public static readonly RoutedUICommand SaveProject = new RoutedUICommand("SaveProject", "SaveProject", typeof(CustomCommands));
        public static readonly RoutedUICommand RemoveFlaw = new RoutedUICommand("RemoveFlaw", "RemoveFlaw", typeof(CustomCommands));
        public static readonly RoutedUICommand InsertFlaw = new RoutedUICommand("InsertFlaw", "InsertFlaw", typeof(CustomCommands));
    }


}
