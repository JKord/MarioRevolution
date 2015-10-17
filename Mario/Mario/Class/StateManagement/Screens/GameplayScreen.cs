#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace NetworkStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields    

        ContentManager content;
        SpriteFont gameFont;   

        #endregion

        #region Initialization

        public GameplayScreen()
        {
                             
        }
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //gameFont = content.Load<SpriteFont>("Font\\gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.          
           // Thread.Sleep(1000);
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }        
        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }        
        #endregion

        KeyboardState oldState;
        public override void HandleInput(InputState input)
        {
            KeyboardState keyboardState = Keyboard.GetState();           
            if (keyboardState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
                ScreenManager.AddScreen(new PauseMenuScreen(), null);
            oldState = keyboardState;
        }    

        #region Update and Draw
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);            
                 
        }
       
        public override void Draw(GameTime gameTime)
        {
            

         }


        #endregion
    }
}
