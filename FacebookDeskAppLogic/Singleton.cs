using System;
using System.Reflection;

namespace FacebookDeskAppLogic
{
    public static class Singleton<T> where T: class
    {
        static Singleton(){}

        private static readonly object sr_CreationalLockContext = new object();
        private static T s_Instance = null;
        public static T Instance
        {
            get
            {
                if(s_Instance == null)
                {
                    lock(sr_CreationalLockContext)
                    {
                        if(s_Instance == null)
                        {
                            Type typeOfT = typeof(T);
                            ConstructorInfo[] constructors = typeOfT.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            s_Instance = (T)constructors[0].Invoke(null);
                        }
                    }
                }

                return s_Instance;
            }
        }
    }
}
