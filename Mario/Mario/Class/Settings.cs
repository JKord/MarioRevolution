#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Text;
using System.IO;
using Mario;
#endregion

namespace Settings
{
   public class Setting
    {
        #region Fields

        String[] text;
        int poch = 0;

        #endregion

        #region Initialization

        public Setting()
        {

        }
        #endregion

        #region Methods Convert To Words

        int amountWord(String str)
        {
            int count = 0;
            foreach (char c in str)
              if (c == ' ')
                count++;
            return count;
        }

        String ToWord(String str)
        {
            String word = "";
            for (int i = poch; i < str.Length; i++)
            {
                if (str[i] == ' ')
                {
                    poch = i + 1;
                    break;
                }
                word += str[i];
            }            
            return word;
        }

        String[] convertToWords(String str)
        {
            String[] strWords = new String[30];
            for (int i = 0; i < amountWord(str); i++)
              strWords[i] = ToWord(str);               
            poch = 0;
            return strWords;
        }

        #endregion

        #region Setting File Read & Write

        void ReadAmountStatistic(String []str,int n)
        {
            Game1.ListAmountProfile[n].type = str[2];
            Game1.ListAmountProfile[n].Apple = Convert.ToInt32(str[4]);
            Game1.ListAmountProfile[n].Ruby = Convert.ToInt32(str[6]);
            Game1.ListAmountProfile[n].Life = Convert.ToInt32(str[8]);
            Game1.ListAmountProfile[n].Lives = Convert.ToInt32(str[10]);
            Game1.ListAmountProfile[n].ammunition = Convert.ToInt32(str[12]);
            Game1.ListAmountProfile[n].maxLife = Convert.ToInt32(str[14]);
            Game1.ListAmountProfile[n].maxLives = Convert.ToInt32(str[17]);
        }
        void SetGumSetting(int n, String[] strWords)
        {
            Game1.ListGums[n].Lives = Convert.ToInt32(strWords[2]);
            Game1.ListGums[n].speed = Convert.ToInt32(strWords[4]);
        }

        public void SetSettingFromFile()
        {
            text = File.ReadAllLines("Bin\\Setting.ini");
            
            #region Setting 
            foreach (String str in text)
            {
                String[] strWords = new String[30];
                strWords = convertToWords(str);
                switch (strWords[0])
                {
                    case "isFullScreen=":
                        {
                            if (Convert.ToBoolean(strWords[1]))
                              Game1.graphics.ToggleFullScreen();                             
                        } break;
                    case "VolumeMusic=":
                        {
                            Game1.songLiberty.VolumeMusic = MediaPlayer.Volume = (float)Convert.ToDouble(strWords[1]);
                        } break;
                    case "VolumeEffect=":
                        {
                            Game1.songLiberty.VolumeEffect = (float)Convert.ToDouble(strWords[1]);
                        } break;  
                   }               
               }  
              #endregion

            #region Game Playing
            text = File.ReadAllLines("Bin\\Game Playing.ini");

            foreach (String str in text)
            {
                String[] strWords = new String[30];
                strWords = convertToWords(str);
                switch (strWords[0])
                {
                    case "[AmountStatistic0]":
                        {
                            ReadAmountStatistic(strWords, 0);
                        } break;
                    case "[AmountStatistic1]":
                        {
                            ReadAmountStatistic(strWords, 1);
                        } break;
                    case "[AmountStatistic2]":
                        {
                            ReadAmountStatistic(strWords, 2);
                        } break;
                    case "[AmountStatistic3]":
                        {
                            ReadAmountStatistic(strWords, 3);
                        } break;
                    case "[AmountStatistic4]":
                        {
                            ReadAmountStatistic(strWords, 4);
                        } break;
                    case "[AmountStatistic5]":
                        {
                            ReadAmountStatistic(strWords, 5);
                        } break;
                    case "[menper]":
                        {
                            SetGumSetting(0, strWords); 
                        } break;
                    case "[robot]":
                        {
                            SetGumSetting(1, strWords);
                        } break;
                    case "[monkey]":
                        {
                            SetGumSetting(2, strWords);
                        } break;
                    case "[hand]":
                        {
                            SetGumSetting(3, strWords);
                        } break;
                    case "[rock]":
                        {
                            SetGumSetting(4, strWords);
                        } break;
                    case "maxLevel=":
                        {
                            Game1.maxLevel = Convert.ToInt32(strWords[1]);
                        } break;
                }
             }
             #endregion
        } 

        public void SettingSetToFile()
        {
            String []str = new String [5];
            str[0] = "[Video] ";
            str[1] = "isFullScreen= ";
            if (Game1.graphics.IsFullScreen) str[1] += "true ";
              else str[1] += "false ";
            str[2] = "[Sound] ";
            str[3] = "VolumeMusic= " + Game1.songLiberty.VolumeMusic + " ";
            str[4] = "VolumeEffect= " + Game1.songLiberty.VolumeEffect + " ";

            File.WriteAllLines("Bin\\Setting.ini",str);
        }

        #endregion
    }
        
}
