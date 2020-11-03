using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace BreakoutTest
{
    class PaddleController
    {
        InputHandler input;
        Ball ball; //may should delgate to parent
        public Vector2 Direction { get; private set; }

        public PaddleController(Game game, Ball ball)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
            this.ball = ball;   //need refernce to ball to be able to lanch ball could possibly use delegate here
        }

        public void HandleInput(GameTime gametime, Game game)
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new upafet

            //No need to sum input only uses left and right
            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                this.Direction = new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                this.Direction = new Vector2(1, 0);
            }
            //TODO add mouse controll?

            //Up launches ball
            if (input.KeyboardState.WasKeyPressed(Keys.Up))
            {
                if (ball.State == BallState.OnPaddleStart) //Only Launch Ball is it's on paddle
                    this.ball.LaunchBall(gametime, this.Direction);
            }

            HandleTimeSlow();
        }

        private void HandleTimeSlow()
        {
            // ball.speed = 50
            if (input.KeyboardState.IsKeyDown(Keys.Space))
            {
                SlowTime(ball);
            }
            else if(input.KeyboardState.IsKeyDown(Keys.G))
            {
                FlipGravity();
            }
            // Return to normal speed after release
            if(input.KeyboardState.HasReleasedKey(Keys.Space))
            {
                ball.Speed = ball.randSpeed;
            }       
            else if(input.KeyboardState.HasReleasedKey(Keys.G))
            {
                ball.Speed = ball.randSpeed + 250;
            }            
        }

        private void SlowTime(Ball ball)
        {
            ball.Speed = 50;
        }

        private void FlipGravity()
        {
            ball.Speed = 0;
            ball.Speed -= 20;
        }

    }
}
    

