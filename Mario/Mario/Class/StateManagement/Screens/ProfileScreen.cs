#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mario;
using GObject;
using Animated;
#endregion



namespace NetworkStateManagement
{
    class ProfileScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont Font;

        Vector2 PosScreen = new Vector2(-2,0);
        Vector2 PosScreen2 = new Vector2(500, 0);

        GameObject Avatar;
        GameObject Lives;
        GameObject Life;
        GameObject Apple;
        GameObject Ruby;
        GameObject Ammunition;       

        #endregion

        #region Initialization

        public ProfileScreen()
        {

        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Bin");
            Font = content.Load<SpriteFont>("Font\\Arial14");

            String str = "Textures\\Game Elements\\Bonus\\";
            Texture2D LivesTexture = content.Load<Texture2D>(str + "men");
            Texture2D LifeTexture = content.Load<Texture2D>(str + "heart");
            Texture2D AppleTexture = content.Load<Texture2D>(str + "apple");
            Texture2D RubyTexture = content.Load<Texture2D>(str + "ruby");
            Texture2D AmmunitionTexture = content.Load<Texture2D>("Textures\\a_Persons\\ammunition");
           
            Lives = new GameObject(LivesTexture);
            Life = new GameObject(LifeTexture);
            Apple = new GameObject(AppleTexture);
            Ruby = new GameObject(RubyTexture);
            Ammunition = new GameObject(AmmunitionTexture);          

            ScreenManager.Game.ResetElapsedTime();
        }

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
           
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Avatar.Sprite = Game1.hero.idle;            

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        void drawSceenPrf(SpriteBatch spriteBatch, Vector2 _PosScreen, AnimatedSprite _hero)
        {
            Avatar = new GameObject(_hero.idle);
            Avatar.rect = new Rectangle(0 + (int)_PosScreen.X, 0 + (int)_PosScreen.Y, 73, 73);            
            Apple.rect = new Rectangle(71 + (int)_PosScreen.X, 50 + (int)_PosScreen.Y,23, 23);
            Ruby.rect = new Rectangle(156 + (int)_PosScreen.X, 50 + (int)_PosScreen.Y, 23, 23);
            Ammunition.rect = new Rectangle(62 + (int)_PosScreen.X, 78 + (int)_PosScreen.Y, 40, 40);  

            Avatar.Draw(spriteBatch);
            for (int i = 0; i < _hero.AmountProfile.Lives; i++)
            {
                Lives.rect = new Rectangle(73 + i * 27 + (int)_PosScreen.X, 10 + (int)_PosScreen.Y, 20, 25);
                Lives.Draw(spriteBatch);
            }
            if(_hero.AmountProfile.Lives > -1)
              for (int i = 0; i < _hero.AmountProfile.Life; i++)
              {
                  Life.rect = new Rectangle(64 + 27 * (_hero.AmountProfile.Lives - 1) + 10 + (i + 1) * 28 +
                                                    (int)_PosScreen.X, 15 + (int)_PosScreen.Y, 17, 17);
                  Life.Draw(spriteBatch);
               }

            Apple.Draw(spriteBatch);
            spriteBatch.DrawString(Font, " x" + _hero.AmountProfile.Apple,
                             new Vector2(Apple.rect.X + Apple.rect.Height + 3, Apple.rect.Y + 3), Color.Red);

            Ruby.Draw(spriteBatch);
            spriteBatch.DrawString(Font, " x" + _hero.AmountProfile.Ruby,
                             new Vector2(Ruby.rect.X + Ruby.rect.Height + 3, Ruby.rect.Y + 3), Color.Red);


            spriteBatch.Draw(Ammunition.Sprite, Ammunition.rect, new Rectangle(0, 0, 79, 70),
                              Color.AliceBlue, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(Font, " x" + _hero.AmountProfile.ammunition,
                       new Vector2(Ammunition.rect.X + Ammunition.rect.Height - 3, Ammunition.rect.Y + 3), Color.Red);

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;        

            spriteBatch.Begin();

            drawSceenPrf(spriteBatch, PosScreen, Game1.hero);
            if(Game1.isTwoPlayers) drawSceenPrf(spriteBatch, PosScreen2, Game1.hero2);           

            spriteBatch.End();
        }

        #endregion
    }
}
