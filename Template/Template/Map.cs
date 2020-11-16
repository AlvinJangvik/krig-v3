using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Template
{
    class Map : VisualBase 
    {
        private static MouseState oldState;
        private static Random rand = new Random();

        private static SpriteFont text;
        private static Texture2D medium;
        private static Texture2D big;

        private static int owned_palces;
        public static int town_amount = 100;
        public static Vector2[] towns = new Vector2[town_amount];
        private static int[] diff = new int[town_amount];
        private static int[] side = new int[town_amount]; // 1 == röd; 2 == neutral; 3 == Blå
        private static int fought_town = 0;
        private static int temp;
        private static int temp2;

        private static float fTemp;
        private static float redChange;
        private static float blueChange;

        private static Color[] sidec = new Color[town_amount];

        private static int town_size = 17;

        // Enemy ints
        private static int eSize = 10;
        private static int eDmg = 15;
        private static int eHealth = 100;
        private static int bOwned_places;

        public Map(Texture2D Big, Texture2D Medium, Texture2D skin, SpriteFont sprf)
        {
            tex = skin;
            big = Big;
            medium = Medium;
            text = sprf;

            for (int i = 0; i < town_amount; i++)
            {
                towns[i] = new Vector2(Rand(10, 760), Rand(10, 300));
                if(Rand(1, 5) == 1)
                {
                    if(Rand(1, 5) == 1)
                    {
                        diff[i] = 3;
                    }
                    else
                    {
                        diff[i] = 2;
                    }
                }
                else
                {
                    diff[i] = 1;
                }
                side[i] = 2;
            }
            Town_Move();
        }

        // ############################################################################
        //                Get sets
        // ############################################################################
        public static int OwnedPlaces
        {
            get { return owned_palces; }
        }


        // ############################################################################
        //                Win
        // ############################################################################
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
            if (bOwned_places < 1)
            {
                temp = Rand(0, town_amount - 1);
                if (diff[temp] != 1)
                {
                    while (true)
                    {
                        temp = Rand(0, town_amount - 1);
                        if(diff[temp] == 1 && side[temp] < 3)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                temp = Rand(0, town_amount - 1);

                if (eSize + eDmg < 120 + 50)
                {
                    while (true)
                    {
                        temp = Rand(0, town_amount - 1);
                        if(diff[temp] == 1 && side[temp] < 3 && Available(temp, 3))
                        {
                            break;
                        }
                    }
                }
                else if (eSize + eDmg < 450 + 100)
                {
                    while (true)
                    {
                        temp = Rand(0, town_amount - 1);
                        if(diff[temp] < 3 && side[temp] < 3 && Available(temp, 3))
                        {
                            break;
                        }
                    }
                }
                else if (side[temp] < 3)
                {
                    while (true)
                    {
                        temp = Rand(0, town_amount - 1);
                        if(side[temp] < 3 && Available(temp, 3))
                        {
                            break;
                        }
                    }
                }
            }


            // Figthing
            if (side[temp] == 1) // Players town
            {
                temp2 = Rand(-(eSize + eDmg + eHealth), soldiers.RedDmgValue + soldiers.BlueHealthValue + soldiers.RedSoldierAmount);
                if (temp2 < 0)
                {
                    if (diff[temp] == 1)
                    {
                        side[temp] = 3;
                        eDmg += Rand(1, 6);
                        eSize += Rand(2, 10);
                        eHealth += Rand(1, 7);
                    }
                    else if (diff[temp] == 2)
                    {
                        side[temp] = 3;
                        eDmg += Rand(5, 14);
                        eSize += Rand(10, 23);
                        eHealth += Rand(6, 17);
                    }
                    else if (diff[temp] == 3)
                    {
                        side[temp] = 3;
                        eDmg += Rand(12, 26);
                        eSize += Rand(20, 44);
                        eHealth += Rand(15, 32);
                    }
                }
            }
            else if (diff[temp] == 1) // Neutrals towns
            {
                temp2 = Rand(-(eSize + eDmg), Rand(1, 35) + Rand(5, 20));
                Console.WriteLine(-(eSize + eDmg));
                if (temp2 < 0)
                {
                    side[temp] = 3;
                    eDmg += Rand(1,4);
                    eSize += Rand(2,8);
                    eHealth += Rand(1,5);
                }
            }
            else if (diff[temp] == 2)
            {
                temp2 = Rand(-(eSize + eDmg), Rand(120, 150) + Rand(50, 70));
                if (temp2 < 0)
                {
                    side[temp] = 3;
                    eDmg += Rand(5,12);
                    eSize += Rand(10,21);
                    eHealth += Rand(6,15);
                }
            }
            else
            {
                temp2 = Rand(-(eSize + eDmg), Rand(450, 850) + Rand(100, 130));
                if (temp2 < 0)
                {
                    side[temp] = 3;
                    eDmg += Rand(12,24);
                    eSize += Rand(20,42);
                    eHealth += Rand(15,30);
                }
            }
        }



        // ############################################################################
        //                Update
        // ############################################################################
        public override void Update()
        {
            MouseState mState = Mouse.GetState();
            ColorChange();
            

            for(int i = 0; i < town_amount; i++)
            {
                if(mState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    if(Collision(mState, towns[i], diff[i]) && side[i] > 1) // Clicking
                    {
                        fought_town = i;
                        if (owned_palces < 2)
                        {
                            if (diff[i] == 1)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(1, 35), Rand(5, 20), Rand(50, 150), false);
                                soldiers.Game = 1;
                            }
                        }
                        else if (owned_palces > 1 && side[i] > 1 && Available(i, 1))
                        {
                            if(side[i] == 3)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(eSize, eDmg, eHealth, true);
                                soldiers.Game = 1;
                            }
                            else if (diff[i] == 1)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(1, 35), Rand(5, 20), Rand(50, 150), false);
                                soldiers.Game = 1;
                            }
                            else if (diff[i] == 2)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(120, 150), Rand(50, 70), Rand(150, 250), false);
                                soldiers.Game = 1;
                            }
                            else if (diff[i] == 3)
                            {
                                Effects.Clear_blood();
                                soldiers.Fight(Rand(550, 950), Rand(200, 230), Rand(350, 450), false);
                                soldiers.Game = 1;
                            }
                        }
                    }
                }
            }
            oldState = mState;
            Owned_Places();
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
        //                Owned places
        // ############################################################################
        private static void Owned_Places()
        {
            owned_palces = 1;
            bOwned_places = 0;
            for(int i = 0; i < town_amount; i++)
            {
                if(side[i] == 1)
                {
                    owned_palces++;
                }
                else if(side[i] == 3)
                {
                    bOwned_places++;
                }
            }
        }


        // ############################################################################
        //                Draw
        // ############################################################################
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < town_amount; i++)
            {
                if (Available(i, 1) && side[i] > 1)
                {
                    if (diff[i] == 3)
                    {
                        spriteBatch.Draw(big, new Rectangle((int)towns[i].X - 2, (int)towns[i].Y - 4, (diff[i] * town_size) + 4, (diff[i] * town_size) + 6), Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(medium, new Rectangle((int)towns[i].X - 2, (int)towns[i].Y - 4, (diff[i] * town_size) + 4, (diff[i] * town_size) + 6), Color.Black);
                    }
                }

                if (diff[i] == 3)
                {
                    spriteBatch.Draw(big, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i] * town_size, diff[i] * town_size), sidec[i]);
                }
                else
                {
                    spriteBatch.Draw(medium, new Rectangle((int)towns[i].X, (int)towns[i].Y, diff[i] * town_size, diff[i] * town_size), sidec[i]);
                }
            }
            Bottom_Bar(spriteBatch);
        }


        // ############################################################################
        //                Collision
        // ############################################################################
        public void Town_Move()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int i = 0; i < town_amount; i++)
                {
                    for (int o = 0; o < town_amount; o++)
                    {
                        if (Collision(towns[i], towns[o], diff[o], diff[i]))
                        {
                            while (Collision(towns[i], towns[o], diff[o], diff[i])) // Towns not on eachother
                            {
                                towns[i] = new Vector2(Rand(10, 760), Rand(10, 300));
                            }
                        }
                    }
                }
            }
        }

        public static bool Available(int which , int team) // 1 == red; 3 == blue
        {
            for(int i = 0; i < town_amount; i++)
            {
                if (side[i] == team)
                {
                    if (towns[which].X < towns[i].X + 80 && towns[which].X > towns[i].X - 80)
                    {
                        if (towns[which].Y < towns[i].Y + 80 && towns[which].Y > towns[i].Y - 80)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Collision(MouseState m, Vector2 b, int d)
        {
            if (m.X > b.X && m.X < b.X + d * town_size && m.Y > b.Y && m.Y < b.Y + d * town_size)
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
            if (CollisionCheck(b1 + new Vector2(0, 0), b2 + new Vector2(0, 0), d2, d1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CollisionCheck(Vector2 b1, Vector2 b2, int d2, int d1)
        {
            /*
             * _____
             * |  _|___
             * |__||  |
             *    |___|
             */
            if (b1.X > b2.X && b1.X < b2.X + d2 * town_size && b1.Y > b2.Y && b1.Y < b2.Y + d2 * town_size)
            {
                return true;
            }
            /*  ___
             * _|_|_
             * ||_||
             * |___|
             *    
             */
            else if (b1.X > b2.X && b1.X < b2.X + d2 * town_size && b1.Y + d1 * town_size > b2.Y && b1.Y + d1 * town_size < b2.Y + d2 * town_size)
            {
                return true;
            }
            /*  ______
             * _|___ |
             * ||__|_|
             * |___|
             *    
             */
            else if (b1.X > b2.X && b1.X < b2.X + d2 * town_size && b1.Y < b2.Y && b1.Y * town_size > b2.Y)
            {
                return true;
            }
            /*
             *    _____
             *   _|_  |
             *   |_|  |
             *    |___|
             */
            else if (b1.X + d1 * town_size > b2.X && b1.X < b2.X && b1.Y > b2.Y && b1.Y < b2.Y + d2 * town_size)
            {
                return true;
            }
            else if (b1.X > b2.X && b1.X < b2.X + d2 * town_size && b1.Y > b2.Y && b1.Y < b2.Y + d2 * town_size)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // ############################################################################
        //                Color Change
        // ############################################################################
        private static void ColorChange()
        {
            for(int i = 0; i < town_amount; i++)
            {
                if(side[i] == 1)
                {
                    sidec[i] = Color.Red;
                }
                else if (side[i] == 2)
                {
                    sidec[i] = Color.White;
                }
                else if (side[i] == 3)
                {
                    sidec[i] = Color.Blue;
                }
            }
        }

        // ############################################################################
        //                Bottom bar
        // ############################################################################
        private void Bottom_Bar(SpriteBatch spritebatch)
        {
            spritebatch.Draw(tex, new Rectangle(0, 350, 820, 180), Color.SlateGray);
            spritebatch.Draw(tex, new Rectangle(0, 350, 820, 5), Color.DarkGray);

            if(bOwned_places > 0 && owned_palces > 1)
            {
                temp = (bOwned_places + (owned_palces - 1));
                if (bOwned_places > owned_palces - 1)
                {
                    fTemp = (bOwned_places * 1000) / temp;
                    redChange = 1 - (fTemp * 0.001f);
                    blueChange = fTemp * 0.001f;
                }
                else
                {
                    fTemp = ((owned_palces - 1) * 1000) / temp;
                    redChange = fTemp * 0.001f;
                    blueChange = 1 - (fTemp * 0.001f);
                }
            }
            if(redChange <= 0)
            {
                redChange = 0.5f;
                blueChange = 0.5f;
            }
            spritebatch.Draw(tex, new Rectangle(140, 420, (int)(525 * redChange), 5), Color.Red);
            spritebatch.Draw(tex, new Rectangle((int)(525 * redChange) + 140, 420, (int)(525 * blueChange), 5), Color.Blue);

            // Red houses
            temp = owned_palces - 1;
            spritebatch.DrawString(text, "Red towns: " + temp, new Vector2(10, 405), Color.Black);
            spritebatch.Draw(tex, new Rectangle(10, 425, 125, 2), Color.DarkGray);

            //Blue houses
            temp = bOwned_places;
            spritebatch.DrawString(text, "Blue towns: " + temp, new Vector2(670, 405), Color.Black);
            spritebatch.Draw(tex, new Rectangle(670, 425, 125, 2), Color.DarkGray);
        }
    }
}
