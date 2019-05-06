using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using System.IO;
using System.Windows.Forms;

namespace Export_Import_Tool
{
    
    class ObtainLinkFiles
    {
        Document initialDoc;
        public List<string[]> contentContainer = new List<string[]>();
        public List<string> passedFileList = new List<string>();


        public ObtainLinkFiles(Document doc)
        {
            this.initialDoc = doc;
        }

        //The method to find the link information of all the links related to the selected file
        public List<string[]> GetAllRevitInstances(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(RevitLinkInstance));

            string fullPath = doc.PathName.ToString();
            string[] fullPathSpliter = fullPath.Split('\\');
            string docName = fullPathSpliter[fullPathSpliter.Length - 1];

            if (!passedFileList.Contains(docName))
            {
                passedFileList.Add(docName);
            }
            
            if(collector == null)
            {
                List<string[]> a = null;
                return a;
            }

            foreach (Element elem in collector)
            {
                RevitLinkInstance instance = elem as RevitLinkInstance;
                Document linkDoc = instance.GetLinkDocument();
                RevitLinkType linkType = doc.GetElement(instance.GetTypeId()) as RevitLinkType;
                LinkedFileStatus fileStatus = linkType.GetExternalFileReference().GetLinkedFileStatus();
                bool savePosition = linkType.HasSaveablePositions();
                
                //Gain the information of a link
                string[] wholeFileName = instance.Name.Split(':');
                string filePath;
                if (linkDoc != null)
                {
                    filePath = linkDoc.PathName;
                }
                else
                {
                    filePath = wholeFileName[0];
                }
                string fileName = wholeFileName[0];
                string status = fileStatus.ToString();
                string referenceType = linkType.AttachmentType.ToString();
                string pathType = linkType.PathType.ToString();
                string[] linkInformation = { fileName, status, referenceType, savePosition.ToString(), filePath, pathType };

                //All the information is stored in the form of List<string[]>
                if (!passedFileList.Contains(filePath))
                {
                    passedFileList.Add(filePath);
                    contentContainer.Add(linkInformation);
                }
                

                
                
                //Whether the linked file can be loaded and go further
                if(linkDoc != null)
                {
                    GetAllRevitInstances(linkDoc);
                }

                //Whether the linked file has childs
                if (linkType.GetChildIds().Count > 0)
                {
                    foreach (ElementId elemId in linkType.GetChildIds())
                    {
                        RevitLinkInstance childLink = linkDoc.GetElement(elemId) as RevitLinkInstance;
                        if(childLink != null)
                        {
                            Document childDocument = childLink.GetLinkDocument();
                            GetAllRevitInstances(childDocument);
                        }
                        
                    }
                }

                //Whether the linked file has parent
                if (linkType.IsNestedLink)
                {
                    RevitLinkInstance parentLink = linkDoc.GetElement(linkType.GetParentId()) as RevitLinkInstance;
                    if(parentLink != null)
                    {
                        Document parentDocument = parentLink.GetLinkDocument();
                        GetAllRevitInstances(parentDocument);
                    }
                    
                }
            }
            return contentContainer;
        }

    }
}
