using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model
{
    public struct LoginNumPad
    {
        static string _Key1 = "1";
        static string _Key2 = "2";
        static string _Key3 = "3";
        static string _Key4 = "4";
        static string _Key5 = "5";
        static string _Key6 = "6";
        static string _Key7 = "7";
        static string _Key8 = "8";
        static string _Key9 = "9";
        static string _Key0 = "0";
        static string _KeyStar = "*";
        static string _KeyHash = "#";

        public static string Key1
        {
            get { return _Key1; }
        }
        public static string Key2
        {
            get { return _Key2; }
        }
        public static string Key3
        {
            get { return _Key3; }
        }
        public static string Key4
        {
            get { return _Key4; }
        }public static string Key5
        {
            get { return _Key5; }
        }

        public static string Key6
        {
            get { return _Key6; }
        }
        public static string Key7
        {
            get { return _Key7; }
        }
        public static string Key8
        {
            get { return _Key8; }
        }
        public static string Key9
        {
            get { return _Key9; }
        }
        public static string Key0
        {
            get { return _Key0; }
        }
        public static string KeyStar
        {
            get { return _KeyStar; }
        }
        public static string KeyHash
        {
            get { return _KeyHash; }
        }
        public static string None
        {
            get
            {
                return null;
            }
        }

    }

}
