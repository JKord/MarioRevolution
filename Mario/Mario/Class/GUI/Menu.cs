using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GUI
{
    public class Menu
    {
        protected int current = 0;
        protected List<Button> buttons = new List<Button>();
        protected int x;
        protected int y;
        protected Texture2D texture;
        public bool CheckMode = true;
        protected int width;
        protected int height;

        public bool Visible = true;
        public Color TextColor = Color.White;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public List<Button> Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }
        public Menu(int x, int y, Texture2D texture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
            if (texture != null)
            {
                width = texture.Width;
                height = texture.Height;
            }

        }
        public Menu(int x, int y, Texture2D texture, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
            this.width = width;
            this.height = height;
        }

        public void UnselectAll()
        {
            foreach (Button button in buttons)
            {
                button.Clicked = false;
            }
        }

        #region update
        public virtual void Update()
        {
            if (Visible)
            {
                MouseState state = Mouse.GetState();
                foreach (Button button in buttons)
                {
                    button.Selected = false;
                    if (button.MouseIn(state.X - x, state.Y - y) && button.Active)
                    {
                        button.Selected = true;
                        if (state.LeftButton == ButtonState.Pressed && !button.Clicked)
                        {
                            button.OnClick();
                            if (CheckMode)
                            {
                                UnselectAll();
                                button.Clicked = true;
                            }
                        }
                    }
                }
             }
        }
        #endregion

        public virtual void Draw(SpriteBatch spriteBatch)
        {
           if (Visible)
           {
             if (texture != null)
             {
                // spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                 spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.White);
                // spriteBatch.End();
             }
             foreach (Button button in buttons)
             {
                 button.Draw(spriteBatch, x, y);
             }       
          }
        }
    }
}