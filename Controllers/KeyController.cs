//
//  KeyController.cs
//
//  Author:
//       Michael J. Brown <michael.brown6@my.jcu.edu.au>
//
//  Copyright (c) 2013 Michael J. Brown
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using GenericRoguelike.Models;
using GenericRoguelike.Views;

namespace GenericRoguelike.Controllers
{
	public class HandlerResult
	{
		private System.Text.StringBuilder sb;
		public HandlerResult() {
			this.sb = new System.Text.StringBuilder ();
		}
		public void AddResult(string s) {
			this.sb.Append(s);
		}
		public override string ToString ()
		{
			return this.sb.ToString();
		}
	}
	public class KeyController
	{
		public const ConsoleKey KEY_MOVE_UP = ConsoleKey.W;
		public const ConsoleKey KEY_MOVE_DOWN = ConsoleKey.S;
		public const ConsoleKey KEY_MOVE_LEFT = ConsoleKey.A;
		public const ConsoleKey KEY_MOVE_RIGHT = ConsoleKey.D;
		public const ConsoleKey KEY_QUIT = ConsoleKey.Escape;

		public delegate void Handler(string message, HandlerResult result);

		private Dictionary<ConsoleKey, Handler> _handlers;

		public KeyController ()
		{
			this._handlers = new Dictionary<ConsoleKey, Handler>();
		}

		public void RegisterCallback(ConsoleKey k, Handler h)
		{
			if (!this._handlers.ContainsKey (k)) {
				this._handlers.Add (k, h);
			} else {
				this._handlers [k] += h;
			}
		}

		public string RunOnce()
		{
			ConsoleKeyInfo cki = Console.ReadKey (false);
			HandlerResult result = new HandlerResult ();

			if (this._handlers.ContainsKey(cki.Key)) {
				this._handlers [cki.Key] (KeyToString(cki.Key),result);
			}
			return result.ToString ();
		}

		private string KeyToString(ConsoleKey k) {
			switch (k) {
			case KeyController.KEY_MOVE_UP:
				return "MOVE_UP";
			case KeyController.KEY_MOVE_DOWN:
				return "MOVE_DOWN";
			case KeyController.KEY_MOVE_RIGHT:
				return "MOVE_RIGHT";
			case KeyController.KEY_MOVE_LEFT:
				return "MOVE_LEFT";
			case KeyController.KEY_QUIT:
				return "QUIT";
			}
			return "";
		}
	}
}

