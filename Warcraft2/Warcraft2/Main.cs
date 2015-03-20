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
using Warcraft2.Application.Networking;
using Warcraft2.Application.Core;
using Warcraft2.Application.Graphics;
using Warcraft2.Application.Core.Players;
using System.Reflection;
using System.Windows.Forms;

namespace Warcraft2
{

    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Timer timer;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Engine.CreateGameEngine();
            IntPtr hWnd = this.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            InactiveSleepTime = new TimeSpan(0);
            GameScreen.Setup(graphics, control.FindForm());
            GameScreen.SetWindowSize(800, 800);
            GameScreen.SetBorderlessWindow(false);
            GameScreen.SetFullscreen(false);
            TileType.Setup();

            // Allows game to run while not focus'd and dragging
            object host = typeof(Game).GetField("host", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            host.GetType().BaseType.GetField("Suspend", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(host, null);
            host.GetType().BaseType.GetField("Resume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(host, null);
            timer = new Timer();
            timer.Interval = (int)(this.TargetElapsedTime.TotalMilliseconds);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        bool manualTick;
        int manualTickCount = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            if(manualTickCount > 2)
            {
                manualTick = true;
                this.Tick();
                manualTick = false;
            }

            manualTickCount++;
        }


        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
            Engine.renderer = new Renderer(new RailorLibrary.Graphics.AssetManager(Content), graphics.GraphicsDevice);
            Engine.playerController = new PlayerController();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if(!manualTick)
                manualTickCount = 0;

            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            RailorLibrary.Input.InputManager.UpdateStates(Keyboard.GetState(), Mouse.GetState());
            PlayerControllerCommands.RaiseEvents();

            Engine.Update(gameTime);
            //control2.NetworkProcess();
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Engine.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
