using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Template
{
    class Map : VisualBase 
    {
        private static Random rand = new Random();
        private static int town_amount = 10;
        private static Vector2[] towns = new Vector2[town_amount];
        private static int[] diff = new int[town_amount];

        public Map(Texture2D skin)
        {
            tex = skin;
            for(int i = 0; i < town_amount; i++)
            {
                towns[i] = new Vector2(Rand(10, 760), Rand(10, 450));
                diff[i] = Rand(1, 3);
            }
        }

        // ############################################################################
        //                Random
        // ############################################################################
        public static int Rand(int min, int max)
        {
            int r = rand.Next(min, max + 1);

            return r;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < town_amount; i++)
            {
                spriteBatch.Draw(tex, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i], diff[i]), Color.White);
            }
        }
    }
}
