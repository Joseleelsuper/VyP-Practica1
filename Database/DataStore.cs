using System;

namespace Database
{
    public static class DataStore
    {
        private static readonly CapaDatos _instance = new CapaDatos();

        public static CapaDatos Instance
        {
            get { return _instance; }
        }
    }
}
