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
    class ElectoralCharacter : GameScreen
    {
        #region Fields    

        ContentManager content;
        SpriteFont Font;
        SpriteFont smallFont;

        PreferredHW PositionScreen = new PreferredHW() { Height = 0, Width = 0 };

        int heroChoice;

        GameObject ChoiceTex;
        GameObject Ramka;

        GameObject Lives;
        GameObject Life;
        GameObject Apple;
        GameObject Ammunition;

        bool isHandleInput;
        int timePauseHandle = 30;

        int chPlayer = 1;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public ElectoralCharacter()
        {
            Game1.hero.Visible = true;
            Game1.hero2.Visible = true;

            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;
        }

        public ElectoralCharacter(int _chPlayer)
        {
            chPlayer = _chPlayer;

            Game1.hero.Visible = true;
            Game1.hero2.Visible = true;

            Accepted += AcceptedEntrySelected;
            Cancelled += CancelledEntrySelected;
        }    
        
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Bin");
            Font = content.Load<SpriteFont>("Font\\Arial18");
            smallFont = content.Load<SpriteFont>("Font\\Arial14");

            Ramka = new GameObject(content.Load<Texture2D>("Textures\\menu\\ramka"), new Rectangle(0, 0, 800, 600));
            ChoiceTex = new GameObject(Game1.spritePers[Game1.heroChoice].idle, new Rectangle(180, 150, 190, 200));           

            String str = "Textures\\Game Elements\\Bonus\\";
            Texture2D LivesTexture = content.Load<Texture2D>(str + "men");
            Texture2D LifeTexture = content.Load<Texture2D>(str + "heart");
            Texture2D AppleTexture = content.Load<Texture2D>(str + "apple");
            Texture2D AmmunitionTexture = content.Load<Texture2D>("Textures\\a_Persons\\ammunition");
         
            Lives = new GameObject(LivesTexture);
            Life = new GameObject(LifeTexture);
            Apple = new GameObject(AppleTexture);
            Ammunition = new GameObject(AmmunitionTexture);            
           
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
            
            if (chPlayer == 1) Game1.heroChoice = heroChoice;
             else
              {
                  Game1.heroChoice2 = heroChoice;
                  LoadingScreen.Load(ScreenManager, true, null);
                  ScreenManager.AddScreen(new ProfileScreen(), null);
                  Mario.Game1.isStart = true;
              }
            if (!Game1.isTwoPlayers)
            {
                LoadingScreen.Load(ScreenManager, true, null);
                ScreenManager.AddScreen(new ProfileScreen(), null);
                Mario.Game1.isStart = true;
            }
            if (Game1.isTwoPlayers && chPlayer == 1) ScreenManager.AddScreen(new ElectoralCharacter(2), e.PlayerIndex);
            ExitScreen();
        }
        void CancelledEntrySelected(object sender, PlayerIndexEventArgs e)
        {

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

            if (keyboardState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
                heroChoice++;
            if (keyboardState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
                heroChoice--;
            if (heroChoice > Game1.maxHeroChoice) heroChoice = 0;
            if (heroChoice < 0) heroChoice = Game1.maxHeroChoice;

            if (chPlayer == 1) Game1.heroChoice = heroChoice;
            else Game1.heroChoice2 = heroChoice;

            oldState = keyboardState;
        }
        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            ChoiceTex.Sprite = Game1.spritePers[heroChoice].idle;
            if (chPlayer == 1) Game1.ChangePersTexture(Game1.hero,Game1.heroChoice);
            else Game1.ChangePersTexture(Game1.hero2,Game1.heroChoice2);
            
            base.Update(gameTime, otherScreenHasFocus, false);

            if(timePauseHandle < 0 && !isHandleInput) isHandleInput = true;
            timePauseHandle--;
        }

        int ColorPr = 0;
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 6);
            spriteBatch.Begin();

            if (ColorPr < 150)
                ColorPr += 3;
            spriteBatch.Draw(Ramka.Sprite, Ramka.rect, new Color(ColorPr/2, ColorPr, ColorPr));
            ChoiceTex.Draw(spriteBatch);

            if(Game1.isTwoPlayers)
                spriteBatch.DrawString(Font, Mario.Resource.ChoiceOfHero + "  " + chPlayer,
                                       new Vector2(185,65), Color.AliceBlue);

            PositionScreen = new PreferredHW() { Height = ChoiceTex.rect.X + 270, Width = ChoiceTex.rect.Y };

            spriteBatch.DrawString(Font, Mario.Resource.Name + "  " + Game1.ListAmountProfile[heroChoice].type,
                                  new Vector2(PositionScreen.Height, PositionScreen.Width + 12), Color.AliceBlue);

            spriteBatch.DrawString(Font, Mario.Resource.Deaths, new Vector2(PositionScreen.Height, 
                                    PositionScreen.Width + 43), Color.AliceBlue);
            for (int i = 0; i < Game1.ListAmountProfile[heroChoice].maxLives; i++)
            {
                Lives.rect = new Rectangle(88 + i * 27 + PositionScreen.Height,
                                           PositionScreen.Width + 42, 20, 25);
                Lives.Draw(spriteBatch);
            }

            spriteBatch.DrawString(Font, Mario.Resource.Health, new Vector2(PositionScreen.Height,
                                   PositionScreen.Width + 75), Color.AliceBlue);
            for (int i = 0; i < Game1.ListAmountProfile[heroChoice].maxLife; i++)
            {
                Life.rect = new Rectangle(84 + (i + 1) * 28 + PositionScreen.Height,
                                          Lives.rect.Y + 40, 17, 17);
                Life.Draw(spriteBatch);
            }

            spriteBatch.DrawString(Font, Mario.Resource.Ammunition, new Vector2(PositionScreen.Height,
                                    PositionScreen.Width + 108), Color.AliceBlue);
            Ammunition.rect = new Rectangle(PositionScreen.Height + 143, PositionScreen.Width + 108, 40, 40);
            spriteBatch.Draw(Ammunition.Sprite, Ammunition.rect, new Rectangle(0, 0, 79, 70),
                              Color.AliceBlue, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(Font, " x" + Game1.ListAmountProfile[heroChoice].ammunition, 
                       new Vector2(Ammunition.rect.X + 36, Ammunition.rect.Y), Color.AliceBlue);

            spriteBatch.DrawString(smallFont,Mario.Resource.Character , new Vector2(550,530), Color.AliceBlue);

            spriteBatch.End();            
        }

        #endregion
    }
}
  
    