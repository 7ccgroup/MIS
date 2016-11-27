using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Sdk
{
    // Amjad 6/1 
    // Integrate Elo SDK for CashDrawer
    public class EloCashDrawer
    {

        [DllImport("toggle_cd.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _tmain();

        private const int CASH_DRAWER_LOW_PORT = 0;
        private const int CASH_DRAWER_STATUS_PORT = 1;
        private const int CASH_DRAWER_HIGH_PORT = 2;
        private const int CASH_DRAWER_RESERVED_PORT = 3;


        public static int Open()
        {

            int result = _tmain();

            return result;
        }

    }
}
