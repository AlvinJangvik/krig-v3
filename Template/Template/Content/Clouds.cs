using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Content
{
    class Clouds : VisualBase
    {
        private static Random rand = new Random(); 

        private static int num = 20;
        private static Vector2[] pos = new Vector2[num];
        private static int[] size = new int[num];
        private static double[] speed = new double[num];

        public Clouds(Texture2D skin)
        {
            tex = skin;
            for (int i = 0; i < num; i++)
            {
                pos[i] = new Vector2(Menu.Rand(820, 820 * 2), Menu.Rand(0, 300));
                size[i] = Menu.Rand(20, 30);
                speed[i] = Rand(0, 2);
            }
        }

        public override void Update()
        {
            for(int i = 0; i < num; i++)
            {
                pos[i].X -= (float)speed[i];
                if(pos[i].X <= -10)
                {
                    pos[i] = new Vector2(Menu.Rand(820, 830), Menu.Rand(0, 300));
                    size[i] = Menu.Rand(20, 30);
                    speed[i] = Rand(0, 2);
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < num; i++) 
            {
                spriteBatch.Draw(tex, new Rectangle((int)pos[i].X, (int)pos[i].Y, size[i] * 2, size[i]), Color.WhiteSmoke);
            }
        }

        // ############################################################################
        //                Float Random
        // ############################################################################
        public static double Rand(double min, double max)
        {
            double r = rand.Next((int)min, (int)max + 1);
            r += rand.Next(1, 10) * 0.1;

            return r;
        }
    }
}
