using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        bool isGameOver;

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
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        protected virtual void LoadLevel()
        {
            CreateBlockArrayByWidthAndHeight(24, 2, 1);
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
            CheckWinLoss();

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

        private void CheckWinLoss()
        {
            if(!Blocks.Any())
            {
                spriteBatch = new SpriteBatch(Game.GraphicsDevice);
                spriteFont = Game.Content.Load<SpriteFont>("Arial");
                winLossStateText = "You Win!";
                isGameOver = true;
                //WIN
            }
            else if(ScoreManager.Lives == 0)
            {
                spriteBatch = new SpriteBatch(Game.GraphicsDevice);
                spriteFont = Game.Content.Load<SpriteFont>("Arial");
                winLossStateText = "You Lose!";
                isGameOver = true;
                //LOSS
            }
            else
            {
                isGameOver = false;
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

            if(isGameOver)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, winLossStateText, new Vector2(Game.GraphicsDevice.Viewport.Width / 1.15f, Game.GraphicsDevice.Viewport.Height / 1.06f), Color.Red);
                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}
