using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailorLibrary.Input;
using Warcraft2.Application.Graphics;
using Warcraft2.Application.Core.Players;

namespace Warcraft2.Application.Core.Players
{
    public class PlayerController
    {
        public PlayerController()
        {
            PlayerControllerCommands.LeftClickEvent += new PlayerControllerCommands.EventDelegate(HandleLeftClick);
            PlayerControllerCommands.RightClickEvent += new PlayerControllerCommands.EventDelegate(HandleRightClick);
            //PlayerControllerCommands.EndTurnEvent += new PlayerControllerCommands.EventDelegate(HandleEndTurn);
            //PlayerControllerCommands.ForwardTurnEvent += new PlayerControllerCommands.EventDelegate(HandleForwardTurn);
            //PlayerControllerCommands.ReverseTurnEvent += new PlayerControllerCommands.EventDelegate(HandleReverseTurn);
            //PlayerControllerCommands.TabEvent += new PlayerControllerCommands.EventDelegate(HandleTabDown);
            //PlayerControllerCommands.DownEvent += new PlayerControllerCommands.EventDelegate(HandleDownDown);
            PlayerControllerCommands.ArrowLeftEvent += new PlayerControllerCommands.EventDelegate(HandleArrowLeftDown);
            PlayerControllerCommands.ArrowRightEvent += new PlayerControllerCommands.EventDelegate(HandleArrowRightDown);
            PlayerControllerCommands.ArrowUpEvent += new PlayerControllerCommands.EventDelegate(HandleArrowUpDown);
            PlayerControllerCommands.ArrowDownEvent += new PlayerControllerCommands.EventDelegate(HandleArrowDownDown);
        }

        public void Update()
        {
            //if(controllerAction != null)
            //{
            //    controllerAction.Update();
            //}
        }

        public void HandleLeftClick(RailorLibrary.Input.Bindable.BindableState state)
        {

        }

        public void HandleRightClick(RailorLibrary.Input.Bindable.BindableState state)
        {

        }

        public void HandleArrowLeftDown(RailorLibrary.Input.Bindable.BindableState state)
        {
            if(state == RailorLibrary.Input.Bindable.BindableState.Held)
            {
                GameScreen.AddPosition(-1, 0);
            }
        }
        public void HandleArrowRightDown(RailorLibrary.Input.Bindable.BindableState state)
        {
            if(state == RailorLibrary.Input.Bindable.BindableState.Held)
            {
                GameScreen.AddPosition(1, 0);
            }
        }
        public void HandleArrowUpDown(RailorLibrary.Input.Bindable.BindableState state)
        {
            if(state == RailorLibrary.Input.Bindable.BindableState.Held)
            {
                GameScreen.AddPosition(0, -1);
            }
        }
        public void HandleArrowDownDown(RailorLibrary.Input.Bindable.BindableState state)
        {
            if(state == RailorLibrary.Input.Bindable.BindableState.Held)
            {
                GameScreen.AddPosition(0, 1);
            }
        }

        //public void ClearControllerAction()
        //{
        //    controllerAction = null;
        //}

        //public void SetControllerAction(ControllerAction action)
        //{
        //    controllerAction = action;
        //}

        //public ControllerAction GetControllerAction()
        //{
        //    return controllerAction;
        //}

        //public Boolean IsDoingAction(Type actionType)
        //{
        //    return controllerAction != null && controllerAction.GetType() == actionType;
        //}
    }
}
