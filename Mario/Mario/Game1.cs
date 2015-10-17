#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Threading;
using System.Media;

using NetworkStateManagement;
using Animated;
using GObject;
using GUI;
using Levels;
using Settings;

#endregion

namespace Mario
{
    #region Struct
    
    // Структура яка відповідає за розмір екрана
    public struct PreferredHW { public int Width, Height; }
    // Структура в якій зберігається анімація і тип героя
    public struct SpriteAnimatedTexture 
    { 
        public Texture2D idle, run, jump;
        public String type;
    }
    // Структура в якій зберігаються звуки
    public struct SongLiberty
    {
        public float VolumeMusic;
        public float VolumeEffect;
        public Song background;
        public Song winner;
        public Song []musicLevel;
        public SoundEffect click, throwChuck, hitGum, shot;
        public SoundEffect hitBlock, catchBonus; 
    }
    // Структура в якій зберігаються елементи гейм плея
    public struct ELEMENTS
    {
        public List<GameObject> blocks;
        public List<GameObject> elemeNotCollides;
        public List<GameObject> bonus;
        public List<AnimatedSprite> gums;
    }
    // Структура в якій зберігаються характеристики ворогів
    public struct StatisticGum
    {
        public int Lives, speed;
    }

    #endregion

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;    
        public static ScreenManager screenManager;   
           
        public static PreferredHW SizeScreen = new PreferredHW() { Width = 800, Height = 600 }; // Розмір екрана

        public static SongLiberty songLiberty = new SongLiberty(); // Бібліотека звуків
        public static ELEMENTS Elements; // Бібліотека елементів гейм плея
        public static Setting SetSetting;

        public static Level level; // Ігровий рівень
        public static bool isTwoPlayers; // Чи два гравці грають?       

        public static AnimatedSprite hero; // Герой №1
        public static int heroChoice = 0; // Вибір ткстури (герой №1)
        public static AnimatedSprite hero2; // Герой №2
        public static int heroChoice2 = 0; // Вибір ткстури (герой №2)
       
       

        public static SpriteAnimatedTexture[] spritePers; // Масив текстур анімації

        public static AmountStatistic[] ListAmountProfile; // Масив характеристик героїв
        public static StatisticGum[] ListGums; // Масив характеристик ворогів

        Texture2D[] background; // Масив текстур фону
        SpriteFont Font; // Шрифт

        public static bool isStart; // Чи гра почалась
        public static bool isPaused; // Чи поуза
        public bool isStartNewGame; // Чи почата нова гра

        public static int ScrollX = 0; // Позиція видимої області екрану

        public static int maxLevel = 5; // Максимальний рівень
        public static int maxHeroChoice = 5; // Максимална кількість текстур героїв      

        int CollidUpdateLevel = -1; // № попереднього рівня 

        #endregion

        #region Methods

        // Установка характеристик героя
        void SetListAmountProfile()
        {
            ListAmountProfile = new AmountStatistic[6];
            ListGums = new StatisticGum[6];

            ListGums[0] = new StatisticGum() { Lives = 1, speed = 1 };
            ListGums[1] = new StatisticGum() { Lives = 2, speed = 1 };
            ListGums[2] = new StatisticGum() { Lives = 1, speed = 2 };
            ListGums[3] = new StatisticGum() { Lives = 2, speed = 3 };
            ListGums[4] = new StatisticGum() { Lives = 5, speed = 1 };
            ListGums[5] = new StatisticGum() { Lives = 2, speed = 2 };

            ListAmountProfile[0] = new AmountStatistic()
            {
                Apple = 0,
                Life = 1,
                Lives = 3,
                ammunition = 100,
                maxLife = 1,
                maxLives = 3,
                type = spritePers[0].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
            ListAmountProfile[1] = new AmountStatistic()
            {
                Apple = 0,
                Life = 3,
                Lives = 3,
                ammunition = 80,
                maxLife = 3,
                maxLives = 3,
                type = spritePers[1].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
            ListAmountProfile[2] = new AmountStatistic()
            {
                Apple = 0,
                Life = 2,
                Lives = 4,
                ammunition = 100,
                maxLife = 2,
                maxLives = 4,
                type = spritePers[2].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
            ListAmountProfile[3] = new AmountStatistic()
            {
                Apple = 0,
                Life = 4,
                Lives = 2,
                ammunition = 120,
                maxLife = 4,
                maxLives = 2,
                type = spritePers[3].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
            ListAmountProfile[4] = new AmountStatistic()
            {
                Apple = 0,
                Life = 2,
                Lives = 2,
                ammunition = 320,
                maxLife = 2,
                maxLives = 2,
                type = spritePers[4].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
            ListAmountProfile[5] = new AmountStatistic()
            {
                Apple = 0,
                Life = 4,
                Lives = 2,
                ammunition = 220,
                maxLife = 4,
                maxLives = 2,
                type = spritePers[5].type,
                kilGums = 0,
                Points = 0,
                Death = 0,
                Ruby = 0
            };
        }
        // Очистка змінних для початку нової гри
        public void StartNewGame()
        {
            level = new Level(graphics, Content);
            level.Load(Content);
            level.CreateLevel();
            ScrollX = 0;
            hero.rect = hero.rectStartUP;
            hero.AmountProfile = ListAmountProfile[heroChoice];

            if (isTwoPlayers)
            {
                hero2.rect = hero2.rectStartUP;
                hero2.AmountProfile = ListAmountProfile[heroChoice2];
            }

            isPaused = false;
            isStartNewGame = true;

        }
        // Завантаження текстур героїв
        SpriteAnimatedTexture LoadPersTexture(String name)
        {
            SpriteAnimatedTexture spritePers = new SpriteAnimatedTexture();
            String str = "Textures\\a_Persons\\";
            spritePers.idle = Content.Load<Texture2D>(str + name + "\\idle");
            spritePers.run = Content.Load<Texture2D>(str + name + "\\Run");
            spritePers.jump = Content.Load<Texture2D>(str + name + "\\jump");
            spritePers.type = name;
            return spritePers;
        }
        // Встановлення герєві анімації текстур
        public static void ChangePersTexture(AnimatedSprite _hero, int _heroChoice)
        {
            _hero.idle = spritePers[_heroChoice].idle;
            _hero.run = spritePers[_heroChoice].run;
            _hero.jump = spritePers[_heroChoice].jump;
            _hero.YShift = _hero.frameWidth = _hero.frameHeight = _hero.run.Height;

            _hero.AmountProfile.type = spritePers[_heroChoice].type;
        }
       
        // Переміщення видимої області екрану
        void Scroll(AnimatedSprite tmpHero)
        {
            if (isTwoPlayers)
            {
                Rectangle rectH1 = GetScreenRect(hero.rect);
                Rectangle rectH2 = GetScreenRect(hero2.rect);

                if ((SizeScreen.Width - 200 < rectH1.X && SizeScreen.Width - 200 < rectH2.X)
                        || (200 > rectH1.X && 200 > rectH2.X) && (hero.Visible.Equals(hero2.Visible)))
                    ScrollX += (int) tmpHero.dxspeed;

                if ((SizeScreen.Width - 50 < GetScreenRect(tmpHero.rect).X || 10 > GetScreenRect(tmpHero.rect).X))                
                    tmpHero.rect.Offset((int)-tmpHero.dxspeed, 0);
                

                if ((SizeScreen.Width - 200 < GetScreenRect(tmpHero.rect).X || 200 > GetScreenRect(tmpHero.rect).X)
                          && !(hero.Visible.Equals(hero2.Visible)))
                    ScrollX += (int) tmpHero.dxspeed;
            }
             else
              if ((SizeScreen.Width - 200 < GetScreenRect(tmpHero.rect).X || 200 > GetScreenRect(tmpHero.rect).X) && !isTwoPlayers)
                  ScrollX += (int) tmpHero.dxspeed;
        }
        // Розміщення обєкту на екрані
        public static Rectangle GetScreenRect(Rectangle rect)
        {
            Rectangle screenRect = rect;
            screenRect.Offset(-ScrollX, 0);
            return screenRect;
        }

        #endregion

        #region Initialization

        // Конструктор
        public Game1() 
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SizeScreen.Width;
            graphics.PreferredBackBufferHeight = SizeScreen.Height;

            ///////////////////////////////////////////////////////////////////////////////////
          //// Graphics.PreferredBackBufferFormat = SurfaceFormat.HalfSingle;   дуже обережно ж цим!!! 
       //////////////////////////////////////////////////////////////////////////////////////////

            Content.RootDirectory = "Bin";           

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            Components.Add(new MessageDisplayComponent(this));
          //  Components.Add(new GamerServicesComponent(this));
          
            screenManager.AddScreen(new BackgroundScreen(), null);          
           
            songLiberty.musicLevel = new Song[5];
            background = new Texture2D[5];

            SetSetting = new Setting();        
        }

        // Ініціалізація
        protected override void Initialize()
        {
            base.Initialize();

            SetListAmountProfile();
            SetSetting.SetSettingFromFile();

            screenManager.AddScreen(new IntroScreen(), null);
        }      

        // Завантаження файлів в память
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);           

            #region Music Load
            String str = "Sounds\\";            
            MediaPlayer.IsRepeating = true;
            songLiberty.background = Content.Load<Song>(@str + "background");
            songLiberty.winner = Content.Load<Song>(@str + "winner");
            songLiberty.musicLevel[0] = Content.Load<Song>(@str + "NinjAcademy_Music");
            songLiberty.musicLevel[1] = Content.Load<Song>(@str + "smb2-characterselect");
            songLiberty.musicLevel[2] = Content.Load<Song>(@str + "QotR");
            songLiberty.musicLevel[3] = Content.Load<Song>(@str + "smb-overworld");

            songLiberty.click = Content.Load<SoundEffect>(@str + "click");
            songLiberty.throwChuck = Content.Load<SoundEffect>(@str + "throwChuck");
            songLiberty.hitGum = Content.Load<SoundEffect>(@str + "hitGum");
            songLiberty.shot = Content.Load<SoundEffect>(@str + "shot");
            songLiberty.hitBlock = Content.Load<SoundEffect>(@str + "hitBlock");
            songLiberty.catchBonus = Content.Load<SoundEffect>(@str + "catchBonus");
            #endregion

            Font = Content.Load<SpriteFont>("Font\\Arial14");
            //cursor = new GameObject(Content.Load<Texture2D>("Textures\\menu\\cursor"));

            spritePers = new SpriteAnimatedTexture[6];
            spritePers[0] = LoadPersTexture("Mario");
            spritePers[1] = LoadPersTexture("ChuckNorris");
            spritePers[2] = LoadPersTexture("Aladdin");
            spritePers[3] = LoadPersTexture("Assassin");
            spritePers[4] = LoadPersTexture("Scorpion");
            spritePers[5] = LoadPersTexture("Homer");

            Rectangle rectStartUP = new Rectangle(300, 370, 60, 60);
            hero = new AnimatedSprite(rectStartUP, spritePers[heroChoice].idle, spritePers[heroChoice].run,
                                      spritePers[heroChoice].jump);
            hero.fireTexture = Content.Load<Texture2D>("Textures\\a_Persons\\ammunition");
            hero.rectStartUP = rectStartUP;

            hero2 = new AnimatedSprite(Game1.hero.rectStartUP, Game1.spritePers[Game1.heroChoice2].idle,
                                             Game1.spritePers[Game1.heroChoice2].run,
                                             Game1.spritePers[Game1.heroChoice2].jump);
            hero2.fireTexture = Content.Load<Texture2D>("Textures\\a_Persons\\ammunition");
            hero2.rectStartUP = Game1.hero.rectStartUP;
            hero2.rectStartUP.Offset(-35, 0);

            str = "Textures\\Game Elements\\Background\\";
            background[0] = Content.Load<Texture2D>(str + "highscoreBg");
            background[1] = Content.Load<Texture2D>(str +  "43");
            background[2] = Content.Load<Texture2D>(str + "gameplayBG");
            background[3] = Content.Load<Texture2D>(str + "love_is_all_around_us_by_Codex_nz");        
            background[4] = Content.Load<Texture2D>(str + "1600_0069");
        }

        // Вивантаження з памяті
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Control with Hero

        KeyboardState oldState;
        // Контроль героєм №1
        void ControlClickHero(KeyboardState keyboardState)
        {
            hero.speed = 4;
            if (keyboardState.IsKeyDown(Keys.D))
            {
                hero.StartRun(true);
                Scroll(hero);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                hero.StartRun(false);
                Scroll(hero);
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                hero.Stop();
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                hero.Jump();
            }
            if (keyboardState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (hero.AmountProfile.ammunition > 0) songLiberty.throwChuck.Play(songLiberty.VolumeEffect, 0.01f, 1);
                hero.countfire++;
                hero.doFire = true;
            }
            if (keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D))
            {
                hero.Stop();
            }
            if (keyboardState.IsKeyDown(Keys.NumPad1) && oldState.IsKeyUp(Keys.NumPad1))
            {
                level.currentLevel++;
                level.CreateLevel();
            }
            if (keyboardState.IsKeyDown(Keys.NumPad2) && oldState.IsKeyUp(Keys.NumPad2))
            {               
                hero.doSmall();
            }
            if (keyboardState.IsKeyDown(Keys.NumPad3) && oldState.IsKeyUp(Keys.NumPad3))
            {
                heroChoice++;
                if (heroChoice > maxHeroChoice) heroChoice = 0;
                ChangePersTexture(hero, heroChoice);
            }
        }
        // Контроль героєм №2
        void ControlClickHero2(KeyboardState keyboardState)
        {
            hero2.speed = 4;           
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                hero2.StartRun(true);
                Scroll(hero2);
            }           
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                hero2.StartRun(false);
                Scroll(hero2);
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                hero2.Stop();
            }           
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                hero2.Jump();
            }
            if (keyboardState.IsKeyDown(Keys.RightControl) && oldState.IsKeyUp(Keys.RightControl))
            {
                if (hero2.AmountProfile.ammunition > 0) songLiberty.throwChuck.Play(songLiberty.VolumeEffect, 0.01f, 1);
                hero2.countfire++;
                hero2.doFire = true;                
            }   
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left))  
            {
                hero2.Stop(); 
            }          
            if (keyboardState.IsKeyDown(Keys.NumPad2) && oldState.IsKeyUp(Keys.NumPad2))
            {              
                hero2.doSmall();
            }
            if (keyboardState.IsKeyDown(Keys.NumPad3) && oldState.IsKeyUp(Keys.NumPad3))
            {
                heroChoice2++;
                if (heroChoice2 > maxHeroChoice) heroChoice2 = 0;
                ChangePersTexture(hero2, heroChoice2);
            }                       
        }
       
        #endregion

        #region Update and Draw         

        // Оновлення всіх об'єктів
        protected override void Update(GameTime gameTime)
        {
            if (isStart)
            {
                StartNewGame();
                isStart = false;
            }
            // Дозволяє грі, щоб вийти
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mause = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.NumPad0))
                graphics.ToggleFullScreen();                           
            
            if (isStartNewGame && !isPaused)
            {
                if (CollidUpdateLevel != level.currentLevel)
                {                   
                    CollidUpdateLevel = level.currentLevel;
                    if (hero.isSmall) hero.doSmall();
                    hero.countfire = -1;
                  //  level.timeLife = 800;   
     
                    if (isTwoPlayers)
                    {
                        if (hero2.isSmall) hero2.doSmall();
                        hero2.countfire = -1;
                       // level.timeLife2 = 800;
                    }
                }

                if(hero.Visible) ControlClickHero(keyboardState);
                hero.Update(gameTime);

                if (isTwoPlayers)
                {
                    if (hero2.Visible) ControlClickHero2(keyboardState);
                    hero2.Update(gameTime);
                }

                level.Update(gameTime);                 
            }
            

            oldState = keyboardState;  
            base.Update(gameTime);
        }

        // Малювання всік об'єктів
        protected override void Draw(GameTime gameTime)
        {
            if (isStartNewGame)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue); 
                spriteBatch.Begin();                

                try
                  {
                      spriteBatch.Draw(background[level.currentLevel - 1],
                                  new Rectangle(0, 0, SizeScreen.Width, SizeScreen.Height), Color.White);
                  }
                  catch (System.Exception ex)
                  {
                      spriteBatch.Draw(background[1],
                                  new Rectangle(0, 0, SizeScreen.Width, SizeScreen.Height), Color.White);            	
                  }                   
                level.Draw(spriteBatch);

                if (isTwoPlayers) hero2.Draw(spriteBatch);
                hero.Draw(spriteBatch);               

                spriteBatch.End();
            }
           
            base.Draw(gameTime);
        }

        #endregion
    }
}
