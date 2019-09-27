using UnityEngine;
using System;

namespace Hakumuchu.DayDreamController.Components
{
    public class TouchPad
    {
        public struct State
        {
            public bool click;
            public Vector2 position;
            public State(bool click, Vector2 position)
            { this.click = click; this.position = position; }
        }
        public void 
    }

    public struct Buttons
    {
        public struct State
        {
            public bool app, home, up, down;
            public State(bool app, bool home, bool up, bool down)
            { this.app = app; this.home = home; this.up = up; this.down = down; }
        }

    }
}
