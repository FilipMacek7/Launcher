using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

namespace WpfApp1
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            createButtons();
           
        }
        List<string> files = new List<string>();
        string[] allfiles = Directory.GetFiles(@"D:\maceklu161", "*.csproj", SearchOption.AllDirectories);
        void createButtons()
        {
            for (int i = 0; i < allfiles.Length - 1; i++)
            {

                XDocument doc = XDocument.Load(allfiles[i]);
                //PREDPONA
                var debug = doc.Descendants().First(p => p.Name.LocalName == "OutputPath").Value;
                var release = doc.Descendants().Last(p => p.Name.LocalName == "OutputPath").Value;

                //GET EXE
                files.Add(System.IO.Path.GetFileName(allfiles[i]));
                int pocetp = allfiles[i].Length - files[i].Length;
                string path = allfiles[i].Substring(0, pocetp);


                string[] exepath = Directory.GetFiles(path + debug, "*.exe", SearchOption.AllDirectories);
                //BUTTON
                Button newBtn = new Button();
                newBtn.Name = "Button"+i.ToString();
                newBtn.Content = System.IO.Path.GetFileNameWithoutExtension(exepath[0]);
                newBtn.Click += startProcess;
                sp.Children.Add(newBtn);
            }
        }
        void startProcess(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int pathIndex = int.Parse(btn.Name.Substring(4, btn.Name.Length - 1));
            XDocument doc = XDocument.Load(allfiles[pathIndex]);
            //PREDPONA
            var debug = doc.Descendants().First(p => p.Name.LocalName == "OutputPath").Value;
            var release = doc.Descendants().Last(p => p.Name.LocalName == "OutputPath").Value;

            //GET EXE
            files.Add(System.IO.Path.GetFileName(allfiles[pathIndex]));
            int pocetp = allfiles[pathIndex].Length - files[pathIndex].Length;
            string path = allfiles[pathIndex].Substring(0, pocetp);


            string[] exepath = Directory.GetFiles(path + debug, "*.exe", SearchOption.AllDirectories);
            string value = exepath[0];
            initprocess(value);
        }

        Process initprocess(string programPath)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(programPath);
            proc.Start();
            return proc;
        }

    }
}
