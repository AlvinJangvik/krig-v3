using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Template.Content;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        soldiers soldier;
        Effects effects;
        Controll controll = new Controll();
        Menu menu;
        Map map;
        Road road;
        Clouds clouds;
        //KOmentar
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            soldiers.RedSoldierAmount = 500;
            soldiers.Fight(500, 15, 100, false);
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            soldier = new soldiers(Content.Load<Texture2D>("Devilman"), Content.Load<Texture2D>("Swordsman"), Content.Load<SoundEffect>("Explosion"));
            effects = new Effects(Content.Load<Texture2D>("svart"));
            menu = new Menu(Content.Load<Texture2D>("Button"), Content.Load<SpriteFont>("basic"), Content.Load<SoundEffect>("Select"));
            road = new Road(Content.Load<Texture2D>("svart"));
            map = new Map(Content.Load<Texture2D>("Castle"), Content.Load<Texture2D>("Tavern"), Content.Load<Texture2D>("svart"), Content.Load<SpriteFont>("basic"));
            clouds = new Clouds(Content.Load<Texture2D>("Cloud"));
            // TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            menu.Update();
            controll.Update();
            soldier.Update();
            effects.Update();
            if(soldiers.Game == 4)
            {
                clouds.Update();
                map.Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);
            spriteBatch.Begin();

            if (!Controll.Blood)
            {
                effects.Draw(spriteBatch);
            }
            soldier.Draw(spriteBatch);
            if (Controll.Blood)
            {
                effects.Draw(spriteBatch);
            }
            menu.Draw(spriteBatch);
            if(soldiers.Game == 4)
            {
                road.Draw(spriteBatch);
                map.Draw(spriteBatch);
                clouds.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
