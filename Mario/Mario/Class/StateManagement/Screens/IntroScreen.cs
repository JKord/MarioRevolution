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
    class IntroScreen : GameScreen
    {
        #region Fields

        GOVideo video;
        
        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public IntroScreen()
        {
            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            video = new GOVideo(content.Load<Video>("Video\\Intro"), 
                    new Rectangle(0, 0, Game1.SizeScreen.Width, Game1.SizeScreen.Height + 5));           
        }

        #endregion

        #region Handle Input

        void AcceptedEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Exit();
        }
        void CancelledEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Exit();   
        }
        
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Підніміть приймаються події, то вихід вікні повідомлення.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Підніміть відмінено подія, то вихід вікні повідомлення.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {            
            if (video.VideoFile.FramesPerSecond - 2 == gameTime.TotalGameTime.Seconds) video.color.A--;

            video.Update(gameTime);

            if (video.VideoFile.FramesPerSecond + 1 == gameTime.TotalGameTime.Seconds) Exit();
            
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            video.Draw(spriteBatch);

            spriteBatch.End();
        }      

        #endregion   
    
        void Exit()
        {
            video.Dispose();
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(), null);
        }
    }
}