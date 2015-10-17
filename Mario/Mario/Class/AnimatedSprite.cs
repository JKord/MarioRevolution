using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario;
using GObject;
using Microsoft.Xna.Framework.Content;

namespace Animated
{
    public class AnimatedSprite
    {
        #region Fields
        public Rectangle rect;
        public Texture2D idle;
        public Texture2D run;
        public Texture2D jump;       

        public GameObjectAnimated[] fire = new GameObjectAnimated[50];
        public Texture2D fireTexture;

        public int countfire = -1;
        public bool doFire;  

        public bool Visible = true;
        public Color color = new Color(255, 255, 255, 255);

        bool isRunning;
        public bool isSlowMode;
        bool isRunningRight;
        public bool isJumping;
        public bool isLogicRun;

        public int speed = 1;
        public int dxspeed;
        float yVelocity;
        float maxYVelocity = 10;
        float g = 0.2f;

        public int frameWidth;
        public int frameHeight;

        public int YShift;

        public int graund = 700;
        public int tempTimeSlowMode = 4;

        public Rectangle rectStartUP;
        public AmountStatistic AmountProfile;
                              

        public int Frames
        {
            get
            {
                return run.Width / frameWidth;
            }
        }

        public int X { get { return rect.X; } set { rect = new Rectangle(value, rect.Y, rect.Width, rect.Height); } }
        public int Y { get { return rect.Y; } set { rect = new Rectangle(rect.X, value, rect.Width, rect.Height); } }

        int currentFrame;
        int timeElapsed;
        int timeForFrame = 100;
        #endregion

        #region Initialization
        public AnimatedSprite(Rectangle rect, Texture2D run)
        {
            this.rect = rect;
            this.jump = this.idle = this.run = run;
            YShift = frameWidth = frameHeight = run.Height;           
        }
        public AnimatedSprite(Rectangle rect, Texture2D idle, Texture2D run, Texture2D jump)
        {
            this.rect = rect;
            this.idle = idle;
            this.run = run;
            this.jump = jump;

            YShift = frameWidth = frameHeight = run.Height;
        }
        #endregion

        #region Control
        public void SwitchModes()
        {
            isSlowMode = !isSlowMode;
        }
        public void StartRun(bool isRight)
        {
            if (!isRunning)
            {
                isRunning = true;
                currentFrame = 0;
                timeElapsed = 0;
            }
            isRunningRight = isRight;
        }
        public void Stop()
        {
            isRunning = false;
        }
        public void Jump()
        {
            if (!isJumping && yVelocity == 0.0f)
            {
                isJumping = true;
                currentFrame = 0;
                timeElapsed = 0;
                yVelocity = maxYVelocity;
            }
        }
        public bool isSmall;
        public void doSmall()
        {
            if (!isSmall)
            {
                YShift = 50;
                rect.Width = rect.Height = 30;
                maxYVelocity = (int)(maxYVelocity / 1.5);
                isSmall = true;
            }
            else
            {
                YShift = frameWidth;
                rect.Width = rect.Height = 60;
                maxYVelocity = (int)(maxYVelocity * 1.5);
                isSmall = false;
            }
        }
        public void Fire()
        {
            if (countfire > 49) countfire = 0;
            if (doFire && AmountProfile.ammunition > 0)
            {
                AmountProfile.ammunition--;
                fire[countfire] = new GameObjectAnimated(fireTexture, new Rectangle(rect.X, rect.Y, 40, 40));
                fire[countfire].doLogic = true;
                fire[countfire].frameWidth = 74;
                fire[countfire].StartAnimated(fire[countfire].isRight);
                doFire = false;
                fire[countfire].isRight = isRunningRight;
            }
        }
        #endregion

        #region Interaction with the level
        public void CollidesWithBonus()
        {
            foreach (GameObject bon in Game1.Elements.bonus)
            {
                Rectangle rectBon = Game1.GetScreenRect(bon.rect);
                rectBon = new Rectangle(rectBon.X, rectBon.Y, rectBon.Width - rectBon.Width % 10, rectBon.Height);
                if (rectBon.Intersects(Game1.GetScreenRect(rect)) && bon.Visible)
                {
                    if (bon.type == "apple")
                    { AmountProfile.Apple++; bon.Visible = false; }
                    if (bon.type == "heart" && AmountProfile.Life < AmountProfile.maxLife)
                    { AmountProfile.Life++; bon.Visible = false; }
                    if (bon.type == "men" && AmountProfile.Lives < AmountProfile.maxLives)
                    { AmountProfile.Lives++; bon.Visible = false; }
                    if (bon.type == "beetle")
                    {
                        rect.Offset(0, -35);
                        doSmall();
                        bon.Visible = false;
                    }
                    if (bon.type == "box1")
                    { AmountProfile.ammunition += 10; bon.Visible = false; }
                    if (bon.type == "ruby")
                    { AmountProfile.Ruby++; bon.Visible = false; }
                    if (!isLogicRun && !bon.Visible) Game1.songLiberty.catchBonus.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                }
            }
        }
        public bool CollidesWithLevel(Rectangle rec)
        {
            foreach (GameObject block in Game1.Elements.blocks)
            {
                Rectangle rectBlock = Game1.GetScreenRect(block.rect);
                rectBlock.Offset(-10, 0);
                if (rectBlock.Intersects(Game1.GetScreenRect(rec)) && block.Visible)
                    return true;
            }
            return false;
        }
        public void ApplyGravity(GameTime gameTime)
        {
            yVelocity = yVelocity - g * gameTime.ElapsedGameTime.Milliseconds / 10;
            float dy = yVelocity * gameTime.ElapsedGameTime.Milliseconds / 10;

            Rectangle nextPosition = rect;
            nextPosition.Offset(0, -(int)dy);

            Rectangle boudingRect = GetBoundingRect(nextPosition);
            if (boudingRect.Bottom < graund && !CollidesWithLevel(boudingRect))
                rect = nextPosition;

            bool collideOnFallDown = (CollidesWithLevel(boudingRect) && yVelocity < 0);

            if (boudingRect.Bottom > graund || collideOnFallDown)
            {
                yVelocity = 0;
                isJumping = false;
            }

        }

        void RestartLevel()
        {
            AmountProfile.Death++;
            AmountProfile.Lives--;
            AmountProfile.Life = AmountProfile.maxLife;
            if (AmountProfile.Lives > -1) Game1.ScrollX = 0;
            rect = rectStartUP;
            countfire = 0;
            if (isSmall) doSmall();

        }
        void profileEditor()
        {
            if (rect.Y > graund - 100)
            {
                Game1.songLiberty.shot.Play(Game1.songLiberty.VolumeEffect, 0.01f, 1);
                RestartLevel();
            }
            if (AmountProfile.Life < 1) RestartLevel();
            if (AmountProfile.Life > AmountProfile.maxLife)
                AmountProfile.Life = AmountProfile.maxLife;
            if (AmountProfile.Lives > AmountProfile.maxLives)
                AmountProfile.Lives = AmountProfile.maxLives;
        }
        #endregion

        #region Update and Draw    
      
        public void Update(GameTime gameTime)
        {
            if (Visible && Game1.Elements.elemeNotCollides != null)
            {
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                int tempTime = timeForFrame;
                if (isSlowMode)
                    tempTime *= tempTimeSlowMode;
                if (timeElapsed > tempTime)
                {
                    currentFrame = (currentFrame + 1) % Frames;
                    timeElapsed = 0;
                }
                if (isRunning)
                {
                    dxspeed = speed * gameTime.ElapsedGameTime.Milliseconds / 10;
                    if (!isRunningRight)
                        dxspeed = -dxspeed;
                    Rectangle nextPosition = rect;
                    nextPosition.Offset(dxspeed, 0);
                    Rectangle boudingRect = GetBoundingRect(nextPosition);
                    if (!CollidesWithLevel(boudingRect))
                        rect = nextPosition;
                }
                ApplyGravity(gameTime);
                CollidesWithBonus();

                Fire();
                try
                {
                    if (countfire > -1 && AmountProfile.ammunition > 0)
                        for (int i = 0; i <= countfire; i++)
                        {
                            fire[i].UpdateLogic(this);
                            fire[i].Update(gameTime);
                        }
                }
                catch (System.Exception ex)  {  }
               

                if (isLogicRun)
                {
                    Rectangle nextPosition = rect;
                    nextPosition.Offset(dxspeed, 0);
                    Rectangle boudingRect = GetBoundingRect(nextPosition);
                    if (!CollidesWithLevel(boudingRect))
                        StartRun(isRunningRight);
                    else
                    {                                              
                        isRunningRight = !isRunningRight;
                    }
                }

                if (isLogicRun && AmountProfile.Lives < 1)
                    Visible = false;
                if (!isLogicRun) profileEditor();
            }
        }

        public Rectangle GetBoundingRect(Rectangle rectangle)
        {
            int width = (int)(YShift * 0.2f);
            int height = (int)(YShift * 0.62f);
            int x = rectangle.Left + (int)(YShift * 0.1f);

            return new Rectangle(x, rectangle.Top - 1, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && Game1.Elements.elemeNotCollides != null)
            {
                Rectangle r = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
                SpriteEffects effects = SpriteEffects.None;
                if (isRunningRight)
                    effects = SpriteEffects.FlipHorizontally;
                if (isJumping)
                {
                    spriteBatch.Draw(jump, Game1.GetScreenRect(rect), r, color, 0, Vector2.Zero, effects, 0);
                }
                else
                    if (isRunning)
                    {
                        spriteBatch.Draw(run, Game1.GetScreenRect(rect), r, color, 0, Vector2.Zero, effects, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(idle, Game1.GetScreenRect(rect), color);
                    }
               try
               {
                   if (countfire > -1 && AmountProfile.ammunition > 0)
                       for (int i = 0; i <= countfire; i++)
                           fire[i].Draw(spriteBatch);  
               }
               catch (System.Exception ex) { countfire = -1; }                                          
                
            }
        }

        #endregion
    }

}
