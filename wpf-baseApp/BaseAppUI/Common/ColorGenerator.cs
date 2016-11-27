using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace BaseAppUI.Common
{
    public class ColorGenerator
    {
        private  int index;
      
        public SolidColorBrush GetNextBrush()
        {
            SolidColorBrush brush = new SolidColorBrush(GetNext());
          
            brush.Freeze();

            return brush;
        }

      
        public Color GetNext()
        {

            byte[] colorBytes = RgbColors[index];

            if (index == (RgbColors.Count - 1))
                Reset();
            else index++;
         


            Color color = new Color();

            color.A = 255;
            color.R = colorBytes[0];
            color.G = colorBytes[1];
            color.B = colorBytes[2];

            return color;
        }

        public void Reset()
        {
            index = 0;
        }


        //available colors

        List<byte[]> RgbColors = new List<byte[]> { 
        
            new byte[3]{254,235,0},
            new byte[3]{255,72,83},
              new byte[3]{16,198,219},
            new byte[3]{163,208,123},
              new byte[3]{255,112,118},
               new byte[3]{255,194,20},
                 new byte[3]{84,217,252},
                    new byte[3]{255,118,178},
                      new byte[3]{176,222,0},
                      new byte[3]{153,51,153},
                      new byte[3]{228,92,44},
                       new byte[3]{26,181,90},


                 



        };
       
    }

    
}
