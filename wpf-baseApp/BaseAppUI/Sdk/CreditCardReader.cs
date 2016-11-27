using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Sdk
{
    public class CreditCardReader
    {
        private PosExplorer myExplorer;
        private Msr myMsr;







        /// <summary>
        /// Connect to Msr Device and Get Card Data.
        /// </summary>
        public void Connect()
        {

            myExplorer = new PosExplorer();


            DeviceInfo device = myExplorer.GetDevice("Msr", "MagTekMSR_Encrypted");

           
           //device= myExplorer.GetDevices()[0];

            /// <summary>
            /// To check if device found or not
            /// </summary>

            if (device != null)
            {

                Disconnect();

                myMsr = (Msr)myExplorer.CreateInstance(device);

                try
                {
                    myMsr.Open();
                    if (myMsr.Claimed == false)
                    {
                        myMsr.Claim(1000);
                    }
                    myMsr.DeviceEnabled = true;
                }
                catch
                {


                }
                myMsr.DataEventEnabled = true;
                myMsr.DecodeData = true;
                myMsr.DataEvent += new DataEventHandler(myMsr_DataEvent);
            }
            else
            {
                throw new Exception("pos device not founded");

                //return;

            }
            myExplorer.DeviceAddedEvent += new DeviceChangedEventHandler(myExplorer_DeviceAddedEvent);
            myExplorer.DeviceRemovedEvent += new DeviceChangedEventHandler(myExplorer_DeviceRemovedEvent);
            //myMsr.DataEvent += new DataEventHandler(myMsr_DataEvent);
        }

        public void Disconnect()
        {
            if (myMsr != null)
            {
                try
                {
                    myMsr.Close();
                }
                catch (PosException)
                {

                }
                finally
                {
                    myMsr = null;


                }
            }
        }

        /// <summary>
        /// This is the Main Function to read Card Number. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myMsr_DataEvent(object sender, DataEventArgs e)
        {

            OnCardReadAction(myMsr);

            myMsr.DataEventEnabled = true;
            myMsr.DeviceEnabled = true;


        }

        void myExplorer_DeviceAddedEvent(object sender, DeviceChangedEventArgs e)
        {

            if (e.Device.Type == "Msr")
            {
                myMsr = (Msr)myExplorer.CreateInstance(e.Device);

                myMsr.Open();
                myMsr.Claim(1000);
                myMsr.DeviceEnabled = true;
                myMsr.DataEventEnabled = true;
                myMsr.DecodeData = true;

                //  Update();
                myMsr.DataEvent += new DataEventHandler(myMsr_DataEvent);
            }

        }
        void myExplorer_DeviceRemovedEvent(object sender, DeviceChangedEventArgs e)
        {
            if (e.Device.Type == "Msr")
            {
                try
                {
                    myMsr.DataEventEnabled = false;
                    myMsr.DeviceEnabled = false;
                    myMsr.Release();
                    myMsr.Close();
                    myMsr = null;
                }
                catch
                {

                }
                finally
                {
                    myMsr = null;

                }


            }
        }

        public event Action<Msr> OnCardReadEvent = null;
        private void OnCardReadAction(Msr myMsr)
        {
            if (OnCardReadEvent != null)
                OnCardReadEvent(myMsr);
        }
    }
}
