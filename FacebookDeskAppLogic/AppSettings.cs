using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace FacebookDeskAppLogic
{
    public class AppSettings
    {
        // Private Members
        private const string k_FileName = "appSettings.xml";
        private static readonly string sr_RemaindersDataPath = Path.Combine(Environment.CurrentDirectory, k_FileName);

        // Ctor
        private AppSettings()
        {
            LastWindowLocation = new Point(20, 50);
            LastWindowSize = new Size(1000, 500);
            RememberSettings = false;
            LastAccessToken = null;
        }

        // Public Methods
        public static AppSettings LoadFromFile()
        {
            AppSettings appSettings = new AppSettings();
            try
            {
                if (File.Exists(sr_RemaindersDataPath))
                {
                    using (Stream fileStream = new FileStream(sr_RemaindersDataPath, FileMode.OpenOrCreate))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(AppSettings));
                        appSettings = mySerializer.Deserialize(fileStream) as AppSettings;
                    }
                }
                else
                {
                    File.Create(sr_RemaindersDataPath);
                }

                return appSettings;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load settings");
            }
        }

        public void SaveToFile()
        {
            try
            {
                using (Stream fileStream = new FileStream(sr_RemaindersDataPath, FileMode.Truncate))
                {
                    XmlSerializer mySerializer = new XmlSerializer(this.GetType());
                    mySerializer.Serialize(fileStream, this);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save settings");
            }
        }

        // Properties
        public Point LastWindowLocation { get; set; }
        public Size LastWindowSize { get; set; }
        public bool RememberSettings { get; set; }
        public string LastAccessToken { get; set; }
    }
}
