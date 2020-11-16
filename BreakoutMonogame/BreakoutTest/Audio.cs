using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BreakoutTest
{
    class Audio : GameComponent
    {
        Song song;

        public Audio(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            song = this.Game.Content.Load<Song>("song");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = .2f;

            base.Initialize();
        }
    }
}
