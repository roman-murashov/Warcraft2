using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using RailorLibrary.Input;
using Warcraft2.Application.Graphics;

namespace Warcraft2.Application.Core.Players
{
    public static class PlayerControllerCommands
    {
        public static Bindable Jump = new Bindable(Keys.W, "Jump", "Jumps the character");
        public static Bindable EndTurn = new Bindable(Keys.Space, "Space", "Ends Turn");
        public static Bindable ForwardTurn = new Bindable(Keys.D, "Forward Turn", "Forward Turn");
        public static Bindable ReverseTurn = new Bindable(Keys.A, "Reverse Turn", "Reverse Turn");
        public static Bindable Down = new Bindable(Keys.S, "Down", "Moves the character down");
        public static Bindable LeftClick = new Bindable(MouseButtons.LeftClick, "Left Click", "Left Click");
        public static Bindable RightClick = new Bindable(MouseButtons.RightClick, "Right Click", "Right Click");
        public static Bindable Tab = new Bindable(Keys.Tab, "Tab", "Tab");
        public static Bindable ToggleInventory = new Bindable(Keys.B, "Toggle Inventory", "Toggles the display of the inventory");
        public static Bindable ArrowLeft = new Bindable(Keys.Left, "Left", "Left");
        public static Bindable ArrowRight = new Bindable(Keys.Right, "Right", "Right");
        public static Bindable ArrowUp = new Bindable(Keys.Up, "Up", "Up");
        public static Bindable ArrowDown = new Bindable(Keys.Down, "Down", "Down");

        public delegate void EventDelegate(Bindable.BindableState state);
        public static event EventDelegate JumpEvent;
        public static event EventDelegate ForwardTurnEvent;
        public static event EventDelegate ReverseTurnEvent;
        public static event EventDelegate EndTurnEvent;
        public static event EventDelegate DownEvent;
        public static event EventDelegate LeftClickEvent;
        public static event EventDelegate RightClickEvent;
        public static event EventDelegate TabEvent;
        public static event EventDelegate ToggleInventoryEvent;
        public static event EventDelegate ArrowLeftEvent;
        public static event EventDelegate ArrowRightEvent;
        public static event EventDelegate ArrowDownEvent;
        public static event EventDelegate ArrowUpEvent;


        public static int GetMouseX()
        {
            return InputManager.GetMouseX();
        }

        public static int GetMouseY()
        {
            return InputManager.GetMouseY();
        }

        public static Point GetMousePoint()
        {
            return new Point(InputManager.GetMouseX(), InputManager.GetMouseY());
        }

        public static int GetGameMouseX()
        {
            return GetMouseX() + GameScreen.GetPosition().X - GameScreen.GetPlayableRectangle().X;
        }

        public static int GetGameMouseY()
        {
            return GetMouseY() + GameScreen.GetPosition().Y - GameScreen.GetPlayableRectangle().Y;
        }

        public static Point GetGameMousePoint()
        {
            return new Point(GetGameMouseX(), GetGameMouseY());
        }

        public static int GetMouseScrolled()
        {
            return InputManager.GetMouseScrolled() / 120;
        }

        public static void RaiseEvents()
        {
            if(Jump.GetState() != Bindable.BindableState.None) { RaiseEvent(JumpEvent, Jump.GetState()); }
            if(LeftClick.GetState() != Bindable.BindableState.None) { RaiseEvent(LeftClickEvent, LeftClick.GetState()); }
            if(RightClick.GetState() != Bindable.BindableState.None) { RaiseEvent(RightClickEvent, RightClick.GetState()); }
            if(EndTurn.GetState() != Bindable.BindableState.None) { RaiseEvent(EndTurnEvent, EndTurn.GetState()); }
            if(ForwardTurn.GetState() != Bindable.BindableState.None) { RaiseEvent(ForwardTurnEvent, ForwardTurn.GetState()); }
            if(ReverseTurn.GetState() != Bindable.BindableState.None) { RaiseEvent(ReverseTurnEvent, ReverseTurn.GetState()); }
            if(Down.GetState() != Bindable.BindableState.None) { RaiseEvent(DownEvent, Down.GetState()); }
            if(Tab.GetState() != Bindable.BindableState.None) { RaiseEvent(TabEvent, Tab.GetState()); }
            if(ToggleInventory.GetState() != Bindable.BindableState.None) { RaiseEvent(ToggleInventoryEvent, ToggleInventory.GetState()); }
            if(ArrowLeft.GetState() != Bindable.BindableState.None) { RaiseEvent(ArrowLeftEvent, ArrowLeft.GetState()); }
            if(ArrowRight.GetState() != Bindable.BindableState.None) { RaiseEvent(ArrowRightEvent, ArrowRight.GetState()); }
            if(ArrowUp.GetState() != Bindable.BindableState.None) { RaiseEvent(ArrowUpEvent, ArrowUp.GetState()); }
            if(ArrowDown.GetState() != Bindable.BindableState.None) { RaiseEvent(ArrowDownEvent, ArrowDown.GetState()); }

        }

        static void RaiseEvent(EventDelegate e, Bindable.BindableState state)
        {
            // make sure we have subscribers to that event
            if(e != null)
            {
                e(state);
            }
        }
    }
}
