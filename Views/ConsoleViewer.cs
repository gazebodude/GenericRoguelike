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

		public ConsoleViewer (World w)
		{
			this._width = Console.WindowWidth;
			this._height = Console.WindowHeight-1;
			this._world = w;
			this._buffer = new char[this._width, this._height];
		}

		private void DrawChar(char c, int x, int y)
		{
			try {
//				Console.SetCursorPosition(x,y);
//				Console.Write(c);
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
		public void Update(int x, int y) {
			if (!this._world.HasLocation (new Location (x, y))) {
				throw new ArgumentOutOfRangeException ("(x,y)", "Cannot draw a location outside of the world!");
			}

			// here is where we would draw the world, but we still need some logic in the World class:
			for (int i = 0; i < this._width; i++) {
				for (int j = 0; j < this._height; j++) {
					Location loc = new Location (x + i, y + j);
					if (!this._world.HasLocation (loc)) {
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
			int _i = this._width / 2 - 10;
			int _j = this._height - 1;
			string msg = "Press ESC to quit...";
			for (int k=0; k < msg.Length; k++) {
				this.DrawChar(msg[k],_i++,_j);
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			for (int i = 0; i < this._height; i++) {
				for (int j = 0; j < this._width; j++) {
					sb.Append (this._buffer [j, i]);
				}
			}
			Console.Clear ();
			Console.Write (sb.ToString());
		}
	}
}

