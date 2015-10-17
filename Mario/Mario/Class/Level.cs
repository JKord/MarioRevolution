#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using NetworkStateManagement;
using GObject;
using System.IO;

using Animated;
using Mario;
#endregion

namespace Levels
{
    public class Level
    {
        #region Fields

        public int currentLevel = 1;       

        public Texture2D[] blockTexture;
        public Texture2D[] bonusTexture;
        public Texture2D[] runGum;        

        public GraphicsDeviceManager graphics;
        public ContentManager Content;

        #endregion

        #region Initialization
        public Level(GraphicsDeviceManager _graphics, ContentManager _Content)
        {
            blockTexture = new Texture2D[5];
            bonusTexture = new Texture2D[7];
            runGum = new Texture2D[6];

            graphics = _graphics;
            Content = _Content;            
        }       

        public void Load(ContentManager Content)
        {
            String str = "Textures\\Game Elements\\";
            blockTexture[0] = Content.Load<Texture2D>(str + "block1");
            blockTexture[1] = Content.Load<Texture2D>(str + "block2");
            blockTexture[2] = Content.Load<Texture2D>(str + "bamboo");
            blockTexture[3] = Content.Load<Texture2D>(str + "bambootopSliceHorizontal");
            blockTexture[4] = Content.Load<Texture2D>(str + "hause1");

            bonusTexture[0] = Content.Load<Texture2D>(str + "Bonus\\apple");
            bonusTexture[1] = Content.Load<Texture2D>(str + "Bonus\\beetle");
            bonusTexture[2] = Content.Load<Texture2D>(str + "Bonus\\hand");
            bonusTexture[3] = Content.Load<Texture2D>(str + "Bonus\\heart");
            bonusTexture[4] = Content.Load<Texture2D>(str + "Bonus\\men");
            bonusTexture[5] = Content.Load<Texture2D>(str + "Bonus\\ruby");
            bonusTexture[6] = Content.Load<Texture2D>(str + "Bonus\\box1");
           
            runGum[0] = Content.Load<Texture2D>(str + "Gum\\menperRun");
            runGum[1] = Content.Load<Texture2D>(str + "Gum\\robotRun");
            runGum[2] = Content.Load<Texture2D>(str + "Gum\\monkeyRun");
            runGum[3] = Content.Load<Texture2D>(str + "Gum\\handGRun");
            runGum[4] = Content.Load<Texture2D>(str + "Gum\\rockRun");
            runGum[5] = Content.Load<Texture2D>(str + "Gum\\ghostRun");
        }

        public void UnloadContent()
        {
            Content.Unload();
        }        
        #endregion

        public void CreateLevel()
        {
            try { MediaPlayer.Play(Game1.songLiberty.musicLevel[currentLevel - 1]); }
             catch (System.Exception ex)
            { MediaPlayer.Play(Game1.songLiberty.musicLevel[0]); }            

            Game1.Elements.blocks = new List<GameObject>();
            Game1.Elements.elemeNotCollides = new List<GameObject>();
            Game1.Elements.bonus = new List<GameObject>();
            Game1.Elements.gums = new List<AnimatedSprite>();

            String st = "Bin\\Levels\\level";          
            if (!File.Exists(st + currentLevel + ".lv")) currentLevel = 1;
            string[] s = File.ReadAllLines(st + currentLevel + ".lv");
            //File.Encrypt(st + currentLevel + ".lv");
            //File.Decrypt(st + currentLevel + ".lv");

            int x = 0;
            int y = 0;
            foreach (string str in s)
            {
                foreach (char c in str)
                {
                    Rectangle rect = new Rectangle(x, y, 40, 40);
                    switch (c)
                    {
                        #region Block
                        case 'X':
                            {
                                GameObject block = new GameObject(blockTexture[0], rect);
                                block.type = "block1";
                                Game1.Elements.blocks.Add(block);
                            } break;
                        case 'Y':
                            {
                                GameObject block = new GameObject(blockTexture[1], rect);
                                block.type = "block2";
                                Game1.Elements.blocks.Add(block);
                            } break;
                        case 'B':
                            {
                                GameObject block = new GameObject(blockTexture[2], new Rectangle(x, y, 41, 184));
                                block.type = "bamboo";
                                Game1.Elements.blocks.Add(block);
                            } break;
                        case 'S':
                            {
                                GameObject block = new GameObject(blockTexture[3], new Rectangle(x, y, 41, 92));
                                block.type = "bambootopSliceHorizontal";
                                Game1.Elements.blocks.Add(block);
                            } break;
                        #endregion
                        #region Bonus
                        case 'a':
                            {
                                GameObject bon = new GameObject(bonusTexture[0], new Rectangle(x, y,
                                                                 rect.Width / 2, rect.Height / 2));
                                bon.type = "apple";
                                Game1.Elements.bonus.Add(bon);
                            } break;

                        case 'b':
                            {
                                GameObject bon = new GameObject(bonusTexture[1], rect);
                                bon.type = "beetle";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        case 'h':
                            {
                                GameObject bon = new GameObject(bonusTexture[2], new Rectangle(x, y,
                                                                 rect.Width, rect.Height / 2));
                                bon.type = "hand";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        case 'c':
                            {
                                GameObject bon = new GameObject(bonusTexture[3], new Rectangle(x, y,
                                                                 rect.Width / 2, rect.Height / 2));
                                bon.type = "heart";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        case 'm':
                            {
                                GameObject bon = new GameObject(bonusTexture[4], new Rectangle(x, y,
                                                                 rect.Width / 2, rect.Height));
                                bon.type = "men";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        case 'r':
                            {
                                GameObject bon = new GameObject(bonusTexture[5], new Rectangle(x, y,
                                                                 rect.Width / 2, rect.Height / 2));
                                bon.type = "ruby";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        case 'A':
                            {
                                GameObject bon = new GameObject(bonusTexture[6], new Rectangle(x, y,
                                                                 rect.Width / 2, rect.Height / 2));
                                bon.type = "box1";
                                Game1.Elements.bonus.Add(bon);
                            } break;
                        #endregion
                        #region Elements not Collides
                        case 'H':
                            {
                                GameObject block = new GameObject(blockTexture[4], new Rectangle(x, y, 280, 220));
                                block.type = "hause1";
                                Game1.Elements.elemeNotCollides.Add(block);
                            } break;
                        #endregion
                        #region Gum
                        case 'M':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[0]);
                                gum.isSlowMode = true;                                
                                gum.AmountProfile.Lives = Game1.ListGums[0].Lives;
                                gum.speed = Game1.ListGums[0].speed;
                                Game1.Elements.gums.Add(gum);
                            } break;
                        case 'R':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[1]);
                                gum.isSlowMode = true;
                                gum.AmountProfile.Lives = Game1.ListGums[1].Lives;
                                gum.speed = Game1.ListGums[1].speed;                             
                                Game1.Elements.gums.Add(gum);
                            } break;
                        case 'G':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[2]);
                                //gum.isSlowMode = true;
                                gum.AmountProfile.Lives = Game1.ListGums[2].Lives;
                                gum.speed = Game1.ListGums[2].speed;
                                Game1.Elements.gums.Add(gum);
                            } break;
                        case 'C':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[3]);
                                gum.isSlowMode = true;
                                gum.AmountProfile.Lives = Game1.ListGums[3].Lives;
                                gum.speed = Game1.ListGums[3].speed;
                                Game1.Elements.gums.Add(gum);
                            } break;
                        case 'K':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[4]);
                                gum.isSlowMode = true;
                                gum.AmountProfile.Lives = Game1.ListGums[4].Lives;
                                gum.speed = Game1.ListGums[4].speed;
                                Game1.Elements.gums.Add(gum);
                            } break;
                        case 'g':
                            {
                                AnimatedSprite gum = new AnimatedSprite(new Rectangle(x, y, 70, 70), runGum[5]);
                              //  gum.isSlowMode = true;
                                gum.AmountProfile.Lives = Game1.ListGums[5].Lives;
                                gum.speed = Game1.ListGums[5].speed;
                                Game1.Elements.gums.Add(gum);
                            } break;

                        #endregion
                    }                 
                    x += 40;
                }
                x = 0;
                y += 40;
            }
         }
         
        #region Update and Draw

        int CollidUpdateLevel = -1;
        public int timeLife = 800;
        public int timeLife2 = 800;     
  
        void LogicGums(GameTime gameTime)
        {
            foreach (AnimatedSprite gum in Game1.Elements.gums)
            {
                if (gum.Visible)
                {
                    if (CollidUpdateLevel != currentLevel)
                        CollidUpdateLevel = currentLevel;
                    gum.isLogicRun = true;
                    gum.Update(gameTime);
                    Rectangle worldRectGum = Game1.GetScreenRect(gum.rect);
                    worldRectGum = new Rectangle(worldRectGum.X, worldRectGum.Y,
                        worldRectGum.Width - worldRectGum.Width % 10, worldRectGum.Height);
                    if (worldRectGum.Intersects(Game1.GetScreenRect(Game1.hero.rect)) && timeLife > 800 && Game1.hero.Visible)
                    {
                        Game1.songLiberty.shot.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                        if (!Game1.hero.isJumping && !Game1.hero.isSmall) Game1.hero.rect.Offset(0, -10);
                        Game1.hero.AmountProfile.Life--;

                        timeLife = 0;
                        Game1.hero.color.A = 0;
                    }
                    if (Game1.hero.color.A < 255 && timeLife % 2 == 0 ) Game1.hero.color.A++;                   
                    timeLife++;
                    if (Game1.isTwoPlayers)
                    {
                        if (worldRectGum.Intersects(Game1.GetScreenRect(Game1.hero2.rect)) && timeLife2 > 800
                                                                   && Game1.hero.Visible && Game1.isTwoPlayers)
                        {
                            Game1.songLiberty.shot.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                            if (!Game1.hero2.isJumping && !Game1.hero2.isSmall) Game1.hero2.rect.Offset(0, -10);
                            Game1.hero2.AmountProfile.Life--;

                            timeLife2 = 0;
                            Game1.hero2.color.A = 0;
                        }
                        if (Game1.hero2.color.A < 255 && timeLife2 % 2 == 0) Game1.hero2.color.A++;
                        timeLife2++;
                    }
                }
            }

        }

        void heroAnew(AnimatedSprite _hero)
        {           
            _hero.rect = _hero.rectStartUP;           
            _hero.countfire = 0;
            _hero.Visible = true;
            if (_hero.isSmall) _hero.doSmall();
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.Elements.elemeNotCollides != null)
            {
                LogicGums(gameTime);
                foreach (GameObject block in Game1.Elements.elemeNotCollides)
                {
                    Rectangle rectBlock = Game1.GetScreenRect(block.rect);
                    Rectangle rectObj = Game1.GetScreenRect(Game1.hero.rect);
                    Rectangle rectObj2 = Game1.GetScreenRect(Game1.hero2.rect);
                    rectBlock.Offset(140, 0);
                    if ((rectBlock.Intersects(rectObj) || rectBlock.Intersects(rectObj2)) && block.type == "hause1")
                    {
                        MsgBoxStatisticScreen cMessageBox = new MsgBoxStatisticScreen();
                        Game1.screenManager.AddScreen(cMessageBox, null);
                        Game1.isPaused = true;
                    }
                }
                if (Game1.hero.AmountProfile.Lives < 0 && !Game1.isTwoPlayers)
                {
                    heroAnew(Game1.hero);
                    Game1.hero.AmountProfile = Game1.ListAmountProfile[Game1.heroChoice];
                    Game1.ScrollX = 0;
                    currentLevel = 1;
                    CreateLevel();
                }
                if (Game1.hero.AmountProfile.Lives < 0 && Game1.hero2.AmountProfile.Lives < 0 && Game1.isTwoPlayers)
                {
                    heroAnew(Game1.hero);
                    heroAnew(Game1.hero2);
                    Game1.hero.AmountProfile = Game1.ListAmountProfile[Game1.heroChoice];
                    Game1.hero2.AmountProfile = Game1.ListAmountProfile[Game1.heroChoice2];
                    Game1.ScrollX = 0;
                    currentLevel = 1;                  
                    CreateLevel();
                }
                if (Game1.hero.AmountProfile.Lives < 0 && Game1.isTwoPlayers) Game1.hero.Visible = false;
                if (Game1.hero2.AmountProfile.Lives < 0 && Game1.isTwoPlayers) Game1.hero2.Visible = false;
                  
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.Elements.elemeNotCollides != null)
            {
                foreach (GameObject block in Game1.Elements.elemeNotCollides)
                {
                    block.DrawWorld(spriteBatch);
                }
                foreach (GameObject block in Game1.Elements.blocks)
                {
                    block.DrawWorld(spriteBatch);
                }
                foreach (GameObject bon in Game1.Elements.bonus)
                {
                    bon.DrawWorld(spriteBatch);
                }
                foreach (AnimatedSprite gum in Game1.Elements.gums)
                {
                    gum.Draw(spriteBatch);
                }
            }      
        }       

        #endregion

        public void Destroy()
        {
            Game1.Elements.blocks = null;
            Game1.Elements.bonus = null;
            Game1.Elements.elemeNotCollides = null;
            Game1.Elements.gums = null;
        }
    }
}
