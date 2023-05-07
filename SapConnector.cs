using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace AcadSAPConnector
{
    ////////////////////////////////////////////////////////////////////////////////
    // SAP Connector Utility
    //
    ////////////////////////////////////////////////////////////////////////////////
    class SapConnector
    {
        MATERIAL.MATERIAL _service;

        KeyValueConfigurationCollection _settings;

        ////////////////////////////////////////////////////////////////////////////
        // Constructor
        //
        ////////////////////////////////////////////////////////////////////////////
        public SapConnector()
        {
            FileInfo fi = new FileInfo(
                    System.Reflection.Assembly.GetExecutingAssembly().Location);

            string configPath = fi.DirectoryName + "\\" + "addin.config";

            Configuration config =
                ConfigurationManager.OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap { ExeConfigFilename = configPath },
                    ConfigurationUserLevel.None);

            _settings = config.AppSettings.Settings;
        }

        ////////////////////////////////////////////////////////////////////////////
        // Initialize SAP web service
        //
        ////////////////////////////////////////////////////////////////////////////
        public void InitializeSAP()
        {
            string url =
                "http://gw.esworkplace.sap.com/sap/opu/sdata/IWCNT/MATERIAL/";

            _service = new MATERIAL.MATERIAL(new Uri(url));

            _service.Credentials = new NetworkCredential(
                _settings["SAPLogin"].Value,
                _settings["SAPPassword"].Value);
        }

        ////////////////////////////////////////////////////////////////////////////
        // Returns list of count materials
        //
        ////////////////////////////////////////////////////////////////////////////
        public List<MATERIAL.Material> GetMaterials(int count)
        {
            try
            {
                List<MATERIAL.Material> result = new List<MATERIAL.Material>();

                if (_service == null)
                    return result;

                var query = (from MATERIAL.Material material in
                                 _service.MaterialCollection
                             select material).Take(count);

                foreach (MATERIAL.Material material in query)
                {
                    result.Add(material);
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // Returns list of filtered materials
        //
        ////////////////////////////////////////////////////////////////////////////
        public List<MATERIAL.Material> FilterMaterials(
            int count,
            string expression,
            bool contain)
        {
            try
            {
                List<MATERIAL.Material> result = new List<MATERIAL.Material>();

                if (contain)
                {
                    var query = (from MATERIAL.Material material in _service.MaterialCollection
                                 where material.MaterialDescription == expression
                                 select material).Take(count);

                    foreach (MATERIAL.Material material in query)
                    {
                        result.Add(material);
                    }
                }
                else
                {
                    var query = (from MATERIAL.Material material in _service.MaterialCollection
                                 where material.MaterialDescription != expression
                                 select material).Take(count);

                    foreach (MATERIAL.Material material in query)
                    {
                        result.Add(material);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
