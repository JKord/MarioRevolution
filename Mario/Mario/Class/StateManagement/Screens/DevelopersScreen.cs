#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace NetworkStateManagement
{
    class DevelopersScreen : GameScreen
    {
        #region Fields

        string message;             
        Vector2 textPosition;
        SpriteFont SmallFont;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public DevelopersScreen(string message): this(message, true)
        { }

        public DevelopersScreen(string message, bool includeUsageText)
        {
            this.message = "            PC DEVELOPMENT TEAM  \n\n" +
                            "PROJECT MANAGER AND PROGRAMMING LEADER  \n" +
                            "               Кордюк Юрiй  \n\n" +
                            "             PROGRAMMING TEAM  \n\n" +                            
                            "               GAME DESIGNER  \n" +
                            "              Ковтюх Вiталiй  \n\n" +
                            "               CREATOR LEVELS  \n" +
                            "               Рузевич Сергiй  \n\n" +
                            "                  SOUNDMAN   \n" +
                            "             Трiщенко Валера  \n\n\n\n\n" +
                            "              Copyright ©  2011";           
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2); 

            textPosition = new Vector2((Mario.Game1.SizeScreen.Height) / 2 - 150, Mario.Game1.SizeScreen.Width - 250);           
        }
       
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;            
            SmallFont = content.Load<SpriteFont>("Font\\Arial14"); 
        }               

        #endregion

        #region Handle Input


        /// <summary>
        /// Реагує на дії користувача, що приймає або скасування вікні повідомлення.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Підніміть відмінено подія, то вихід вікні повідомлення.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));                

                ExitScreen();                
            }
        }

        #endregion

        #region  Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                              bool coveredByOtherScreen)
        {
            textPosition.Y--;
            Vector2 textSize = ScreenManager.Font.MeasureString(message);
            if (textPosition.Y + textSize.Y < 0)
                textPosition.Y = Mario.Game1.SizeScreen.Width - 250;
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Темніше будь-які інші екрани, які були розроблені під спливаючого вікна.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 5 / 6);

            Vector2 textSize = font.MeasureString(message);
            // Послужний список включає кордону дещо більше, ніж сам текст.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);
            // Fade спливаючих альфа під час переходів.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Малюємо прямокутник фону.
           // spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            //Малювання тексту вікні повідомлення.
            spriteBatch.DrawString(font, message, textPosition, color);  

            spriteBatch.DrawString(SmallFont, "Esc - " + Mario.Resource.Back, new Vector2(680, 560), color);
            spriteBatch.End();
        }


        #endregion
    }
}
