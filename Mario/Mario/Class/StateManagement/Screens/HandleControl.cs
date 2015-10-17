#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mario;
using GObject;
#endregion


namespace NetworkStateManagement
{
    class HandleControl : GameScreen
    {
        #region Fields    

        ContentManager content;
        GameObject Control;

        SpriteFont SmallFont;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public HandleControl() 
        {
            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;
        }       
        
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Bin");

            Control = new GameObject(content.Load<Texture2D>("Textures\\menu\\control"),
                     new Rectangle(30, 100, 755, 368));

            SmallFont = content.Load<SpriteFont>("Font\\Arial14");      
           
            ScreenManager.Game.ResetElapsedTime();
        }     
       
        public override void UnloadContent()
        {
            content.Unload();
        }        
        #endregion

        #region Handle Input

      
        void AcceptedEntrySelected(object sender, PlayerIndexEventArgs e)
        {            
             ExitScreen();
        }
        void CancelledEntrySelected(object sender, PlayerIndexEventArgs e)
        {           
            ExitScreen();
        }

        KeyboardState oldState;
        public override void HandleInput(InputState input)
        {
            KeyboardState keyboardState = Keyboard.GetState();

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
           

            oldState = keyboardState;
        }
        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
           
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.FadeBackBufferToBlack(0.73f); 

            spriteBatch.Begin();

            Control.Draw(spriteBatch);

            spriteBatch.DrawString(SmallFont, "Esc,Space,Enter - " + Mario.Resource.Back, new Vector2(560, 560), Color.AliceBlue);

            spriteBatch.End();            
        }

        #endregion
    }


}
