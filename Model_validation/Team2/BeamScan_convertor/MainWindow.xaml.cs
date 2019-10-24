using System;
using System.IO;
using System.Collections.Generic;
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
using VMS.TPS.Common.Model.API;
using Microsoft.Win32;
using ScanClass;

namespace BeamScan_convertor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ScanModel> ds_list = new List<ScanModel>();
        public MainWindow() 
        {
            InitializeComponent();
        }
        public VMS.TPS.Common.Model.API.Application app;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            {
             //   try
               // {
                   // using (VMS.TPS.Common.Model.API.Application app1 = VMS.TPS.Common.Model.API.Application.CreateApplication())
                 //   {
                   //     app = app1;
                 //   }
              //  }
               // catch
               // {
                 //   throw new ApplicationException("Could not open app:");
               // }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ds_list.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".asc";
            if(ofd.ShowDialog()== true)
            {
                //MessageBox.Show(ofd.FileName);
                foreach (string line in File.ReadAllLines(ofd.FileName))
                {
                    if (line.Contains("STOM")){
                        ds_list.Add(new ScanModel());
                    }
                }
            }
        }
        
    }
}
