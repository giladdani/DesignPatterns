using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookDeskAppUI
{
    class FaceBookServiceProxy
    {
        // Data Members
        private static bool m_IsFirstLogin = true;
        private const string k_XmlFileName = "LastUserDetails.xml";

        // Private Methods
        private static void deleteFileIfNeeded()
        {
            if (m_IsFirstLogin == true)
            {
                if (File.Exists(k_XmlFileName))
                {
                    File.Delete(k_XmlFileName);
                }
            }
        }

        private static LoginResult loadFromFile()
        {
            LoginResult result = null;
            if (File.Exists(k_XmlFileName))
            {
                using(Stream stream = new FileStream(k_XmlFileName, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(result.GetType());
                    result = serializer.Deserialize(stream) as LoginResult;
                }
            }

            return result;
        }

        private static void saveToFile(LoginResult result)
        {
            using (Stream stream = new FileStream(k_XmlFileName, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(result.GetType());
                serializer.Serialize(stream, result);
            }
        }

        // Public Methods
        public static LoginResult Login(string i_AppId, params string[] i_Permissions)
        {
            LoginResult result = null;
            try
            {
                deleteFileIfNeeded();
                result = FacebookService.Login(i_AppId, i_Permissions);
                saveToFile(result);
            }
            catch (Exception ex)
            {
                result = loadFromFile();
            }

            m_IsFirstLogin = false;
            return result;
        }

        public static LoginResult Login(string i_AppId, string i_Permissions)
        {
            LoginResult result = null;
            try
            {
                deleteFileIfNeeded();
                result = FacebookService.Login(i_AppId, i_Permissions);
                saveToFile(result);
            }
            catch (Exception ex)
            {
                result = loadFromFile();
            }

            m_IsFirstLogin = false;
            return result;
        }

        public static void Logout(Action i_CallBackAfterSuccessfulLogout = null, string i_AccessToken = null)
        {
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
    }
}
