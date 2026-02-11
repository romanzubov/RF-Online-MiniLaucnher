using System;

namespace PatchGenerator.Helper
{
    public abstract class Singleton
    {
    }
    public abstract class Singleton<T> : Singleton where T : new()
    {
        private static T instance;
        private static object syncRoot = new Object();

        public static T GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new T();
                    }
                }

                return instance;
            }
        }
    }
}
