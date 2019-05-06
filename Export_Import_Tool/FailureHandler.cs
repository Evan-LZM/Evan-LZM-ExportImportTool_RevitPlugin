using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using System.IO;
using System.Data.Odbc;
using System.Collections;

namespace Execute_Forms
{
    public class FailureHandler : IFailuresPreprocessor

    {

        public string ErrorMessage { set; get; }

        public string ErrorSeverity { set; get; }



        public FailureHandler()

        {

            ErrorMessage = "";

            ErrorSeverity = "";

        }



        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)

        {

            IList<FailureMessageAccessor> failureMessages = failuresAccessor.GetFailureMessages();



            foreach (FailureMessageAccessor failureMessageAccessor in failureMessages)

            {

                // We're just deleting all of the warning level   

                // failures and rolling back any others  



                FailureDefinitionId id = failureMessageAccessor.GetFailureDefinitionId();



                try

                {

                    ErrorMessage = failureMessageAccessor.GetDescriptionText();

                }

                catch

                {

                    ErrorMessage = "Unknown Error";

                }



                try

                {

                    FailureSeverity failureSeverity = failureMessageAccessor.GetSeverity();



                    ErrorSeverity = failureSeverity.ToString();



                    if (failureSeverity == FailureSeverity.Warning)

                    {

                        // 如果是警告，则禁止消息框  

                        failuresAccessor.DeleteWarning(failureMessageAccessor);

                    }

                    else

                    {

                        // 如果是错误：则取消导致错误的操作，但是仍然继续整个事务  

                        return FailureProcessingResult.ProceedWithRollBack;

                    }

                }

                catch

                {

                }

            }

            return FailureProcessingResult.Continue;

        }

    }
}



