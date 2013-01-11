using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReduDroidPhotoEditor
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    [IntentFilter(new[] {"EDIT_PHOTO"})]
    public sealed class PhotoEditingActivity : AndroidGameActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Game.Activity = this;

            PhotoEditing photoEditing = new PhotoEditing();

            SetContentView(photoEditing.Window);

            photoEditing.Run();
        }

        public class PhotoEditing : Game
        {
            private GraphicsDeviceManager graphics;
            private SpriteBatch spriteBatch;
            private Texture2D image;
            private Texture2D alphaChannel;
            private Vector2 screenResolution;
            private Vector2 displayScale;
            private Vector2 origin;
            private Vector2 center;
            private float rotation;

            public PhotoEditing()
            {
                screenResolution = Utils.GetScreenResolution(Activity);

                graphics = new GraphicsDeviceManager(this);
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = (int) screenResolution.X;
                graphics.PreferredBackBufferHeight = (int) screenResolution.Y;
                graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

                Content.RootDirectory = "Content";
            }

            protected override void LoadContent()
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);

                image = Texture2D.FromStream(GraphicsDevice,
                                             Utils.ByteArray2Stream(Activity.Intent.GetByteArrayExtra("imageBytes")));
                alphaChannel = Content.Load<Texture2D>("Images/alphaChannel");

                displayScale = image.Width <= screenResolution.X && image.Height <= screenResolution.Y
                                   ? Vector2.One
                                   : new Vector2(screenResolution.X/image.Width, screenResolution.Y/image.Height);
                origin = new Vector2(image.Width/2.0F, image.Height/2.0F);
                center = new Vector2((screenResolution.X - (image.Width*displayScale.X))/2,
                                     (screenResolution.Y - (image.Height*displayScale.Y))/2) + (origin*displayScale);
            }

            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                SamplerState state = GraphicsDevice.SamplerStates[0];
                state.AddressU = TextureAddressMode.Wrap;
                state.AddressV = TextureAddressMode.Wrap;

                spriteBatch.Draw(alphaChannel, Vector2.Zero,
                                 new Rectangle(0, 0, (int) screenResolution.X, (int) screenResolution.Y), Color.White);

                state.AddressU = TextureAddressMode.Clamp;
                state.AddressV = TextureAddressMode.Clamp;

                spriteBatch.Draw(image, center, null, Color.White, MathHelper.ToRadians(rotation), origin, displayScale,
                                 SpriteEffects.None, 0);
                spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}