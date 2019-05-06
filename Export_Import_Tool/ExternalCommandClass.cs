using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;

namespace Export_Import_Tool
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class ExternalCommandClass : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Invoke the FunctionSelectionForm 
            System.Windows.Forms.DialogResult result;

            FunctionSelectionForm NewForm = new FunctionSelectionForm(commandData);
            result = NewForm.ShowDialog();
            return Result.Succeeded;

        }

    }
}
