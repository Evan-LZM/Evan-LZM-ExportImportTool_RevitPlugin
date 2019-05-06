using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace Export_Import_Tool
{
    public class ExternalApplicationClass : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //Icon Creation
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Plugin Function");

            string thisAssemblyPath = @"C:\Users\Evan_PC\Desktop\Revit_Project\Export_Import_Tool_Files\Export_Import_Tool_Files\Export_Import_Tool\Export_Import_Tool\bin\Debug\Export_Import_Tool.dll";  //Please enter the path of the dll file when customized
            PushButtonData buttonData = new PushButtonData("Export/Import", "Export/Import", thisAssemblyPath, "Export_Import_Tool.ExternalCommandClass");

            buttonData.AvailabilityClassName = "Export_Import_Tool.Availability";

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            pushButton.ToolTip = "Export and import function realization";

            //Icon Picture
            Uri uriImage = new Uri(@"C:\Users\Evan_PC\Desktop\Revit_Project\Export_Import_Tool_Files\Export_Import_Tool_Files\Export_Import_Tool\plugin.png"); //The path of the icon
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            pushButton.Enabled = true;

            return Result.Succeeded;
        }
    }

    //This class is used to enable the push button when there is no file activated
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication a, CategorySet b)
        {
            return true;
        }
    }
}
