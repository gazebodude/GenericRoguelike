//
//  World.cs
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

namespace GenericRoguelike.Models
{
	public struct Location {
		public int x,y;
		public Location(int x, int y) {
			this.x = x;
			this.y = y;
		}
		public override string ToString() {
			return "("+this.x+","+this.y+")";
		}
	}
	public class LocalObject {
		private World world;
		private Location loc;

		public LocalObject(World w, Location l) {
			/// If the location l is outside of the world w throws ArgumentOutOfRange exception
			this.world = w;
			this.loc = l;
			if (!w.HasLocation (l))
				throw new ArgumentOutOfRangeException ("Location l",
					"Location passed to LocalObject(World w, Location l) which is outside of specified world!");
		}

		public World World() {
			return world;
		}
		public Location Location() {
			return loc;
		}
	}

	public class World
	{
		private string _name;
		private uint _width, _height;
		public static string[] WorldNames = new string[] {"Endaril","Westmar","Vanaheim","Hof"};

		public World (uint w, uint h)
		{
			Random rand = new Random ();
			this._name = WorldNames [rand.Next (WorldNames.Length)];
			this._width = w;
			this._height = h;
		}

		public string Name() {
			return _name;
		}
		public uint Width() { return _width; }
		public uint Height() { return _height; }
		public bool HasLocation(Location loc) {
			if (loc.x >= 0 && loc.x < _width && loc.y >= 0 && loc.y < _height) {
				return true;
			} else {
				return false;
			}
		}

	}
}

