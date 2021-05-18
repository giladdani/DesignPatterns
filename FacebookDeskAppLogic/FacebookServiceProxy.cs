using System;
using System.IO;
using System.Xml.Serialization;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookDeskAppLogic
{
    public class FacebookServiceProxy
    {
        // Data Members
        private const string k_xmlFileName = "UserSettings.xml";
        private static readonly string Sr_xmlFilePath = Path.Combine(Environment.CurrentDirectory, k_xmlFileName);

        // Private Methods
        private static void saveUserSettingsToFile()
        {
            UserSettings userSettings = new UserSettings();
            userSettings.AccessToken = Singleton<LoggedinUserData>.Instance.LoginResult.AccessToken;
            try
            {
                using (Stream fileStream = new FileStream(Sr_xmlFilePath, FileMode.OpenOrCreate))
                {
                    XmlSerializer mySerializer = new XmlSerializer(userSettings.GetType());
                    mySerializer.Serialize(fileStream, userSettings);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to save settings");
            }
        }

        private static UserSettings loadFromFile()
        {
            UserSettings userSettings = new UserSettings();
            try
            {
                if (File.Exists(Sr_xmlFilePath))
                {
                    using (Stream fileStream = new FileStream(Sr_xmlFilePath, FileMode.OpenOrCreate))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
                        userSettings = mySerializer.Deserialize(fileStream) as UserSettings;
                    }

                    File.Delete(Sr_xmlFilePath);
                }

                return userSettings;
            }
            catch (Exception)
            {
                throw new Exception("Failed to load settings");
            }
        }

        // Public Methods
        public static LoginResult Login(string i_AppId, params string[] i_Permissions)
        {
            LoginResult result = null;
            try
            {
                UserSettings userSettings = loadFromFile();
                if (string.IsNullOrEmpty(userSettings.AccessToken) == false)
                {
                    result = FacebookService.Connect(userSettings.AccessToken);
                }
                else
                {
                    result = FacebookService.Login(i_AppId, i_Permissions);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return result;
        }

        public static LoginResult Login(string i_AppId, string i_Permissions)
        {
            LoginResult result = null;
            try
            {
                UserSettings userSettings = loadFromFile();
                if (string.IsNullOrEmpty(userSettings.AccessToken) == false)
                {
                    result = FacebookService.Connect(userSettings.AccessToken);
                }
                else
                {
                    result = FacebookService.Login(i_AppId, i_Permissions);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return result;
        }

        public static void Logout(Action i_CallBackAfterSuccessfulLogout = null, string i_AccessToken = null)
        {
            saveUserSettingsToFile();
            FacebookService.Logout();
        }

        public static LoginResult Connect(string i_NonExpiredAccessToken)
        {
            return FacebookService.Connect(i_NonExpiredAccessToken);
        }

        public static T GetObject<T>(string i_ID, string i_Fields = "")
            where T : FacebookObject, new()
        {
            return FacebookService.GetObject<T>(i_ID, i_Fields);
        }

        public static dynamic GetObject(string i_ID, string i_Fields = "")
        {
            return FacebookService.GetObject(i_ID, i_Fields);
        }

        public static T GetObjectByFQL<T>(string i_FqlPredicate, string i_Fields = "")
            where T : FacebookObject, new()
        {
            return FacebookService.GetObjectByFQL<T>(i_FqlPredicate, i_Fields);
        }

        public static void GetObjectAsync<T>(
            string i_ID,
            Action<FBAsyncCompletedEventArgs<T>> i_CallBackDelegate,
            string i_Fields = "")
            where T : DynamicWrapper, new()
        {
            FacebookService.GetObjectAsync<T>(i_ID, i_CallBackDelegate, i_Fields);
        }

        public static FacebookObjectCollection<object> GetDynamicCollection(
            string i_Connection,
            string i_ParentID = "me",
            string i_Fields = "",
            int? i_Limit = null)
        {
            return FacebookService.GetDynamicCollection(i_Connection, i_ParentID, i_Fields, i_Limit);
        }

        public static FacebookObjectCollection<T> GetCollection<T>(
            string i_Connection,
            string i_ParentID = "me",
            string i_Fields = "",
            int? i_Limit = null,
            DynamicWrapper.eLoadOptions i_LoadOptions = DynamicWrapper.eLoadOptions.Full,
            string i_JsonFieldOfData = null)
            where T : DynamicWrapper, new()
        {
            return FacebookService.GetCollection<T>(i_Connection, i_ParentID, i_Fields, i_Limit);
        }

        public static dynamic GetDynamicData(
            string i_Connection,
            string i_ParentID = "me",
            string i_Fields = "",
            int? i_Limit = null,
            DynamicWrapper.eLoadOptions i_LoadOptions = DynamicWrapper.eLoadOptions.Full,
            string i_JsonFieldOfData = null)
        {
            return FacebookService.GetDynamicData(i_Connection, i_ParentID, i_Fields, i_Limit);
        }

        public static FacebookObjectCollection<T> GetCollectionByFQL<T>(
            string i_FqlPredicate,
            string i_Fields = "",
            DynamicWrapper.eLoadOptions i_LoadOptions = DynamicWrapper.eLoadOptions.Full)
            where T : FacebookObject, new()
        {
            return FacebookService.GetCollectionByFQL<T>(i_FqlPredicate, i_Fields, i_LoadOptions);
        }

        public static void GetCollectionAsync<T>(
            string i_Connection,
            Action<FBCollectionEventArgs<T>> i_CallBackDelegate,
            string i_ParentID = "me",
            string i_Fields = "",
            int? i_Limit = null,
            DynamicWrapper.eLoadOptions i_LoadOptions = DynamicWrapper.eLoadOptions.Full)
            where T : DynamicWrapper, new()
        {
            FacebookService.GetCollectionAsync<T>(i_Connection, i_CallBackDelegate, i_ParentID, i_Fields, i_Limit, i_LoadOptions);
        }

        // Properties
        public string LastAccessToken { get; set; }
    }
}
