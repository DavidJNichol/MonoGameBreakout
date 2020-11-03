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
    public enum BallState {  OnPaddleStart, Playing }

    public class Ball : DrawableSprite
    {        
        public BallState State { get; private set; }

        GameConsole console;

        Random random;

        public Ball(Game game)
            : base(game)
        {
            this.State = BallState.OnPaddleStart;

            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console let's add a new one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
#if DEBUG
            this.ShowMarkers = true;
#endif
        }

        public void SetInitialLocation()
        {
            Viewport viewPort = new Viewport();
            this.Location = new Vector2(viewPort.Width/2, viewPort.Height/2); 
        }

        public void LaunchBall(GameTime gameTime, Vector2 paddleDirection)
        {
            random = new Random();
            int randSpeed = random.Next(225,255);
            double randAngle = random.NextDouble() * MathHelper.TwoPi;
            
            this.Speed = randSpeed;
            //this.Direction.X += paddleDirection.X;
            this.Direction.Y = -(float)Math.Sin(randAngle * 2);
            //this.Direction = new Vector2((float)Math.Cos(randAngle)*2, -(float)Math.Sin(randAngle)*2); 
            this.State = BallState.Playing;
            this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            SetInitialLocation();
            base.LoadContent();
        }

        private void resetBall(GameTime gameTime)
        {
            this.Speed = 0;
            this.State =  BallState.OnPaddleStart;
            this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
        }

        public override void Update(GameTime gameTime)
        {
            switch(this.State)
            {
                case BallState.OnPaddleStart:
                    break;

                case BallState.Playing:
                    UpdateBall(gameTime);
                    break;
            }
            
            base.Update(gameTime);
        }

        private void UpdateBall(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X < 0))
            {
                this.Direction.X *= -1;
            }
            //bottom Miss
            if (this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
            {
                //Lose a life
                ScoreManager.Lives--;
                this.resetBall(gameTime);
            }

            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }
        }

        public void Reflect(MonogameBlock block)
        {
            this.Direction.Y *= -random.Next(1,2); //TODO check for side collision with block
        }

    }
}
