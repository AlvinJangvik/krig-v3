﻿using System;
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
    class Menu : VisualBase
    {
        private SpriteFont text;
        private SoundEffect select;
        private MouseState oldMState;
        private static Random rand = new Random();

        //Money ints
        private static int money = 0;
        private int dmg_price;
        private int amount_price = 0;
        private int hp_price = 0;

        // Button vectors
        private Vector2 button_size = new Vector2(130, 40);
        private Vector2 button_text = new Vector2(40, 10);
        // Starts
        private Vector2 game_start = new Vector2(350, 200);
        private Vector2 open_map = new Vector2(650, 410);
        // Upgrades
        private Vector2 up_Dmg  = new Vector2(20, 80);
        private Vector2 up_Amount = new Vector2(20, 140);
        private Vector2 up_Hp = new Vector2(20, 200);


        // ############################################################################
        //                Constructor
        // ############################################################################
        public Menu(Texture2D skin, SpriteFont sprf, SoundEffect sound)
        {
            tex = skin;
            text = sprf;
            select = sound;
        }


        // ############################################################################
        //                Win
        // ############################################################################
        public static void Win()
        {
            money += (soldiers.BlueDmgValue / 2) + (soldiers.BlueSoldierAmount / 2);
        }


        // ############################################################################
        //                Update
        // ############################################################################
        public override void Update()
        {
            if(soldiers.Game == 3)
            {
                MouseState mState = Mouse.GetState();

                if (mState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
                {
                    if (Collision(mState, game_start))
                    {
                        Road.Create(Map.towns, Map.town_amount);
                        select.Play(0.1f, 1, 0);
                        Effects.Clear_blood();
                        soldiers.RedSoldierAmount = 10;
                        soldiers.Game = 2;
                    }
                }

                oldMState = mState;
            }
            if (soldiers.Game == 2)
            {
                MouseState mState = Mouse.GetState();
                dmg_price = soldiers.RedDmgValue;
                amount_price = soldiers.RedSoldierAmount;
                hp_price = (int)((double)soldiers.RedHealthValue * 0.75);

                if (mState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
                {
                    // ######
                    // STARTS
                    if(Collision(mState, open_map))
                    {
                        /* Effects.Clear_blood();
                        select.Play(0.1f, 1, 0);
                        soldiers.Fight(Rand(1, 35), Rand(5, 20), Rand(50, 150));
                        soldiers.Game = 1; */
                        money += 12 * (Map.OwnedPlaces / 5);
                        select.Play(0.1f, 1, 0);
                        Effects.Clear_blood();
                        soldiers.Game = 4;
                    }

                    // ########
                    // UPGRADES
                    if (Collision(mState, up_Dmg) && money >= dmg_price)
                    {
                        select.Play(0.1f, 1, 0);
                        soldiers.RedDmgValue += 5;
                        money -= dmg_price;
                    }
                    else if (Collision(mState, up_Amount) && money >= amount_price)
                    {
                        select.Play(0.1f, 1, 0);
                        soldiers.RedSoldierAmount += 10;
                        money -= amount_price;
                    }
                    else if (Collision(mState, up_Hp) && money >= hp_price)
                    {
                        select.Play(0.1f, 1, 0);
                        soldiers.RedHealthValue += 10;
                        money -= hp_price;
                    }
                }

                oldMState = mState;
            }
        }


        // ############################################################################
        //                Draw
        // ############################################################################
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(soldiers.Game == 2)
            {
                // Money
                spriteBatch.DrawString(text, "Money: " + money + "$", new Vector2(10, 10), Color.Black);

                // Start button
                if (Collision(oldMState, open_map))
                {
                    spriteBatch.Draw(tex, new Rectangle((int)open_map.X - 6, (int)open_map.Y - 4, (int)button_size.X + 15, (int)button_size.Y + 12), Color.Gold);
                    spriteBatch.DrawString(text, "Map", open_map + button_text, Color.Black);
                }
                
                spriteBatch.Draw(tex, new Rectangle((int)open_map.X, (int)open_map.Y, (int)button_size.X, (int)button_size.Y), Color.NavajoWhite);
                spriteBatch.DrawString(text, "Map", open_map + button_text, Color.Black);

                // Upgrade buttons
                spriteBatch.DrawString(text, "Upgrades", new Vector2(20, 55), Color.Black);

                if (!Collision(oldMState, up_Dmg))
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Dmg.X, (int)up_Dmg.Y, (int)button_size.X, (int)button_size.Y), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Dmg", up_Dmg + button_text, Color.Black);
                    spriteBatch.DrawString(text, dmg_price + "$", up_Dmg + button_text + new Vector2(100, 0), Color.White);
                }
                else
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Dmg.X - 2, (int)up_Dmg.Y - 2, (int)button_size.X + 4, (int)button_size.Y + 4), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Dmg", up_Dmg + button_text, Color.Black);
                    spriteBatch.DrawString(text, dmg_price + "$", up_Dmg + button_text + new Vector2(100, 0), Color.White);
                }

                if (!Collision(oldMState, up_Amount))
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Amount.X, (int)up_Amount.Y, (int)button_size.X, (int)button_size.Y), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Size", up_Amount + button_text, Color.Black);
                    spriteBatch.DrawString(text, amount_price + "$", up_Amount + button_text + new Vector2(100, 0), Color.White);
                }
                else
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Amount.X - 2, (int)up_Amount.Y - 2, (int)button_size.X + 4, (int)button_size.Y + 4), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Size", up_Amount + button_text, Color.Black);
                    spriteBatch.DrawString(text, amount_price + "$", up_Amount + button_text + new Vector2(100, 0), Color.White);
                }

                if (!Collision(oldMState, up_Hp))
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Hp.X, (int)up_Hp.Y, (int)button_size.X, (int)button_size.Y), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Hp", up_Hp + button_text, Color.Black);
                    spriteBatch.DrawString(text, hp_price + "$", up_Hp + button_text + new Vector2(100, 0), Color.White);
                }
                else
                {
                    spriteBatch.Draw(tex, new Rectangle((int)up_Hp.X - 2, (int)up_Hp.Y - 2, (int)button_size.X + 4, (int)button_size.Y + 4), Color.NavajoWhite);
                    spriteBatch.DrawString(text, "Hp", up_Hp + button_text, Color.Black);
                    spriteBatch.DrawString(text, hp_price + "$", up_Hp + button_text + new Vector2(100, 0), Color.White);
                }
            }
            if (soldiers.Movement_speed > 1 && soldiers.Game == 1 || soldiers.Movement_speed > 1 && soldiers.Game == 2)
            {
                spriteBatch.DrawString(text, "GameSpeed: " + soldiers.Movement_speed + "X", new Vector2(650, 10), Color.Black);
            }
            if(soldiers.Game == 3)
            {
                if (Collision(oldMState, game_start))
                {
                    spriteBatch.Draw(tex, new Rectangle((int)game_start.X - 6, (int)game_start.Y - 4, (int)button_size.X + 15, (int)button_size.Y + 12), Color.Gold);
                }

                spriteBatch.Draw(tex, new Rectangle((int)game_start.X, (int)game_start.Y, (int)button_size.X, (int)button_size.Y), Color.NavajoWhite);
                spriteBatch.DrawString(text, "Start", game_start + button_text, Color.Black);
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
        //                Collision
        // ############################################################################
        public bool Collision(MouseState m, Vector2 b)
        {
            if (m.X > b.X && m.X < b.X + button_size.X && m.Y > b.Y && m.Y < b.Y + button_size.Y)
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
