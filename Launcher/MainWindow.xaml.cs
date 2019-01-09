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
            launchbutton.Background = Brushes.LightBlue;
        }
        string currentpath = "";
        List<string> files = new List<string>();
        string[] allfiles;
        int clickmode = 1;
        void createButtons()
        {
            allfiles = Directory.GetFiles(currentpath, "*.csproj", SearchOption.AllDirectories);
            wp.Children.Clear();
            textpath.Text = currentpath;
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
                newBtn.Tag = i.ToString();
                newBtn.Content = System.IO.Path.GetFileNameWithoutExtension(exepath[0]);
                newBtn.Click += startProcess;
                newBtn.Height = 100;
                newBtn.Width = 150;
                newBtn.Margin = new Thickness(10, 10, 10, 10);
                // newBtn.Children.Add(stckPanel);
                wp.Children.Add(newBtn);
                //Icon icon = Icon.ExtractAssociatedIcon(exepath[i]);
                //newBtn.Content = icon;

            }        
        }
        void startProcess(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int pathIndex = int.Parse(btn.Tag.ToString());
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
            string parent = System.IO.Directory.GetParent(exepath[0]).FullName;
            string parent2 = System.IO.Directory.GetParent(parent).FullName;
            string parent3 = System.IO.Directory.GetParent(parent2).FullName;

            if (clickmode == 1)
            {
                initprocess(value);
            }
            else if (clickmode == 2)
            {
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.Delete(parent3, true);
                        createButtons();
                        errortext.Text = "Succefuly deleted.";
                    }

                    catch (System.IO.IOException a)
                    {
                        errortext.Text = a.Message;
                    }
                }
            }
            else if (clickmode == 3)
            {
                try
                {
                    System.IO.Directory.Move(parent3, textpath.Text);
                    errortext.Text = "Succefuly moved.";
                }
                catch (System.IO.IOException a)
                {
                    errortext.Text = a.Message;
                }           
            }
        }

        Process initprocess(string programPath)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(programPath);
            proc.Start();
            return proc;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newpath = textpath.Text;
                currentpath = newpath;
                createButtons();
                errortext.Text = "Succefuly processed.";
            }
            catch (Exception d)
            {
                errortext.Text = d.Message;
            }
        }

        private void Launch_Button(object sender, RoutedEventArgs e)
        {
            if (clickmode != 1)
            {
                clickmode = 1;
                launchbutton.Background = Brushes.LightBlue;
                deletebutton.ClearValue(Button.BackgroundProperty);
                movebutton.ClearValue(Button.BackgroundProperty);
            }
        }

        private void Delete_Activate(object sender, RoutedEventArgs e)
        {
            if (clickmode != 2)
            {
                clickmode = 2;
                deletebutton.Background = Brushes.LightBlue;
                launchbutton.ClearValue(Button.BackgroundProperty);
                movebutton.ClearValue(Button.BackgroundProperty);
            }
        }

        private void Move_Button(object sender, RoutedEventArgs e)
        {
            if (clickmode != 3)
            {
                clickmode = 3;
                movebutton.Background = Brushes.LightBlue;
                launchbutton.ClearValue(Button.BackgroundProperty);
                deletebutton.ClearValue(Button.BackgroundProperty);
            }
        }
    }
}
