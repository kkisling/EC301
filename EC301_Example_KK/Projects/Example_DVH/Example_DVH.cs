using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;


// Do not change namespace and class name
// otherwise Eclipse will not be able to run the script.
namespace VMS.TPS
{
    class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context, Window window)
        {
            PlanSetup planSetup = context.PlanSetup;

            // If there's no selected plan with calculated dose throw an exception
            if (planSetup == null || planSetup.Dose == null)
                throw new ApplicationException("Please open a calculated plan before using this script.");

            // Retrieve StructureSet
            StructureSet structureSet = planSetup.StructureSet;
            if (structureSet == null)
                throw new ApplicationException("The selected plan does not reference a StructureSet.");

            // For this example we will retrieve first available structure of PTV type
            /*Structure target = null;
            foreach (var structure in structureSet.Structures)
            {
                if (structure.Id == planSetup.TargetVolumeID)
                {
                    target = structure;
                    break;
                }

            }
            if (target == null)
                throw new ApplicationException("The selected plan does not have a PTV.");

            // Retrieve DVH data
            DVHData dvhData = planSetup.GetDVHCumulativeData(target,
                                          DoseValuePresentation.Relative,
                                          VolumePresentation.Relative, 0.1);

            if (dvhData == null)
                throw new ApplicationException("DVH data does not exist. Script execution cancelled.");*/

            // Add existing WPF control to the script window.
            var mainControl = new Example_DVH.MainControl();
            window.Content = mainControl;
            window.Width = 900;
            window.Height = 600;

            /*foreach (var structure in structureSet.Structures)
            {
                mainControl.MyComboBox.Items.Add(structure.Id);
            }*/
            // using linq
            mainControl.MyComboBox.ItemsSource = structureSet.Structures.Select(x => x.Id);
            mainControl.plan = planSetup;

            // window.Title = "Plan : " + planSetup.Id + ", Structure : " + target.Id;
            window.Title = "Plan : " + planSetup.Id;

            // Draw DVH
            //mainControl.DrawDVH(dvhData);
        }
    }
}
