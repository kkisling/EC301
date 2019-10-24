using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
[assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
  public class Script
  {
    public Script()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
    {

            //USER INPUT
            double s_ssd = 100;                                         // SSD 
            string cmach = "HESN5";                                     // MACHINE
            string[] erg = new string[] { "6X", "10X" };                // ENERGY LIST
            double[] jaw_sizes = new double[] { 4, 8, 10, 20 };         // FIELD SIZE LIST

            // Define local _patient, could be pointed to UI
            Patient _patient = context.Patient;
            _patient.BeginModifications();
            
            // SET STRUCTURE SET
            StructureSet _structureSet = _patient.StructureSets.FirstOrDefault();

            // DETERMINE EXTERNAL STRUCTURE
            var bst = _structureSet.Structures.First(x => x.DicomType == "EXTERNAL");
            var bst_mgb = bst.MeshGeometry.Bounds;

            // CREATE NEW COURSE AND NAME IT
            Course C_Auto = _patient.AddCourse();
            C_Auto.Id = string.Format("C_{0}_{1}SSD", cmach, s_ssd.ToString());

            // LOOP THROUGH ENERGIES PROVIDED BY USER, CREATE A PLAN FOR EACH ENERGY
            foreach (var evar in erg)
            {
                // CREATE PLAN
                ExternalPlanSetup eps = C_Auto.AddExternalPlanSetup(_structureSet);

                // SET PRESCRIPTION
                eps.SetPrescription(1, new DoseValue(100, "cGy"), 1);
                eps.Id = evar.ToString();

                // SET FIELD PARAMETERS
                ExternalBeamMachineParameters ebmp = new ExternalBeamMachineParameters(cmach, evar, 600, "STATIC", null);

                // LOOP THROUGH FIELD SIZES, CREATE A BEAM FOR EACH FIELD SIZE
                foreach (double js in jaw_sizes)
                {
                    // SET JAW POSITION BASED ON USER FIELD SIZES - (ASSUMES ALL FIELDS ARE SYMMETRIC)
                    double xjaw = js * 10;
                    double yjaw = js * 10;

                    // DEFINE BEAM PARAMETERS - (ASSUMES NO ANGLES)
                    double coll     = 0;
                    double gant_a   = 0;
                    double couch_a  = 0;

                    // SET ISOCENTER BASED ON USER SSD
                    double iso_x = 0;
                    double iso_y = -10 * (Math.Round(bst_mgb.SizeY / 10) + Math.Round(bst_mgb.Location.Y / 10) + (s_ssd - 100));
                    double iso_z = 0;
                    VVector isovec = new VVector(iso_x, iso_y, iso_z);

                    // CREATE BEAM
                    Beam b = eps.AddStaticBeam(
                        ebmp,
                        new VRect<double>(-0.5 * xjaw, -0.5 * yjaw, 0.5 * xjaw, 0.5 * yjaw),
                        coll, gant_a, couch_a, isovec);

                    // SET BEAM ID
                    b.Id = String.Format("{0}_{1:F1}x{2:F1}", evar, xjaw / 10, yjaw / 10);
                }
                // CALCULATE DOSE
                eps.CalculateDose();
            }
            MessageBox.Show("DONE");
            
    }
  }
}
