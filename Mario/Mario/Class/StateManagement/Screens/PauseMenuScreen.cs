#region Using Statements
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Mario;
#endregion

namespace NetworkStateManagement
{
    class PauseMenuScreen : MenuScreen
    {
        #region Fields       

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen() : base(Mario.Resource.Paused)
        {          
            MenuEntry resumeGameMenuEntry = new MenuEntry(Mario.Resource.ResumeGame);
            resumeGameMenuEntry.Selected += OnCancel;
            MenuEntries.Add(resumeGameMenuEntry);

            MenuEntry SettingsMenuEntry = new MenuEntry(Mario.Resource.Settings);
            SettingsMenuEntry.Selected += SettingsMenuEntrySelected;
            MenuEntries.Add(SettingsMenuEntry);
       
            MenuEntry quitGameMenuEntry = new MenuEntry(Mario.Resource.QuitGame);
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            MenuEntries.Add(quitGameMenuEntry);

            Game1.isPaused = true;        
        }

        #endregion

        #region Handle Input
        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        /// 
        KeyboardState oldState;
        public override void HandleInput(InputState input)
        {            
            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }
            // Move to the next menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                Game1.isPaused = false;
                ExitScreen();                
            }
        }
        #endregion

        #region Handle Input Selected Menu Entry

        void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            Mario.Game1.isPaused = false;
            ExitScreen();
        }
        void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SettingsScreen(), e.PlayerIndex);
        }   
            
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MessageBoxScreen confirmQuitMessageBox =
                                    new MessageBoxScreen(Mario.Resource.ConfirmQuitGame);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
            MediaPlayer.Play(Game1.songLiberty.background);
            Game1.level.Destroy();
        }
      
        #endregion

        #region Update and Draw
        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected override void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 175f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();           

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 6);
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}
