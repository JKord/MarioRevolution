#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mario;
using GObject;
#endregion

namespace NetworkStateManagement
{    
    class MsgBoxStatisticScreen : GameScreen
    {
        #region Fields

        Vector2 Position = new Vector2(300, 200);
        Vector2 Position2 = new Vector2(500, 200);

        string message;
        string message2;

        Texture2D gradientTexture;

        SpriteFont Font;
        SpriteFont smallFont;

        GameObject Apple;
        GameObject Ruby;

        AmountStatistic tmpProfile;
        AmountStatistic tmpProfile2;

        public GOEffect effect;

        Random rand = new Random();
        
        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public MsgBoxStatisticScreen()            
        {
            MediaPlayer.Play(Game1.songLiberty.winner);

            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;

            Game1.hero.AmountProfile.Points += Game1.hero.AmountProfile.kilGums * 50 +
                                                    Game1.hero.AmountProfile.Apple * 10 +
                                                    Game1.hero.AmountProfile.Ruby * 30 -
                                                    Game1.hero.AmountProfile.Death * 8 -
                                                    Game1.hero.AmountProfile.Points;
            tmpProfile = Game1.hero.AmountProfile;
            if (Game1.isTwoPlayers)
            {
                Game1.hero2.AmountProfile.Points += Game1.hero2.AmountProfile.kilGums * 50 +
                                                    Game1.hero2.AmountProfile.Apple * 10 +
                                                    Game1.hero2.AmountProfile.Ruby * 30 -
                                                    Game1.hero2.AmountProfile.Death * 8 -
                                                    Game1.hero2.AmountProfile.Points;
                tmpProfile2 = Game1.hero2.AmountProfile;
            }
        }    
        public MsgBoxStatisticScreen(string message): this(message, true)
        {
            MediaPlayer.Play(Game1.songLiberty.winner);

            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;
        }
        public MsgBoxStatisticScreen(string message, bool includeUsageText)
        {
            if (includeUsageText)
                this.message = message + Mario.Resource.MessageBoxUsage;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            Font = content.Load<SpriteFont>("Font\\Arial18");
            smallFont = content.Load<SpriteFont>("Font\\Arial14");

            gradientTexture = content.Load<Texture2D>("Textures\\menu\\gradient");

            String str = "Textures\\Game Elements\\Bonus\\";            
            Texture2D AppleTexture = content.Load<Texture2D>(str + "apple");
            Texture2D RubyTexture = content.Load<Texture2D>(str + "ruby");         
                        
            Ruby = new GameObject(RubyTexture);
            Apple = new GameObject(AppleTexture);

            effect = new GOEffect(Game1.graphics, content);
            effect.Load("Effects\\flowerBloom");            
            
        }


        #endregion

        #region Handle Input

        void AcceptedEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (Game1.maxLevel == Game1.level.currentLevel)
            {
                Game1.level.currentLevel = 1;
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());                                   
                ScreenManager.AddScreen(new DevelopersScreen(Mario.Resource.DevelopersText),null);
                MediaPlayer.Play(Game1.songLiberty.background);
                Game1.level.Destroy();
            }
            else
            {
                Game1.level.currentLevel++;
                Game1.level.CreateLevel();
                Game1.isPaused = false;
            }

            Game1.ScrollX = 0;            
            Game1.hero.rect = Game1.hero.rectStartUP;
            Game1.hero2.rect = Game1.hero2.rectStartUP;
            if (!Game1.hero.Visible)
            {
                Game1.hero.AmountProfile.Life = 1;
                Game1.hero.AmountProfile.Lives = Game1.hero.AmountProfile.maxLives;
            }
            if (!Game1.hero2.Visible)
            {
                Game1.hero2.AmountProfile.Life = 1;
                Game1.hero2.AmountProfile.Lives = Game1.hero2.AmountProfile.maxLives;
            }
            Game1.hero.Visible = true;
            Game1.hero2.Visible = true;  
        }
        void CancelledEntrySelected(object sender, PlayerIndexEventArgs e)
        {
                      
        }
        
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
        }


        #endregion

        #region Update and Draw

        int timeEffect = 40;
        public void Update(GameTime gameTime)
        {
            message =   "Кiлькiсть вбитих: " + tmpProfile.kilGums +
                      "\nКiлькiсть смертей: " + tmpProfile.Death;
            if (Game1.isTwoPlayers)
            {
                message2 = "Кiлькiсть вбитих: " + tmpProfile2.kilGums +
                          "\nКiлькiсть смертей: " + tmpProfile2.Death;
            }

            if (timeEffect < 0) effect.trigger = new Vector2(rand.Next(800), rand.Next(600));
            effect.Update(gameTime);
            timeEffect--;
            if (timeEffect < -1) timeEffect = 40;
        }

        void drawStatisticEl(Vector2 poz,AmountStatistic profile,string mess, SpriteBatch spriteBatch,Color color)
        {
            SpriteFont font = ScreenManager.Font;            
            Vector2 textSize = font.MeasureString(mess);

           // spriteBatch.DrawString(Font, mess, poz, color);
            
            spriteBatch.DrawString(Font, mess, poz, color);

            Apple.rect = new Rectangle((int)poz.X + 5, (int)(poz.Y + textSize.Y - 10), 27, 27);
            Apple.Draw(spriteBatch);
            spriteBatch.DrawString(Font, " x" + profile.Apple,
                                   new Vector2(Apple.rect.X + 24, Apple.rect.Y), color);

            Ruby.rect = new Rectangle(Apple.rect.X + 94, Apple.rect.Y, 27, 27);
            Ruby.Draw(spriteBatch);
            spriteBatch.DrawString(Font, " x" + profile.Ruby,
                                   new Vector2(Ruby.rect.X + 24, Ruby.rect.Y), color);

            spriteBatch.DrawString(Font, "Набрано балiв: " + profile.Points,
                                   new Vector2(Apple.rect.X - 10, Apple.rect.Y + 35), color);
            effect.Draw();

            spriteBatch.DrawString(smallFont, "Space, Enter = Далi", new Vector2(588, 545), Color.AliceBlue);

        }

        public override void Draw(GameTime gameTime)
        {
            Update(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
           
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 6);           
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Draw the background rectangle.
            //spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            if (Game1.maxLevel == Game1.level.currentLevel)
            {
                spriteBatch.DrawString(Font, Mario.Resource.GameFinished,
                         new Vector2(270, 120), color);
            }
             else 
                spriteBatch.DrawString(Font, "Рiвень "+ Game1.level.currentLevel +" пройдено!", 
                         new Vector2(290,120), color);
            spriteBatch.DrawString(smallFont, "Результати:", new Vector2(337, 156), color);

            
            if (Game1.isTwoPlayers)
            {
                drawStatisticEl(Position2,tmpProfile2,message2, spriteBatch, color);
                Position = new Vector2(80, 200);
            }
            drawStatisticEl(Position,tmpProfile ,message, spriteBatch, color);

            spriteBatch.End();
        }


        #endregion       
    }
}
