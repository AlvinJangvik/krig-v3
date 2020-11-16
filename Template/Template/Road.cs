using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Template
{
    class Road : VisualBase
    {
        private static int num = 100000;
        private static Color[] dirt = new Color[num];
        private static Vector2[] pos = new Vector2[num];
        private static bool[] active = new bool[num];
        private static int[] used_town;
        private static Vector2 aim;
        private static Vector2 temp;
        private static int temp2;
        private static int temp3;
        private static int used = 0;
        private static int reset = 0;
        public Road(Texture2D skin)
        {
            tex = skin;
            Dirt_Color();
        }

        public static void Create(Vector2[] towns, int amount)
        {
            Reset(amount);
            for(int i = 0; i < amount; i++)
            {
                for(int o = 0; o < amount; o++)
                {
                    if(Menu.Rand(0, 100) == 10 && i != o)
                    {
                        temp2 = Menu.Rand(1, 10);
                        if (temp2 < 2)
                        {
                            if (Used_town_check(i, o))
                            {
                                Console.WriteLine("No road");
                            }
                            else
                            {
                                Lay(towns, i, o);
                            }
                        }
                        else if(temp2 > 8)
                        {
                            Used_town_add(i, o);
                        }
                    }
                }
            }
        }

        public static void Lay(Vector2[] towns, int first, int second)
        {
            Used_town_add(first, second);
            temp2 = 0;
            aim = towns[second] + new Vector2(5, 5);
            temp = towns[first] + new Vector2(5, 5);
            temp3 = 0;
            if(temp.X < aim.X)
            {
                temp3 += (int)(aim.X - temp.X);
            }
            else
            {
                temp3 += (int)(temp.X - aim.X);
            }
            if (temp.Y < aim.Y)
            {
                temp3 += (int)(aim.Y - temp.Y);
            }
            else
            {
                temp3 += (int)(temp.Y - aim.Y);
            }


            while (true)
            {
                if(temp.X < aim.X)
                {
                    temp.X += 1;
                }
                else if (temp.X > aim.X)
                {
                    temp.X -= 1;
                }
                if (temp.Y < aim.Y)
                {
                    temp.Y += 1;
                }
                else if (temp.Y > aim.Y)
                {
                    temp.Y -= 1;
                }

                pos[used] = temp;
                active[used] = true;

                if (pos[used].X < aim.X + 10 && pos[used].X > aim.X - 10 && pos[used].Y < aim.Y + 10 && pos[used].Y > aim.Y - 10)
                {
                    break;
                }
                else if (temp2 == temp3 + 15)
                {
                    break;
                }

                if (used == num - 1)
                {
                    used = 0;
                }

                used++;
                temp2++;
            }
        }

        public static void Used_town_reset()
        {
            for(int i = 0; i < used_town.Length; i++)
            {
                used_town[i] = -1;
            }
        } 

        public static void Used_town_add(int first, int Second)
        {
            for(int i = 0; i < used_town.Length; i++)
            {
                if(used_town[i] == -1)
                {
                    used_town[i] = first;
                    if (i < used_town.Length - 1)
                    {
                        used_town[i + 1] = first;
                    }
                    break;
                }
            }
        }

        public static bool Used_town_check(int first, int second)
        {
            temp2 = 0;
            for(int i = 0; i < used_town.Length; i++)
            {
                if(used_town[i] == first || used_town[i] == second)
                {
                    temp2 = 2;
                    return true;
                }
            }
            if (temp2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < num; i++)
            {
                if(active[i] == true)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)pos[i].X, (int)pos[i].Y, 2, 2), dirt[i]);
                }
            }
        }

        private static void Dirt_Color()
        {
            for(int i = 0; i < num; i++)
            {
                temp2 = Menu.Rand(1, 3);
                if(temp2 == 1)
                {
                    dirt[i] = Color.Black;
                }
                else if (temp2 == 2)
                {
                    dirt[i] = Color.DarkGreen;
                }
                else if (temp2 == 3)
                {
                    dirt[i] = Color.Green;
                }
            }
        }

        private static void Reset(int amount)
        {
            used_town = new int[amount];
            if (reset == 0)
            {
                Used_town_reset();
            }
        }
    }
}
