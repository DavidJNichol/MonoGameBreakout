using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakoutTest
{
    public class Level1 : BlockManager
    {

        public Level1(Game1 game, Ball b) : base(game, b)
        {
            ScoreManager.Level = 1;
        }

        protected override void LoadLevel()
        {
            //Load something differnt
            base.LoadLevel();
        }
    }
}