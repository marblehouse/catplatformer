using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace cat_platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Player mainPlayer;
        SpriteFont font;
        double timeSurvived = 0; 

        Texture2D ringTexture;
        Point ringFrameSize = new Point(75, 75);
        Point ringCurrentFrame = new Point(0, 0);
        Point ringSheetSize = new Point(6, 8);
        int ringTimeSinceLastFrame = 0;
        int ringMillisecondsPerFrame = 50;
        Vector2 ringPosition = Vector2.Zero;
        const float ringSpeed = 8;
        int ringCollisionRectOffset  = 10;

        

        Texture2D skullTexture;
        Point skullFrameSize = new Point(75, 75);
        Point skullCurrentFrame = new Point(0, 0);
        Point skullSheetSize = new Point(6, 8);
        Vector2 skullPosition = new Vector2(100, 100);
        int skullTimeSinceLastFrame = 0;
        int skullMillisecondsPerFrame = 50;
        Vector2 skullSpeed = new Vector2(10, 10);
        int skullCollisionRectOffset = 10; 

        MouseState prevMouseState;


        bouncingSkull[] skulls; 
         

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
            // TODO: Add your initialization logic here
            skulls = new bouncingSkull[5]; 
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
            font = Content.Load<SpriteFont>("myFont");
            ringTexture  = Content.Load<Texture2D>("threerings");
            skullTexture = Content.Load<Texture2D>("skullball");

            skulls[0] = new bouncingSkull(Content); 
            
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            skulls[0].animateBall(gameTime);
            skulls[0].moveBall(Window); 
            


            ringTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (ringTimeSinceLastFrame > ringMillisecondsPerFrame)
            {
                ringTimeSinceLastFrame -= ringMillisecondsPerFrame; 

                ++ringCurrentFrame.X;
                if (ringCurrentFrame.X >= ringSheetSize.X)
                {
                    ringCurrentFrame.X = 0;
                    ++ringCurrentFrame.Y;
                    if (ringCurrentFrame.Y >= ringSheetSize.Y)
                        ringCurrentFrame.Y = 0;
                }
            }

            skullTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (skullTimeSinceLastFrame > skullMillisecondsPerFrame)
            {
                skullTimeSinceLastFrame -= skullMillisecondsPerFrame;

                ++skullCurrentFrame.X;
                if (skullCurrentFrame.X >= skullSheetSize.X)
                {
                    skullCurrentFrame.X = 0;
                    ++skullCurrentFrame.Y;
                    if (skullCurrentFrame.Y >= skullSheetSize.Y)
                        skullCurrentFrame.Y = 0;
                }
            }

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
                ringPosition.Y -= ringSpeed;
            if (keyboardState.IsKeyDown(Keys.A))
                ringPosition.X -= ringSpeed;
            if (keyboardState.IsKeyDown(Keys.D))
                ringPosition.X += ringSpeed;
            if (keyboardState.IsKeyDown(Keys.S))
                ringPosition.Y += ringSpeed;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.X != prevMouseState.X ||
                mouseState.Y != prevMouseState.Y)
                ringPosition = new Vector2(mouseState.X, mouseState.Y);
            prevMouseState = mouseState;

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                ringPosition.X += ringSpeed * 2 * gamePadState.ThumbSticks.Left.X;
                ringPosition.Y -= ringSpeed * 2 * gamePadState.ThumbSticks.Left.Y;
                GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
            }
            else
            {
                ringPosition.X += ringSpeed * gamePadState.ThumbSticks.Left.X;
                ringPosition.Y -= ringSpeed * gamePadState.ThumbSticks.Left.Y;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }

            skullPosition += skullSpeed;

            if (skullPosition.X < 0 || skullPosition.X > Window.ClientBounds.Width - skullFrameSize.X)
                skullSpeed.X *= -1.0f;
            if (skullPosition.Y < 0 || skullPosition.Y > Window.ClientBounds.Height - skullFrameSize.Y)
                skullSpeed.Y *= -1.0f;

            if (ringPosition.X < 0)
                ringPosition.X = 0;
            if (ringPosition.Y < 0)
                ringPosition.Y = 0;
            if (ringPosition.X > Window.ClientBounds.Width - ringFrameSize.X)
                ringPosition.X = Window.ClientBounds.Width - ringFrameSize.X;
            if (ringPosition.Y > Window.ClientBounds.Height - ringFrameSize.Y)
                ringPosition.Y = Window.ClientBounds.Height - ringFrameSize.Y;

            if (Collide())
            {
                ringPosition.X = 0;
                ringPosition.Y = 0;
                timeSurvived = 0; 
            }

            timeSurvived += gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            string temp = "Survived: " + timeSurvived.ToString("##.##") + "s"; 
            
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            //mainPlayer.Draw(spriteBatch, Vector2.Zero);
            spriteBatch.DrawString(font, temp, new Vector2(50,  0), Color.Red);
            //spriteBatch.DrawString(font, "Speed Y: " + mainPlayer._spriteSpeed.Y, new Vector2(250, 0), Color.White);
            //spriteBatch.DrawString(font, "Posit X: " + mainPlayer._spritePosition.X, new Vector2(50, 40), Color.White);
            //spriteBatch.DrawString(font, "Posit Y: " + mainPlayer._spritePosition.Y, new Vector2(250,40), Color.White);

            spriteBatch.Draw(ringTexture, ringPosition,
                new Rectangle(ringCurrentFrame.X * ringFrameSize.X,
                    ringCurrentFrame.Y * ringFrameSize.Y,
                    ringFrameSize.X,
                    ringFrameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1, SpriteEffects.None, 0);

            spriteBatch.Draw(skullTexture, skullPosition,
                new Rectangle(skullCurrentFrame.X * skullFrameSize.X,
                    skullCurrentFrame.Y * skullFrameSize.Y,
                    skullFrameSize.X,
                    skullFrameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1, SpriteEffects.None, 0);

            skulls[0].drawBall(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected bool Collide()
        {
            Rectangle ringRect = new Rectangle((int)ringPosition.X + ringCollisionRectOffset,
                                               (int)ringPosition.Y + ringCollisionRectOffset,
                                               ringFrameSize.X - (2*ringCollisionRectOffset),
                                               ringFrameSize.Y - (2 * ringCollisionRectOffset));
            Rectangle skullRect = new Rectangle((int)skullPosition.X + skullCollisionRectOffset,
                                                (int)skullPosition.Y + skullCollisionRectOffset,
                                                skullFrameSize.X - (2*skullCollisionRectOffset),
                                                skullFrameSize.Y - (2*skullCollisionRectOffset));

            return (ringRect.Intersects(skullRect) || skulls[0].Collide(ringRect)); 
        }
    }
}
