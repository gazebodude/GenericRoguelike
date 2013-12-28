//
//  ConsoleViewer.cs
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
using GenericRoguelike;
using GenericRoguelike.Models;

namespace GenericRoguelike.Views
{
	public class ConsoleViewer
	{
		private int _width;
		private int _height;
		private World _world;
		private char[,] _buffer;
		private int _map_width;
		private int _map_height;

		public ConsoleViewer (World w)
		{
			this._width = Console.WindowWidth;
			this._height = Console.WindowHeight-1;
			this._world = w;
			this._buffer = new char[this._width, this._height];
			this._map_width = this._width * 2 / 3;
			this._map_height = this._height - 5;
		}

		private void DrawChar(char c, int x, int y)
		{
			try {
				this._buffer[x,y]=c;
			} catch (ArgumentOutOfRangeException e) {
				// go quietly...
			}
		}

		/// <summary>
		/// Update view, with the top left corner of the map at the specified World x and y coordinates.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void Update(int x, int y, int turn_counter) {
			this.ClearBuffer ();
			if (!this._world.HasLocation (new Location (x, y))) {
				throw new ArgumentOutOfRangeException ("(x,y)", "Cannot draw a location outside of the world!");
			}

			// draw map:
			for (int i = 0; i < this._map_width; i++) {
				for (int j = 0; j < this._map_height; j++) {
					Location loc = new Location (x + i, y + j);
					if (!this._world.HasLocation (loc)) {
						this.DrawChar('x',i,j);
						continue;
					}
					List<LocalObject> objs = this._world.GetLocalObjectsByLocation (loc);
					if (objs.Count == 0) {
						this.DrawChar ('.', i, j);
					} else {
						foreach (LocalObject o in objs) {
							this.DrawChar (o.Char (), i, j);
						}
					}
				}
			}
			// draw messages:
			int _i = this._width / 2 - 10;
			int _j = this._height - 1;
			string msg = "Press ESC to quit...";
			for (int k=0; k < msg.Length; k++) {
				this.DrawChar(msg[k],_i++,_j);
			}
			_i = 2;
			_j = this._height - 3;
			try {
				var player = this._world.GetLocalObject ("player");
				msg = "Turn: " + turn_counter + " Stats: " + player + " Location: " + player.Location().ToString();
				for (int k=0; k < msg.Length; k++) {
					this.DrawChar(msg[k],_i++,_j);
				}
			} catch (ArgumentException e) {
				// pass
			}

			// compose and draw buffer:
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			for (int i = 0; i < this._height; i++) {
				for (int j = 0; j < this._width; j++) {
					sb.Append (this._buffer [j, i]);
				}
			}
			Console.Clear ();
			Console.Write (sb.ToString());
		}

		public void DeathScreen (Player player, int turns)
		{
			Console.Clear ();
			Console.WriteLine ("You died on turn {0}!", turns);
			Console.WriteLine ("Stats: "+player);
		}

		private void ClearBuffer ()
		{
			for (int i = 0; i < this._height; i++) {
				for (int j = 0; j < this._width; j++) {
					this._buffer [j,i] = ' ';
				}
			}
		}
	}
}

