using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Warcraft2.Application.Graphics
{
    public static class GameScreen
    {
        static int screenWidth = 1200;
        static int screenHeight = 800;
        public static System.Windows.Forms.Form form;
        static Boolean fullscreen = false;
        static GraphicsDeviceManager graphics;
        static Rectangle gameRectangle;
        static Point currentPosition = new Point(0, 0);
        static int scrollSpeed = 8;
        static int marginLeft = 0;//20
        static int marginRight = 0;//20
        static int marginUp = 0;
        static int marginDown = 0;//100
        public static void Setup(GraphicsDeviceManager value2, System.Windows.Forms.Form value3)
        {
            graphics = value2;
            form = value3;
            gameRectangle = new Rectangle(200, 50, screenWidth - 200, screenHeight - 50);
        }

        public static int GetWidth()
        {
            if(fullscreen)
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            return screenWidth;
        }

        public static int GetHeight()
        {
            if(fullscreen)
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            return screenHeight;
        }

        public static void SetWindowSize(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            screenWidth = width;
            screenHeight = height;
            gameRectangle = new Rectangle(marginLeft, marginUp, width - marginLeft - marginRight, height - marginUp - marginDown);
        }

        public static void SetBorderlessWindow(Boolean value)
        {
            if(value)
            {
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                form.Width = screenWidth;
                form.Height = screenHeight;
            }
            else
            {
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            }

        }

        public static Boolean IsFullScreen()
        {
            return fullscreen;
        }

        public static void SetFullscreen(Boolean value)
        {
            if(value && !fullscreen)
            {
                fullscreen = true;
                SetBorderlessWindow(true);
                form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                SetWindowSize(GetWidth(), GetHeight());
            }
            else if(!value && fullscreen)
            {
                fullscreen = false;
                SetBorderlessWindow(false);
                SetWindowSize(screenWidth, screenHeight);
                form.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }

        public static Rectangle GetPlayableRectangle()
        {
            return gameRectangle;
        }

        public static Point GetPosition()
        {
            return currentPosition;
        }

        public static void AddPosition(int x, int y)
        {
            currentPosition.X += x * scrollSpeed;
            currentPosition.Y += y * scrollSpeed;
        }

        public static int GetMarginLeft() { return marginLeft; }
        public static int GetMarginRight() { return marginRight; }
        public static int GetMarginUp() { return marginUp; }
        public static int GetMarginDown() { return marginDown; }
    }
}
