using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutTest
{
    public class MonogameBlock : DrawableSprite
    {
        protected Block block;

        protected string NormalTextureName, HitTextureName;
        protected Texture2D NormalTexture, HitTexture;

        private BlockState blockstate;
        public BlockState BlockState
        {
            //FIX THIS
            get { return this.block.BlockState; } //encapulsate block.BlockState
            set { this.blockstate = block.BlockState; }
            //get { return this.block.BlockState = this.blockstate; } //encapulsate block.BlockState
            //set { this.block.BlockState = this.blockstate = value; }
        }

        public MonogameBlock(Game game)
        : base(game)
        {
            this.block = new Block();

            // different block colors
            if (ScoreManager.Level % 2 == 0)
            {
                NormalTextureName = "block_blue";
                HitTextureName = "block_red";
            }
            else
            {
                NormalTextureName = "block_yellow";
                HitTextureName = "block_bubble";
            }           
        }

        protected virtual void updateBlockTexture()
        {
            switch (block.BlockState)
            {
                case BlockState.Normal:
                    this.Visible = true;
                    this.spriteTexture = NormalTexture;
                    break;
                case BlockState.Hit:
                    this.spriteTexture = HitTexture;
                    break;
                case BlockState.Broken:
                    this.spriteTexture = NormalTexture;
                    //this.enabled = false;
                    this.Visible = false; //don't show block

                    break;
            }
        }

        protected override void LoadContent()
        {
            this.NormalTexture = this.Game.Content.Load<Texture2D>(NormalTextureName);
            this.HitTexture = this.Game.Content.Load<Texture2D>(HitTextureName);
            updateBlockTexture(); //notice this is in loadcontent not the constuctor
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            block.UpdateBlockState();
            UnityBlockUpdate();

            
        }
        protected virtual void UnityBlockUpdate()
        {
            updateBlockTexture();
        }
        //THIS MIGHT NOT BE USED
        public void HitByBall(Ball ball)
        {
            this.block.Hit();
        }
    }
}
