using System;
using System.Collections.Generic;
using System.Linq;
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
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Example_DVH
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public PlanSetup plan;

        public MainControl()
        {
            InitializeComponent();
        }

        public void DrawDVH(DVHData dvhData, Structure s)
        {
            // Calculate multipliers for scaling DVH to canvas.
            //double xCoeff = MainCanvas.Width / dvhData.MaxDose.Dose;
            plan.DoseValuePresentation = DoseValuePresentation.Absolute;
            double xCoeff = MainCanvas.Width / plan.Dose.DoseMax3D.Dose;
            double yCoeff = MainCanvas.Height / 100;

            // Set X axis label
            // If added .Dose, it would not have a unit, and it would need to be hard coded into the string
            DoseMaxLabel.Content = string.Format("{0:F0}", plan.Dose.DoseMax3D);

            // Add stats to stack panel
            stats_sp.Children.Add(new TextBlock
            {
                Text = $"{s.Id}: Max={dvhData.MaxDose}; Min={dvhData.MinDose}; Mean={dvhData.MeanDose}"
            });

            // Draw histogram 
            for (int i = 0; i < dvhData.CurveData.Length - 1; i++)
            {
                // Set drawing line parameters
                //SolidColorBrush brush = new SolidColorBrush(s.Color); didn't work
                var line = new Line() { Stroke = new SolidColorBrush(s.Color), StrokeThickness = 4.0 };

                // Set line coordinates
                line.X1 = dvhData.CurveData[i].DoseValue.Dose * xCoeff;
                line.X2 = dvhData.CurveData[i + 1].DoseValue.Dose * xCoeff;
                // Y axis start point is top-left corner of window, convert it to bottom-left.
                line.Y1 = MainCanvas.Height - dvhData.CurveData[i].Volume * yCoeff;
                line.Y2 = MainCanvas.Height - dvhData.CurveData[i + 1].Volume * yCoeff;

                // Add line to the existing canvas
                MainCanvas.Children.Add(line);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyComboBox.SelectedIndex != -1)
            {
                Structure s = plan.StructureSet.Structures
                    .FirstOrDefault(x => x.Id == MyComboBox.SelectedItem.ToString());
                //This should always be true, but we would have an issue if it wasn't
                DVHData dvhData = plan.GetDVHCumulativeData(s, DoseValuePresentation.Absolute,
                                              VolumePresentation.Relative, 0.1);
                if (dvhData != null)
                {
                    DrawDVH(dvhData, s);

                }
                

            }



        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            
            PrintDialog dlg = new PrintDialog();
            dvh_btn.Visibility = print_btn.Visibility = MyComboBox.Visibility = Visibility.Hidden;
            //this shows the dialog where the user can choose their printer
            //dlg.ShowDialog()
            dlg.PrintVisual(this, "testPrint");
            dvh_btn.Visibility = print_btn.Visibility = MyComboBox.Visibility = Visibility.Visible;
        }
    }
}
