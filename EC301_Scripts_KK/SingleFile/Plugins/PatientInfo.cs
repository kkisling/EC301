using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

[assembly: AssemblyVersion("1.0.0.1")]

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
            // TODO : Add here the code that is called when the script is launched from Eclipse.


            string planID = context.PlanSetup.Id;

            MessageBox.Show(String.Format("Name of plan: {0}", planID));


            string name = context.Patient.Name;
            string createdDate = context.Patient.CreationDateTime.ToString();
            string sex = context.Patient.Sex;
            string DOB = String.IsNullOrEmpty(context.Patient.DateOfBirth.ToString()) ? 
                "Data not available." : context.Patient.DateOfBirth.ToString();

            string courses = "";
            foreach (var c in context.Patient.Courses)
            {
                courses += String.Format("Course: {0}\n", c.Id);
            }
            string numCourse = context.Patient.Courses.Count().ToString();

            MessageBox.Show(String.Format("Num of Courses: {0}", numCourse));
             
            MessageBox.Show(courses);

            //MessageBox.Show(String.Join("\n", context.Patient.Courses.Select(x => x.Id)));


            MessageBox.Show(String.Format("Patient Name: {0} \nCreated: {1} \nSex: {2} \nDOB: {3}", 
                name, createdDate, sex, DOB));

            MessageBox.Show(String.Format("Struct Sets: {0}", context.Patient.StructureSets.Count().
                ToString()));

        }
  }
}
