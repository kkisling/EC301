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
        List<ScanModel> sm_list = new List<ScanModel>();
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


        /*
        public double Depth { get; set; }
        public string BeamName { get; set; }
        public string BeamEnergy { get; set; }
        public bool FFF { get; set; }
        public double FieldSize { get; set; }
        public double SSD { get; set; }
        public ScanTypes ScanType { get; set; }
        public List<ScanDataPoint> DataPoints { get; set; }

        OPP,//profile
        OPD, //depth dose
        DPR //Diagonal

            public List<ScanDataPoint> DataPoints { get; set; }

            public class ScanDataPoint
            {
                 public double XPosition { get; set; }
                 public double YPosition { get; set; }
                 public double ZPosition { get; set; }
                 public double Dose { get; set; }
            };

    
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sm_list.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;  // allow selection of multiple files

            if (ofd.ShowDialog() == true)  // TRUE if user selects "OK" button on file dialog
            {
                foreach (string fn in ofd.FileNames)  // loop through each file among those selected
                {
                    foreach (string line in File.ReadAllLines(fn))  // loop through each line of the file
                    {
                        /* 
                            ** Reference:  Eclipse Algorithms Reference Guide **
                            STOM: start of measurement
                            ENOM: end of measurement 
                            DPTH: depth of profile (mm)
                            BMTY: beam name (type)
                            FLSZ: field size (assume square)
                            SSD: SSD (mm)
                            TYPE: scan type 
                        */
                        if (line.Contains("STOM"))  
                        {
                            sm_list.Add(new ScanModel());
                        }
                        if (line.Contains("TYPE")) 
                        {  
                            sm_list.Last().ScanType = (ScanTypes)Enum.Parse(typeof(ScanTypes), line.Split(' ').Last()) ;
                        }
                        if (line.Contains("DPTH")) 
                        {
                            sm_list.Last().Depth = Convert.ToDouble(line.Split(' ').Last());
                        }
                        if (line.Contains("BMTY")) 
                        { 
                            sm_list.Last().BeamName = line.Split(' ').Last();
                        }
                        //if (line.Contains("FFF")) 
                        //{
                        //    sm_list.Last().BeamEnergy = "no";
                        //}
                        //if (line.Contains("no")) 
                        //{
                        //    sm_list.Last().FFF = false; ;
                        //}
                        if (line.Contains("FLSZ"))  
                        /* ScanModel class does not specify X and Y field sizes separately.  So, we
                           will read the second (Y axis) field size and assume fld is square.  */
                        {
                            sm_list.Last().BeamName = line.Split('*').Last();
                        }
                        if (line.Contains("SSD")) 
                        {
                            sm_list.Last().SSD = Convert.ToDouble(line.Split(' ').Last());
                        }
                        if (line.Contains("<") && line.Contains(">"))  // data point
                        {
                            sm_list.Last().DataPoints.Add(new ScanData.ScanDataPoint());
                            sm_list.Last().DataPoints.Last().XPosition = Convert.ToDouble(line.Split(' ')[0].Trim('<'));
                            sm_list.Last().DataPoints.Last().YPosition = Convert.ToDouble(line.Split(' ')[1]);
                            sm_list.Last().DataPoints.Last().ZPosition = Convert.ToDouble(line.Split(' ')[2]);
                            sm_list.Last().DataPoints.Last().Dose = Convert.ToDouble(line.Split(' ')[3].Trim('>'));
                           
                        }
                        if (line.Contains("ENOF"))  // end of file
                        {
                        
                            break;
                        }
                    }
                }
            }
        }
        
    }
}
