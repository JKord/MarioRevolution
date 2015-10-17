using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
//using System.Windows;

namespace GUI
{
    public class MainMenu : Menu
    {
        public Button NewGame;
        public Button ResumeGame;
        public Button ExitGame;
        static public Texture2D Logo;
        public SpriteFont Font;

        public MainMenu(ContentManager Content, Rectangle rect) : base(0, 0, Logo)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
            int zdvugX = -50;
            int zdvugY = 120;
            NewGame = new Button(x + zdvugX, y + zdvugY, "New Game");
            ResumeGame = new Button(x + zdvugX, y + 90 + zdvugY, "Resume Game");
            ExitGame = new Button(x + zdvugX, y + 180 + zdvugY, "Exit");

            CheckMode = true;         
            RefreshButtons(false);

            Texture2D button_0 = Content.Load<Texture2D>("Textures\\menu\\button_0");
            Texture2D button_1 = Content.Load<Texture2D>("Textures\\menu\\button_1");
            NewGame.Texture = button_0; 
            ResumeGame.Texture = button_0;
            ExitGame.Texture = button_0;

            NewGame.TextureClick = button_1;
            ResumeGame.TextureClick = button_1;
            ExitGame.TextureClick = button_1;
         //   texture = Content.Load<Texture2D>(@"TimeGUI");

            SpriteFont font = Content.Load<SpriteFont>("Font\\Arial12");
            NewGame.Font = font;
            ResumeGame.Font = font;
            ExitGame.Font = font;
            Font = font;

            NewGame.Click += new System.EventHandler(this.NewGame_Click);
            ResumeGame.Click += new System.EventHandler(this.ResumeGame_Click);
            ExitGame.Click += new System.EventHandler(this.ExitGame_Click);            
        }
        public void RefreshButtons(bool resume)
        {
            if (resume)
            {
                ResumeGame.Active = true;
                NewGame.Active = false;
            }
            else
            {
                ResumeGame.Active = false;
                NewGame.Active = true;
            }
            buttons.Clear();

            buttons.Add(NewGame);
            buttons.Add(ResumeGame);
            buttons.Add(ExitGame);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
           
            //Vector2 w =  Font.MeasureString("XNA City Simulator");
            spriteBatch.DrawString(Font, "Next Level - Num1\nfull screen - Nam0", new Vector2(x,y), Color.Red);
         
        }

        void NewGame_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("NewGame");
           // NewGame.Clicked = false;  
            ResumeGame.Active = true;
            NewGame.Active = false;
            this.Visible = false;            
        }
        void ResumeGame_Click(object sender, EventArgs e)
        {            
            this.Visible = false;
        }
        void ExitGame_Click(object sender, EventArgs e)
        {
           
        }

    }
}