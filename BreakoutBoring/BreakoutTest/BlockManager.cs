using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace BreakoutTest
{
    public class BlockManager : DrawableGameComponent
    {
        public List<MonogameBlock> Blocks { get; private set; } //List of Blocks the are managed by Block Manager

        //Dependancy on Ball
        Ball ball;

        List<MonogameBlock> blocksToRemove; //list of block to remove probably because they were hit

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        string winLossStateText;
        public bool isGameOver;

        /// <summary>
        /// BlockManager hold a list of blocks and handles updating, drawing a block collision
        /// </summary>
        /// <param name="game">Reference to Game</param>
        /// <param name="ball">Refernce to Ball for collision</param>
        public BlockManager(Game game, Ball b)
            : base(game)
        {
            this.Blocks = new List<MonogameBlock>();
            this.blocksToRemove = new List<MonogameBlock>();
            
            this.ball = b;
            // CHANGED TO 1 FROM -1
            ScoreManager.Level = 1;
    
        }

        public override void Initialize()
        {
            LoadLevel();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteFont = Game.Content.Load<SpriteFont>("Arial");
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        protected virtual void LoadLevel()
        {
            int arrayWidth = 24;
            int arrayHeight = 2;
            int arrayDepth = 1;
            CreateBlockArrayByWidthAndHeight(arrayWidth, arrayHeight, arrayDepth);
        }

        /// <summary>
        /// Simple Level lays out multiple levels of blocks
        /// </summary>
        /// <param name="width">Number of blocks wide</param>
        /// <param name="height">Number of blocks high</param>
        /// <param name="margin">space between blocks</param>
        protected void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            MonogameBlock b;
            //Create Block Array based on with and hieght
            for (int w = 0; w < width; w++)
            {               
                for (int h = 0; h < height; h++)
                {
                    b = new MonogameBlock(this.Game);
                    b.Initialize();
                    b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                    Blocks.Add(b);                    
                }
            }
        }
        
        bool reflected; //the ball should only reflect once even if it hits two bricks
        public override void Update(GameTime gameTime)
        {
            this.reflected = false; //only reflect once per update
            UpdateCheckBlocksForCollision(gameTime);
            UpdateBlocks(gameTime);
            UpdateRemoveDisabledBlocks();
            CheckWinLoss(gameTime);

            base.Update(gameTime);
        }

        private void UpdateBlocks(GameTime gameTime)
        {
            foreach (var block in Blocks)
            {
                block.Update(gameTime);
            }
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>
        /// 
        //COULD BE THE BUG
        private void UpdateRemoveDisabledBlocks()
        {
            //remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                ScoreManager.Score++;
            }
            blocksToRemove.Clear();
        }

        private void UpdateCheckBlocksForCollision(GameTime gameTime)
        {
            foreach (MonogameBlock b in Blocks)
            {
                if (b.Enabled) //Only chack active blocks
                {
                    b.Update(gameTime); //Update Block
                    //Ball Collision
                    if (b.Intersects(ball)) //chek rectagle collision between ball and current block 
                    {
                        //hit
                        b.HitByBall(ball);
                        //b IS NEVER IN BROKEN OR HIT STATE
                        if(b.BlockState == BlockState.Broken)
                            blocksToRemove.Add(b);  //Ball is hit add it to remove list
                        if (!reflected) //only reflect once
                        {
                            ball.Reflect(b);
                            this.reflected = true;
                        }
                    }
                }
            }
        }

        private void CheckWinLoss(GameTime gameTime)
        {
            if(!Blocks.Any())
            {
                Win(gameTime);
                //WIN
            }
            else if(ScoreManager.Lives <= 0)
            {
                Loss(gameTime);
                //LOSS
            }
            else
            {
                isGameOver = false;
            }
        }

        private void Win(GameTime gameTime)
        {
            InputHandler input = (InputHandler)this.Game.Services.GetService(typeof(IInputHandler));
            winLossStateText = "You Win!";
            isGameOver = true;
            if (input.KeyboardState.IsKeyDown(Keys.F))
            {
                ScoreManager.Level++;
                ScoreManager.Lives = 5;
                LoadLevel();
                isGameOver = false;
                ball.resetBall(gameTime);
            }
        }

        private void Loss(GameTime gameTime)
        {
            ScoreManager.Lives = 0;
            InputHandler input = (InputHandler)this.Game.Services.GetService(typeof(IInputHandler));
            winLossStateText = "You Lose!";
            isGameOver = true;
            if (input.KeyboardState.IsKeyDown(Keys.F))
            {
                ScoreManager.Score = 0;
                ScoreManager.Level = 1;
                ScoreManager.Lives = 5;
                LoadLevel();
                isGameOver = false;
                ball.resetBall(gameTime);
            }
        }

        /// <summary>
        /// Block Manager Draws blocks they don't draw themselves
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var block in this.Blocks)
            {
                if(block.Visible)   //respect block visible property
                    block.Draw(gameTime);
            }

            spriteBatch.Begin();

            if (isGameOver)
            {              
                spriteBatch.DrawString(spriteFont, winLossStateText, new Vector2(Game.GraphicsDevice.Viewport.Width / 1.13f, Game.GraphicsDevice.Viewport.Height / 1.155f), Color.Red);
                spriteBatch.DrawString(spriteFont, "Press F to continue", new Vector2(Game.GraphicsDevice.Viewport.Width / 1.27f, Game.GraphicsDevice.Viewport.Height/1.11f), Color.White);               
            }
            spriteBatch.DrawString(spriteFont, "Press G to slingshot the ball!", new Vector2(Game.GraphicsDevice.Viewport.Width / 80, Game.GraphicsDevice.Viewport.Height / 1.06f), Color.Magenta);
            spriteBatch.DrawString(spriteFont, "Press Space to slow time!", new Vector2(Game.GraphicsDevice.Viewport.Width / 1.4f, Game.GraphicsDevice.Viewport.Height / 1.06f), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
