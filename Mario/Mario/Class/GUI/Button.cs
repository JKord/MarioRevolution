using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GUI
{
    public class Button 
    {
        public int X;
        public int Y;
        public string Text;
        public int Width = 256;
        public int Height = 64;     

        public bool Active = true;
        public bool Selected = false;
        public bool Clicked = false;

        public Color TextColor = Color.White;

        public Texture2D Texture;
        public Texture2D TextureClick; //Content.Load<Texture2D>(@"button");
        public SpriteFont Font;    //Content.Load<Texture2D>(@"FontEnum.Arial22");

        public event EventHandler Click;

        public Button(int x, int y, string text)
        {
            X = x;
            Y = y;
            Text = text;            
        }
        public Button(int x, int y, string text, int width, int height)
            : this(x, y, text)
        {
            this.Width = width;
            this.Height = height;           
        }

        public void OnClick()
        {
            if (Click != null)
                Click(this, null);
           
           // AudioManager.PlayButtonClick();
        }

        public bool MouseIn(int x, int y)
        {
            return (x > X - Width / 2 && y > Y - Height / 2 && x < X + Width / 2 && y < Y + Height / 2);
        }

        #region Draw
        public void Draw(SpriteBatch spriteBatch, int menuX, int menuY)
        {
            //spriteBatch.Begin();
            Texture2D tex;
            if (!Clicked)
                tex = Texture;
              else
                tex = TextureClick;
            if (tex != null)
            {
                spriteBatch.Draw(tex, new Rectangle(menuX + X - Width / 2, menuY + Y - Height / 2, Width, Height),
                                 Color.White);
            }

            Vector2 len = Font.MeasureString(Text);        
            Color col = Active ? TextColor : Color.Gray;
            col = Selected ? Color.BlueViolet : col;
            col = Clicked ? Color.Red : col;

            Vector2 textPos = new Vector2(menuX + X - len.X / 2, menuY + Y - len.Y / 2);

            spriteBatch.DrawString(Font, Text, textPos, col);
            Clicked = false;
           // spriteBatch.End();
        }
        #endregion
    }
}