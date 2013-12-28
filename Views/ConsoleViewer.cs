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
using GenericRoguelike;
using GenericRoguelike.Models;

namespace GenericRoguelike.Views
{
	public class ConsoleViewer
	{
		private int _width;
		private int _height;
		private World _world;

		public ConsoleViewer (World w)
		{
			this._width = Console.WindowWidth;
			this._height = Console.WindowHeight;
			this._world = w;
		}

		private void DrawChar(char c, int x, int y)
		{
			try {
				Console.SetCursorPosition(x,y);
				Console.Write(c);
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
			Console.Clear ();
			if (!this._world.HasLocation (new Location (x, y))) {
				throw new ArgumentOutOfRangeException ("(x,y)", "Cannot draw a location outside of the world!");
			}

			// here is where we would draw the world, but we still need some logic in the World class:
			for (int i = 0; i < this._width; i++) {
				for (int j = 0; j < this._height; j++) {
					this.DrawChar ('.', i, j);
				}
			}
			int _i = this._width / 2 - 10;
			int _j = this._height / 2;
			string msg = "Press enter to quit...";
			for (int k=0; k < msg.Length; k++) {
				this.DrawChar(msg[k],_i++,_j);
			}

		}
	}
}
