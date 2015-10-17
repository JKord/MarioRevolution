#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mario;
using System.Threading;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using GObject;
#endregion

namespace NetworkStateManagement
{    
    class MainMenuScreen : MenuScreen
    {
        ContentManager content;        

        #region Initialization

        public MainMenuScreen(): base("") 
        {
            MediaPlayer.Play(Game1.songLiberty.background);

            // Create our menu entries. 
            MenuEntry singlePlayerMenuEntry = new MenuEntry(Mario.Resource.SinglePlayer);
            MenuEntry twoPlayerMenuEntry = new MenuEntry(Mario.Resource.TwoPlayers);
            MenuEntry SettingsMenuEntry = new MenuEntry(Mario.Resource.Settings);
            MenuEntry Developers = new MenuEntry(Mario.Resource.Developers);
            MenuEntry exitMenuEntry = new MenuEntry(Mario.Resource.Exit);           
          
            // Hook up menu event handlers.
            singlePlayerMenuEntry.Selected += SinglePlayerMenuEntrySelected;
            twoPlayerMenuEntry.Selected += twoPlayerMenuEntrySelected;
            SettingsMenuEntry.Selected += SettingsMenuEntrySelected; 
            Developers.Selected += DevelopersMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;            

            // Add entries to the menu.
            MenuEntries.Add(singlePlayerMenuEntry);
            MenuEntries.Add(twoPlayerMenuEntry);
            MenuEntries.Add(SettingsMenuEntry); 
            MenuEntries.Add(Developers);
            MenuEntries.Add(exitMenuEntry);            
        }
        #endregion      
 
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Bin");           
                         
            ScreenManager.Game.ResetElapsedTime();           
        }


        #region Handle Input 
  
        void SinglePlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex);           
           // ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);  
            Mario.Game1.isPaused = false;           
            Mario.Game1.isTwoPlayers = false;
            ScreenManager.AddScreen(new ElectoralCharacter(), e.PlayerIndex);            
        }
        void twoPlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex);
            // ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);              
            Mario.Game1.isPaused = false;
            Mario.Game1.isTwoPlayers = true;           
            ScreenManager.AddScreen(new ElectoralCharacter(), e.PlayerIndex);         
        }

        void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {           
            ScreenManager.AddScreen(new SettingsScreen(), e.PlayerIndex);
        }      

        void DevelopersMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {             
            DevelopersScreen developersScreen = new DevelopersScreen(Mario.Resource.DevelopersText);
            developersScreen.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(developersScreen, null);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {            
            MessageBoxScreen confirmExitMessageBox =
                                    new MessageBoxScreen(Mario.Resource.ConfirmExitSample);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
