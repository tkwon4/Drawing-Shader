using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    public class FinalProject : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //******View Projection*************
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0,
        0), Vector3.UnitY);
        Matrix projection =
        Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f,
        100f);

        //******Camera*************
        Vector3 cameraPosition, cameraTarget;
        float angle, angle2, distance;
        MouseState preMouse;
        KeyboardState preKeyboard;

        //******Models*********************
        Model currentModel;
        Model torus;
        Model bunny;
        Model paintPlane;


        Effect effect;


        float mouseX;
        float mouseY;
        bool canPaint = false;
        int width = 400;
        int height = 300;

        float red = 0;
        float green = 0;
        float blue = 1;

        float radius = 0.01f;

        SpriteFont font;

        RenderTarget2D renderTarget1;
        Texture2D renderTexture;
        Texture2D texture;
        Texture2D resetTexture;

        bool isReset = false;

        bool instructions = true;
        bool information = false;


        VertexPositionTexture[] vertices =
        {
            new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
            new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
            new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1))
        };

        public FinalProject()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            torus = Content.Load<Model>("torus");
            bunny = Content.Load<Model>("bunnyUV");
            paintPlane = Content.Load<Model>("Plane");
            currentModel = paintPlane;

            font = Content.Load<SpriteFont>("font");

            texture = new Texture2D(GraphicsDevice, width, height);
            resetTexture = new Texture2D(GraphicsDevice, width, height);


            angle = angle2 = 0;
            distance = 20;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderTarget1 = new RenderTarget2D(GraphicsDevice, width,
            height, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //*************Camera*******************8
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                angle2 -= 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                angle2 += 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                angle -= 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                angle += 0.05f;
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                distance += (Mouse.GetState().X - preMouse.X) / 100f;
            }
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                Vector3 ViewRight = Vector3.Transform(Vector3.UnitX,
                Matrix.CreateRotationX(angle2) *
                Matrix.CreateRotationY(angle));
                Vector3 ViewUp = Vector3.Transform(Vector3.UnitY,
                Matrix.CreateRotationX(angle2) *
                Matrix.CreateRotationY(angle));
                cameraTarget -= ViewRight * (Mouse.GetState().X - preMouse.X) /
                10f;
                cameraTarget += ViewUp * (Mouse.GetState().Y - preMouse.Y) / 10f;
            }
            cameraPosition = Vector3.Transform(new Vector3(0, 0, distance),
            Matrix.CreateRotationX(angle2) * Matrix.CreateRotationY(angle) *
            Matrix.CreateTranslation(cameraTarget));
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget,
            Vector3.Transform(Vector3.UnitY,
            Matrix.CreateRotationX(angle2) * Matrix.CreateRotationY(angle)));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);

            //Control Model
            if(Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                currentModel = paintPlane;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                currentModel = torus;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                currentModel = bunny;
            }

            if(Mouse.GetState().X <= width && Mouse.GetState().Y <= height)
            {
                mouseX = Mouse.GetState().X;
                mouseY = Mouse.GetState().Y;
            }

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                canPaint = true;
            }
            else
            {
                canPaint = false;
            }

            //Change Color
            if (Keyboard.GetState().IsKeyDown(Keys.R) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift)) red = MathHelper.Clamp(red + 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.R) && Keyboard.GetState().IsKeyDown(Keys.LeftShift)) red = MathHelper.Clamp(red - 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.G) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift)) green = MathHelper.Clamp(green + 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.G) && Keyboard.GetState().IsKeyDown(Keys.LeftShift)) green = MathHelper.Clamp(green - 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.B) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift)) blue = MathHelper.Clamp(blue + 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.B) && Keyboard.GetState().IsKeyDown(Keys.LeftShift)) blue = MathHelper.Clamp(blue - 0.01f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift)) radius = MathHelper.Clamp(radius + 0.001f, 0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.P) && Keyboard.GetState().IsKeyDown(Keys.LeftShift)) radius = MathHelper.Clamp(radius - 0.001f, 0, 1);

            //Erase
            if(Keyboard.GetState().IsKeyDown(Keys.E))
            {
                red = 0;
                green = 0;
                blue = 0;
            }

            //Reset
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                isReset = true;
            }
            else
            {
                isReset = false;
            }

            //Toggle UI
            if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion) &&
            preKeyboard.IsKeyUp(Keys.OemQuestion))
            {
                if (instructions == true)
                {
                    instructions = false;
                }
                else
                {
                    instructions = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H) &&
            preKeyboard.IsKeyUp(Keys.H))
            {
                if (information == true)
                {
                    information = false;
                }
                else
                {
                    information = true;
                }
            }

            preMouse = Mouse.GetState();
            preKeyboard = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // ************ TEMPLATE ************ //
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            // ************************************* //

            GraphicsDevice.SetRenderTarget(renderTarget1);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawRenderScene();
            GraphicsDevice.SetRenderTarget(null);
            renderTexture = (Texture2D)renderTarget1;
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            DrawModelScene(texture);

            Color[] data = new Color[width * height];
            renderTexture.GetData(data);
            texture.SetData(data);

            if (isReset)
            {
                Color[] resetData = new Color[width * height];
                resetTexture.GetData(resetData);
                texture.SetData(resetData);
            }

            _spriteBatch.Begin();
            if(information)
            {
                _spriteBatch.DrawString(font, "Red: " + red, new Vector2(10, 200), Color.White);
                _spriteBatch.DrawString(font, "Green: " + green, new Vector2(10, 250), Color.White);
                _spriteBatch.DrawString(font, "Blue: " + blue, new Vector2(10, 300), Color.White);
                _spriteBatch.DrawString(font, "Radius: " + radius, new Vector2(10, 350), Color.White);
            }

            if(instructions)
            {
                _spriteBatch.DrawString(font, "R/r: Increase Red Value (+ Shift key: decrease)", new Vector2(450, 50), Color.White);
                _spriteBatch.DrawString(font, "G/g: Increase Green Value (+ Shift key: decrease)", new Vector2(450, 100), Color.White);
                _spriteBatch.DrawString(font, "B/b: Increase Blue Value (+ Shift key: decrease)", new Vector2(450, 150), Color.White);
                _spriteBatch.DrawString(font, "P/p: Increase Radius (+ Shift key: decrease)", new Vector2(450, 200), Color.White);
                _spriteBatch.DrawString(font, "E: Set Eraser", new Vector2(450, 250), Color.White);
                _spriteBatch.DrawString(font, "Q: Reset Texture", new Vector2(450, 300), Color.White);
                _spriteBatch.DrawString(font, "?: Toggle Instructions", new Vector2(450, 350), Color.White);
                _spriteBatch.DrawString(font, "H: Toggle Data", new Vector2(450, 400), Color.White);
            }
            _spriteBatch.End();

            using (SpriteBatch sprite = new SpriteBatch(GraphicsDevice))
            {
                sprite.Begin();
                sprite.Draw(texture, Vector2.Zero, new Rectangle(0, 0, width, height), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                sprite.End();
            }

            

            renderTexture = null;

            base.Draw(gameTime);
        }

        private void DrawModelScene(Texture2D texture)
        {
            effect = Content.Load<Effect>("DepthMap");

            effect.CurrentTechnique = effect.Techniques[0];


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                foreach (ModelMesh mesh in currentModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        effect.Parameters["World"].SetValue(mesh.ParentBone.Transform);
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform));
                        effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        effect.Parameters["NewTexture"].SetValue(texture);

                        pass.Apply();
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            part.VertexOffset,
                            part.StartIndex,
                            part.PrimitiveCount);
                    }
                }
            }

        }

        private void DrawRenderScene()
        {
            effect = Content.Load<Effect>("RenderShader");

            effect.CurrentTechnique = effect.Techniques[0];
            effect.CurrentTechnique.Passes[0].Apply();
            effect.Parameters["Texture1"].SetValue(texture);
            effect.Parameters["canPaint"].SetValue(canPaint);
            effect.Parameters["MouseX"].SetValue(mouseX);
            effect.Parameters["MouseY"].SetValue(mouseY);
            effect.Parameters["Red"].SetValue(red);
            effect.Parameters["Green"].SetValue(green);
            effect.Parameters["Blue"].SetValue(blue);
            effect.Parameters["Radius"].SetValue(radius);

            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);

        }
    }
}