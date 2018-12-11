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
            searchApps();
        }
        List<string> files = new List<string>();
        string searchApps()
        {
            string[] allfiles = Directory.GetFiles(@"D:\maceklu161", "*.csproj", SearchOption.AllDirectories);
            string value;
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
                value = exepath[0];
                //BUTTON
                Button newBtn = new Button();
                newBtn.Name = "Button" + i.ToString();
                newBtn.Content = System.IO.Path.GetFileNameWithoutExtension(exepath[0]);
                sp.Children.Add(newBtn);

            }
            return value;
        }
        void startProcess(object sender, EventArgs e)
        {
            initprocess(searchApps());
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
