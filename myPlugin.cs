// (C) Copyright 2013 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: ExtensionApplication(typeof(AcadSAPConnector.MyPlugin))]

namespace AcadSAPConnector
{
    // This class is instantiated by AutoCAD once and kept alive for the 
    // duration of the session. If you don't do any one time initialization 
    // then you should remove this class.
    public class MyPlugin : IExtensionApplication
    {
        void IExtensionApplication.Initialize()
        {
            Register();
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
        }

        public void Register()
        {
            //AutoCAD (or vertical) and Application keys
            Microsoft.Win32.RegistryKey acadKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    HostApplicationServices.Current.MachineRegistryProductRootKey);

            Microsoft.Win32.RegistryKey acadAppKey = acadKey.OpenSubKey("Applications", true);

            string curAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string curAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string curAssemblyFullName = System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName;

            //already registered?
            String[] subKeyNames = acadAppKey.GetSubKeyNames();

            foreach (String subKeyName in subKeyNames)
            {
                if (subKeyName.Equals(curAssemblyName))
                {
                    Microsoft.Win32.RegistryKey subkey = acadAppKey.OpenSubKey(subKeyName, true);
                    subkey.SetValue("LOADER", curAssemblyPath, Microsoft.Win32.RegistryValueKind.String);
                    subkey.Close();
                    acadAppKey.Close();
                    return;
                }
            }

            //create the addin key
            Microsoft.Win32.RegistryKey acadAppAddInKey = acadAppKey.CreateSubKey(curAssemblyName);

            acadAppAddInKey.SetValue("DESCRIPTION", curAssemblyFullName, Microsoft.Win32.RegistryValueKind.String);
            acadAppAddInKey.SetValue("LOADCTRLS", 14, Microsoft.Win32.RegistryValueKind.DWord);
            acadAppAddInKey.SetValue("LOADER", curAssemblyPath, Microsoft.Win32.RegistryValueKind.String);
            acadAppAddInKey.SetValue("MANAGED", 1, Microsoft.Win32.RegistryValueKind.DWord);

            acadAppKey.Close();
        }
    }
}
