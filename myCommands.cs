// (C) Copyright 2013 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(AcadSAPConnector.MyCommands))]

namespace AcadSAPConnector
{
    public class MyCommands
    {
        static Autodesk.AutoCAD.Windows.PaletteSet _ps = null;

        static SAPConnectorCtrl _connectorCtrl = null;

        [CommandMethod("SAPConnector")]
        public void SAPConnector()
        {
            if (_ps != null)
            {
                _ps.Visible = true;

                _connectorCtrl.RefreshContent();

                return;
            }

            _ps = new Autodesk.AutoCAD.Windows.PaletteSet(
                "SAP Connector",
                new Guid("57AA1B6D-7D2C-426B-B315-6FA7714DA223"));

            _ps.Text = "SAP Connector";

            _ps.DockEnabled = Autodesk.AutoCAD.Windows.DockSides.Left |
                Autodesk.AutoCAD.Windows.DockSides.Right |
                Autodesk.AutoCAD.Windows.DockSides.None;

            _ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowPropertiesMenu |
                       Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton |
                       Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;

            _ps.MinimumSize = new System.Drawing.Size(200, 300);
            _ps.Size = new System.Drawing.Size(300, 500);

            _connectorCtrl = new SAPConnectorCtrl();

            Autodesk.AutoCAD.Windows.Palette palette = 
                _ps.Add("SAP Connector", _connectorCtrl);

            _ps.Visible = true;

            _connectorCtrl.RefreshContent();
        }
    }
}
