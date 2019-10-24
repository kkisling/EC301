using ScanData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanClass
{
    public class ScanModel
    {
        public double Depth { get; set; }
        public string BeamName { get; set; }
        public string BeamEnergy { get; set; }
        public bool FFF { get; set; }
        public double FieldSize { get; set; }
        public double SSD { get; set; }
        public ScanTypes ScanType { get; set; }
        public List<ScanDataPoint> DataPoints { get; set; }
        public ScanModel()
        {
            DataPoints = new List<ScanDataPoint>();
        }
    }
}
