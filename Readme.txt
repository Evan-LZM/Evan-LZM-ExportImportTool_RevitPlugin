This is the tool used for handling export and import processes between the building data in Revit model files and SQL Server database.
This tool is a plugin used with Revit 2018.

Therefore, it is necessary to customize the database connection information before usage.
- ExportFunctionForm.cs
- Import_Schema.cs

With the same reason, the document path in the program should also be customized based on the user machine.
- ExternalApplicationClass.cs

And the absolute path in addin files also require customization
- Export_Import_Tool.addin
- Export_Import_Tool_2.addin

The addin files are necessary for igniting the plugin. The two addins should be placed 
in C:\ProgramData\Autodesk\Revit\2018\Addins\*.addin