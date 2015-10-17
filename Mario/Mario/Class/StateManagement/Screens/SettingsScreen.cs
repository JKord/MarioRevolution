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
    class SettingsScreen : MenuScreen
    {
        ContentManager content;
        Vector2 position = new Vector2(250,120);

        #region Initialization

        public SettingsScreen(): base(Mario.Resource.Settings) 
        {           
            // Create our menu entries. 
             MenuEntry fullScreenVideoMenuEntry;
            if (Game1.graphics.IsFullScreen)
               fullScreenVideoMenuEntry = new MenuEntry(Mario.Resource.smallScreenSettings);
             else
               fullScreenVideoMenuEntry = new MenuEntry(Mario.Resource.fullScreenSettings);

            MenuEntry musicSoundMenuEntry = new MenuEntry(Mario.Resource.musicSoundSettings);
            MenuEntry effectSoundMenuEntry = new MenuEntry(Mario.Resource.effectSoundSettings);
            MenuEntry HandleControlMenuEntry = new MenuEntry(Mario.Resource.HandleControlSettings);
            MenuEntry ConfirmMenuEntry = new MenuEntry(Mario.Resource.Confirm);
            MenuEntry BackMenuEntry = new MenuEntry(Mario.Resource.Back);

            // Menu entries Position
            fullScreenVideoMenuEntry.Position = new Vector2(position.X + 60, position.Y + 55);
            musicSoundMenuEntry.Position = new Vector2(position.X + 60, position.Y + 120);
            effectSoundMenuEntry.Position = new Vector2(position.X + 60, position.Y + 160);
            HandleControlMenuEntry.Position = new Vector2(position.X + 60, position.Y + 230);
            ConfirmMenuEntry.Position = new Vector2(310, 480);
            BackMenuEntry.Position = new Vector2(650,550);
           
          
            // Hook up menu event handlers.
            fullScreenVideoMenuEntry.Selected += fullScreenVideoMenuEntrySelected;
            musicSoundMenuEntry.Selected += musicSoundMenuEntrySelected;
            effectSoundMenuEntry.Selected += effectSoundEntrySelected;
            HandleControlMenuEntry.Selected += HandleControlMenuEntrySelected;
            ConfirmMenuEntry.Selected += ConfirmMenuEntrySelected; 
            BackMenuEntry.Selected += BackMenuEntrySelected;              

            // Add entries to the menu.
            MenuEntries.Add(fullScreenVideoMenuEntry);
            MenuEntries.Add(musicSoundMenuEntry);
            MenuEntries.Add(effectSoundMenuEntry);
            MenuEntries.Add(HandleControlMenuEntry);
            MenuEntries.Add(ConfirmMenuEntry); 
            MenuEntries.Add(BackMenuEntry);     
         }         
 
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
                         
            ScreenManager.Game.ResetElapsedTime();           
        }

        #endregion

        #region Handle Input Selected Menu Entry

        void fullScreenVideoMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (!Game1.graphics.IsFullScreen)
                MenuEntries[0].Text = Mario.Resource.smallScreenSettings;
             else
                MenuEntries[0].Text = Mario.Resource.fullScreenSettings;
            Game1.graphics.ToggleFullScreen();      
        }
        void musicSoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
                   
        }

        void effectSoundEntrySelected(object sender, PlayerIndexEventArgs e)
        {           
            
        }
        void HandleControlMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {            
            ScreenManager.AddScreen(new HandleControl(), e.PlayerIndex);           
        }
        void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Volume = Game1.songLiberty.VolumeMusic;
            Game1.SetSetting.SettingSetToFile();
            ExitScreen();
        }  
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }  
        #endregion

        #region Handle Input
        /// <summary>
        /// Відповідає на призначений для користувача введення, зміни обраного елемента і прийняття
        /// Або скасування меню.
        /// </summary> 
        KeyboardState oldState;
        public override void HandleInput(InputState input)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                switch (selectedEntry)
                {
                    case 1: Game1.songLiberty.VolumeMusic += 0.05000f; break;
                    case 2:
                        {
                            Game1.songLiberty.click.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                            Game1.songLiberty.VolumeEffect += 0.05000f;                             
                        } break;                     
                }                
            }
            if (keyboardState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                switch (selectedEntry)
                {
                    case 1: Game1.songLiberty.VolumeMusic -= 0.05000f; break;
                    case 2:
                        {
                            Game1.songLiberty.click.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                            Game1.songLiberty.VolumeEffect -= 0.05000f;                           
                        } break;                       
                }
            }
            MediaPlayer.Volume = Game1.songLiberty.VolumeMusic;
            oldState = keyboardState;
            // Перейти до попереднього пункту меню?
            if (input.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }
            // Перейти до наступного пункту меню?
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
                ExitScreen();
            }
        }     

        #endregion

        #region Update and Draw
        /// <summary>
        /// Дозволяє екрану можливість позиції в меню запису. За замовчуванням
        /// Усі пункти меню вишикувалися у вигляді вертикального списку, по центру екрану.
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
        /// Оновлення меню.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,  bool coveredByOtherScreen)
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
        /// Малює меню ..
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Переконайтеся, що наші записи знаходяться в потрібному місці, перш ніж ми залучити їх
            //UpdateMenuEntryLocations();           

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 3 / 6); 
            SpriteFont font = ScreenManager.Font;

            if (Game1.songLiberty.VolumeMusic > 1) Game1.songLiberty.VolumeMusic = 1;
            if (Game1.songLiberty.VolumeMusic < 0) Game1.songLiberty.VolumeMusic = 0;
            if (Game1.songLiberty.VolumeEffect > 1) Game1.songLiberty.VolumeEffect = 1;            
            if (Game1.songLiberty.VolumeEffect < 0) Game1.songLiberty.VolumeEffect = 0;

            spriteBatch.Begin();          

            spriteBatch.DrawString(font,Mario.Resource.VideoSettings,position, Color.AliceBlue);
            spriteBatch.DrawString(font, Mario.Resource.SoundSettings, 
                        new Vector2(position.X, position.Y + 70), Color.AliceBlue);
            spriteBatch.DrawString(font, Mario.Resource.ControlSettings,
                        new Vector2(position.X, position.Y + 175), Color.AliceBlue);

            menuEntries[1].Text = Mario.Resource.musicSoundSettings + "  " +
                                  (int) (Game1.songLiberty.VolumeMusic * 100) + "%";
            menuEntries[2].Text = Mario.Resource.effectSoundSettings + "  " +
                                  (int) (Game1.songLiberty.VolumeEffect * 100) + "%";

            // Малює кожну пункту меню, в свою чергу.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }           
            // Створити слайд меню на місце під час переходів, використовуючи
            // Криву потужності, щоб все виглядало більш цікавим (це робить
            // Рух сповільнюється в міру наближення кінця).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Намалюйте меню назва по центру екрану
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
