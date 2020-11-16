using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;


namespace BreakoutTest
{
    class ScoreManager : DrawableGameComponent
    {

        SpriteFont font;
        public static int Lives;    
        public static int Level;
        public static int Score;

        Texture2D paddle;   //Texture for drawing lives left scoremanager is also the GUI/HUD

        SpriteBatch sb;
        Vector2 scoreLoc, livesLoc, levelLoc; //Locations to draw GUI elements
        
        
        public ScoreManager(Game game)
            : base(game)
        {
            SetupNewGame();
        }


        private static void SetupNewGame()  //Generally mixing static and non static methods is messy be careful
        {
            Lives = 5;
            Level = 1;
            Score = 0;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            paddle = this.Game.Content.Load<Texture2D>("paddleSmall");
            livesLoc = new Vector2(Game.GraphicsDevice.Viewport.Width/25.5f, Game.GraphicsDevice.Viewport.Height/43.5f); 
            levelLoc = new Vector2(Game.GraphicsDevice.Viewport.Width / 1.35f, Game.GraphicsDevice.Viewport.Height / 43.5f);
            scoreLoc = new Vector2(Game.GraphicsDevice.Viewport.Width / 1.15f, Game.GraphicsDevice.Viewport.Height / 43.5f);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            for (int i = 0; i < Lives; i++)
            {
                sb.Draw(paddle, new Rectangle((65 * i) + 200, 15, paddle.Width / 2, paddle.Height / 2), Color.White);
            }
            sb.DrawString(font, "Lives: " + Lives, livesLoc, Color.White);
            sb.DrawString(font, "Score: " + Score, scoreLoc, Color.White);
            sb.DrawString(font, "Level: " + Level, levelLoc, Color.White); 
            sb.End();
            base.Draw(gameTime);
        }
    }
}
