using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using Mario;
using Animated;

namespace GObject
{
    public struct AmountStatistic
    {
        public int Apple;
        public int Ruby;
        public int Life;
        public int Lives;
        public int ammunition;
        public int kilGums;
        public int Points;
        public int Death;

        public string type;

        public int maxLife;
        public int maxLives;
    }

    public class GameObject
    {
        #region Fields

        public Texture2D Sprite { get; set; } // Спрайт
        public Rectangle rect; // Позиція, розмір
        public Vector2 Velocity; // Швидкість
        public string type;
        public Color color = new Color(255,255,255,255);

        public int Width { get { return Sprite.Width; } } // Ширина
        public int Height { get { return Sprite.Height; } } // Висота
        public bool Visible { get; set; }
        public Rectangle Bounds  // Границя обєкту
        {
            get
            {
                return rect;
            }
        }

        public bool doLogic;
        public int dxspeed = 10;
        public bool isRight;   

        #endregion

        #region Initialization
        public GameObject(Texture2D sprite)
        {
            Sprite = sprite;
            Visible = true;
            rect = new Rectangle(0, 0, 1, 1);
            Velocity = Vector2.Zero;           
        }
        public GameObject(Texture2D sprite, Rectangle _rect)
        {
            Sprite = sprite;
            Visible = true;
            rect = _rect;
            Velocity = Vector2.Zero;
        }
        #endregion

        #region Methods

        // Разворачивание движения по горизонтальной оси
        public void ReflectHorizontal()
        {
            Velocity.Y = -Velocity.Y;
        }
        // Разворачивание движения по вертикальной оси
        public void ReflectVertical()
        {
            Velocity.X = -Velocity.X;
        }       

        public void UpdateLogic(AnimatedSprite hero)
        {
            if (doLogic && Visible)
            {
                if (!isRight)
                    rect.Offset(-dxspeed, 0);
                else
                   rect.Offset(dxspeed, 0);
            }
            foreach (AnimatedSprite gum in Game1.Elements.gums)
            {
                Rectangle worldRectGum = Game1.GetScreenRect(gum.rect);             
                if (worldRectGum.Intersects(Game1.GetScreenRect(rect)) && gum.Visible && Visible)
                {
                    Game1.songLiberty.hitGum.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1); 
                    gum.AmountProfile.Lives--;
                    if (gum.AmountProfile.Lives < 1) hero.AmountProfile.kilGums++;
                     Visible = false;                    
                }
            }
            foreach (GameObject block in Game1.Elements.blocks)
            {
                Rectangle worldRect = Game1.GetScreenRect(rect);
                worldRect.Height -= 10;
                if (worldRect.Intersects(Game1.GetScreenRect(block.rect)) && block.Visible && Visible)
                {
                    if (block.type == "block1" || block.type == "bambootopSliceHorizontal" || block.type == "bamboo")
                    {
                        Game1.songLiberty.hitBlock.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);  
                        block.Visible = Visible = false;
                        GameObject bon = new GameObject(Game1.level.bonusTexture[0],
                                         new Rectangle(block.rect.X, block.rect.Y,
                                                       rect.Width / 2, rect.Height / 2));
                        bon.type = "apple";
                        Game1.Elements.bonus.Add(bon);
                    }
                    else Visible = false;
                }
            }      
        }

        #endregion

        #region Draw
        public void DrawWorld(SpriteBatch spriteBatch)
        {
            if(Visible)
               spriteBatch.Draw(Sprite, Game1.GetScreenRect(rect), color);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Sprite, rect, color);
        }
        #endregion

        public virtual void Dispose()
        {
            Sprite.Dispose();             
        }
    }

    public class GameObjectAnimated : GameObject
    {
        #region Fields

        public int frameWidth;
        public int frameHeight;

        public bool isAnimated;
        public bool isSlowMode;

        public int currentFrame;
        int timeElapsed;
        int timeForFrame = 100;

        #endregion

        #region Initialization
        public GameObjectAnimated(Texture2D sprite) : base(sprite)       
        {
            Sprite = sprite;
            Visible = true;
            rect = new Rectangle(0, 0, 1, 1);
            frameWidth = frameHeight = Sprite.Height;
        }
        public GameObjectAnimated(Texture2D sprite, Rectangle _rect) : base(sprite, _rect)   
        {
            Sprite = sprite;
            Visible = true;
            rect = _rect;
            frameWidth = frameHeight = Sprite.Height;
        }
        #endregion

        public int Frames { get { return Sprite.Width / frameWidth; } }
        public void StartAnimated(bool isRight)
        {
            if (!isAnimated && Visible)
            {
                isAnimated = true;
                currentFrame = 0;
                timeElapsed = 0;
            }           
        }

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            if (isAnimated && Visible)
            {
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                int tempTime = timeForFrame;
                if (isSlowMode)
                    tempTime *= 4;
                if (timeElapsed > tempTime)
                {
                    currentFrame = (currentFrame + 1) % Frames;
                    timeElapsed = 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Rectangle r = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
                SpriteEffects effects = SpriteEffects.None;
               // if (isRunningRight)
                  //  effects = SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(Sprite, Game1.GetScreenRect(rect), r, color, 0, Vector2.Zero, effects, 0);
            }
        }
        #endregion

    }

    public class GOVideo : GameObject
    {
        #region Fields

        public Video VideoFile;
        public VideoPlayer videoPlayer;
        
        #endregion

        #region Initialization
        public GOVideo(Video _VideoFile) : base(null)
        {
            videoPlayer = new VideoPlayer();
            videoPlayer.Volume = Game1.songLiberty.VolumeMusic;

            VideoFile = _VideoFile;
            Visible = true;
            rect = new Rectangle(0, 0, 1, 1);

            videoPlayer.IsLooped = false;
            videoPlayer.Play(VideoFile);
        }
        public GOVideo(Video _VideoFile, Rectangle _rect): base(null, _rect)
        {
            videoPlayer = new VideoPlayer();
            videoPlayer.Volume = Game1.songLiberty.VolumeMusic;

            VideoFile = _VideoFile;
            Visible = true;
            rect = _rect;

            videoPlayer.IsLooped = false;
            videoPlayer.Play(VideoFile);
        }
        #endregion        

        #region Update

        public void Update(GameTime gameTime)
        {           
            try
            {
                Sprite = videoPlayer.GetTexture();
            }
            catch (System.Exception ex){ }           
        }

        public override void Dispose()
        {
            Sprite.Dispose();          
            videoPlayer.Stop();
        }
       
        #endregion

    }

    public class GOEffect
    {
        #region Fields

        public Renderer renderer;
        public ParticleEffect effect;
        public string fileName = "";
        public Vector2 trigger = new Vector2(1, 1);

        public bool Visible;

        GraphicsDeviceManager graphics;
        ContentManager Content;

        #endregion

        #region Initialization
        public GOEffect(GraphicsDeviceManager _graphics, ContentManager _Content, string _fileName)
        {
            graphics = _graphics;
            Content = _Content;

            renderer = new SpriteBatchRenderer { GraphicsDeviceService = graphics };
            effect = new ParticleEffect();
            fileName = _fileName;

            effect = Content.Load<ParticleEffect>(fileName);
            effect.LoadContent(Content);
            effect.Initialise();
            renderer.LoadContent(Content); 
            Visible = true;      
        }

        public GOEffect(GraphicsDeviceManager _graphics, ContentManager _Content)
        {
            graphics = _graphics;
            Content = _Content;

            effect = new ParticleEffect();       
            renderer = new SpriteBatchRenderer { GraphicsDeviceService = graphics};

            Visible = true; 
        }

        public void Load(string _fileName)
        {
            fileName = _fileName;
            effect = Content.Load<ParticleEffect>(fileName);
            effect.LoadContent(Content);
            effect.Initialise();
            renderer.LoadContent(Content);
        }
        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                effect.Trigger(trigger);
                float SecondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                effect.Update(SecondsPassed);
            }
        }

        public void Draw()
        {                 
           if (Visible) renderer.RenderEffect(effect);
        }
        #endregion

    }
}
