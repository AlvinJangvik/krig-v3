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
        private static int town_amount = 50;
        private static Vector2[] towns = new Vector2[town_amount];
        private static int[] diff = new int[town_amount];
        private static int[] side = new int[town_amount]; // 1 == röd; 2 == neutral; 3 == Blå
        private static int fought_town = 0;
        private static int temp;

        // Enemy ints
        private static int eSize;
        private static int eDmg;
        private static int eHealth;

        public Map(Texture2D skin)
        {
            tex = skin;
            for(int i = 0; i < town_amount; i++)
            {
                towns[i] = new Vector2(Rand(10, 760), Rand(10, 450));
                diff[i] = Rand(1, 3);
                side[i] = 2;
                if(i > 0 && Collision(towns[i], towns[i - 1], diff[i - 1], diff[i]))
                {
                    temp = Rand(1, 4);
                    while(Collision(towns[i], towns[i - 1], diff[i - 1], diff[i]))
                    {
                        if(temp == 1)
                        {
                            towns[i] += new Vector2(20, 0);
                        }
                        else if (temp == 2)
                        {
                            towns[i] -= new Vector2(20, 0);
                        }
                        else if (temp == 3)
                        {
                            towns[i] += new Vector2(0, 20);
                        }
                        else if (temp == 4)
                        {
                            towns[i] -= new Vector2(0, 20);
                        }
                    }
                }
            }
        }

        public static void Win()
        {
            side[fought_town] = 1;
        }

        // ############################################################################
        //                Enemy turn
        // ############################################################################
        public static void Enemey()
        {
            // Target
            temp = Rand(0, town_amount - 1);
            if(eSize + eDmg < 120 + 50)
            {
                while(diff[temp] > 1)
                {
                    temp = Rand(0, town_amount - 1);
                }
            }
            else if (eSize + eDmg < 450 + 100)
            {
                while (diff[temp] > 2)
                {
                    temp = Rand(0, town_amount - 1);
                }
            }

            // Figthing
            if (diff[temp] == 1)
            {
                if(Rand(-eSize - eDmg, Rand(1, 35) + Rand(5, 20)) < 0)
                {
                    side[temp] = 3;
                }
            }
            else if (diff[temp] == 2)
            {
                if (Rand(-eSize - eDmg, Rand(120, 150) + Rand(50, 70)) < 0)
                {
                    side[temp] = 3;
                }
            }
            else
            {
                if (Rand(-eSize - eDmg, Rand(450, 850) + Rand(100, 130)) < 0)
                {
                    side[temp] = 3;
                }
            }
        }

        // ############################################################################
        //                Update
        // ############################################################################
        public void Update()
        {
            MouseState mState = Mouse.GetState();
            for(int i = 0; i < town_amount; i++)
            {
                if(mState.LeftButton == ButtonState.Pressed)
                {
                    if(Collision(mState, towns[i], diff[i]) && side[i] > 1)
                    {
                        fought_town = i;
                        if (side[i] == 2)
                        {
                            if (diff[i] == 1)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(1, 35), Rand(5, 20), Rand(50, 150));
                                soldiers.Game = 1;
                            }
                            else if (diff[i] == 2)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(120, 150), Rand(50, 70), Rand(150, 250));
                                soldiers.Game = 1;
                            }
                            else if (diff[i] == 3)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(450, 850), Rand(100, 130), Rand(250, 350));
                                soldiers.Game = 1;
                            }
                        }
                        else
                        {
                            Effects.Clear_blood();
                            soldiers.Fight(eSize, eDmg, eHealth);
                            soldiers.Game = 1;
                        }
                    }
                }
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


        // ############################################################################
        //                Draw
        // ############################################################################
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < town_amount; i++)
            {
                if (side[i] == 1)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i] * 5, diff[i] * 5), Color.Red);
                }
                if (side[i] == 2)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i] * 5, diff[i] * 5), Color.White);
                }
                if (side[i] == 3)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i] * 5, diff[i] * 5), Color.Blue);
                }
            }
        }


        // ############################################################################
        //                Collision
        // ############################################################################
        public bool Collision(MouseState m, Vector2 b, int d)
        {
            if (m.X > b.X && m.X < b.X + d * 5 && m.Y > b.Y && m.Y < b.Y + d * 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Collision(Vector2 b1, Vector2 b2, int d2, int d1)
        {
            if (b1.X + d1 * 5 > b2.X && b1.X < b2.X + d2 * 5 && b1.Y + d1 * 5 > b2.Y && b1.Y < b2.Y + d2 * 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
