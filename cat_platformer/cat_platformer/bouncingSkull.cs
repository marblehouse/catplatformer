using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace cat_platformer
{
    class bouncingSkull
    {
        int skullCollisionRectOffset = 10;

        Texture2D skullTexture;
        Point skullFrameSize;
        Point skullCurrentFrame;
        Point skullSheetSize;
        Vector2 skullPosition;
        int skullTimeSinceLastFrame;
        int skullMillisecondsPerFrame;
        Vector2 skullSpeed;

        public bouncingSkull(ContentManager Content)
        {
            skullFrameSize = new Point(75, 75);
            skullCurrentFrame = new Point(0, 0);
            skullSheetSize = new Point(6, 8);
            skullPosition = new Vector2(500, 300);
            skullTimeSinceLastFrame = 0;
            skullMillisecondsPerFrame = 50;
            skullSpeed = new Vector2(9.0f, 10.0f);

            skullTexture = Content.Load<Texture2D>("skullball"); 
            
        }

        public void moveBall(GameWindow Window)
        {
            skullPosition += skullSpeed;

            if (skullPosition.X < 0 || skullPosition.X > Window.ClientBounds.Width - skullFrameSize.X)
                skullSpeed.X *= -1.0f;
            if (skullPosition.Y < 0 || skullPosition.Y > Window.ClientBounds.Height - skullFrameSize.Y)
                skullSpeed.Y *= -1.0f;
        }

        public void drawBall(SpriteBatch spriteBatch )
        {
            spriteBatch.Draw(skullTexture, skullPosition,
                new Rectangle(skullCurrentFrame.X * skullFrameSize.X,
                    skullCurrentFrame.Y * skullFrameSize.Y,
                    skullFrameSize.X,
                    skullFrameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1, SpriteEffects.None, 0);
        }

        public void animateBall(GameTime gameTime)
        {
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
        }

        public bool Collide(Rectangle arg)
        {

            Rectangle skullRect = new Rectangle((int)skullPosition.X + skullCollisionRectOffset,
                                                (int)skullPosition.Y + skullCollisionRectOffset,
                                                skullFrameSize.X - (2 * skullCollisionRectOffset),
                                                skullFrameSize.Y - (2 * skullCollisionRectOffset));

            return arg.Intersects(skullRect); 
        }
    }
}
