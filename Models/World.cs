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
using System.Collections.Generic;

namespace GenericRoguelike.Models
{
	/// <summary>
	/// Location represents a location within the game world.
	/// Just a basic wrapper for (x,y) values.
	/// </summary>
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

	/// <summary>
	/// Local object represents an object with a location within the
	/// game world. Has references to the world and its location within it.
	/// </summary>
	public class LocalObject {
		private World world;
		private Location loc;
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRoguelike.Models.LocalObject"/> class.
		/// </summary>
		/// <param name="w">The containing World instance.</param>
		/// <param name="l">The Location of the LocalObject within the world.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the location l is outside of the world w throws ArgumentOutOfRange exception.</exception>
		public LocalObject(World w, Location l) {
			this.world = w;
			this.loc = l;
			if (!w.HasLocation (l))
				throw new ArgumentOutOfRangeException ("Location l",
					"Location passed to LocalObject(World w, Location l) which is outside of specified world!");
		}

		/// <summary>
		/// Return the containing World of this instance.
		/// </summary>
		public World World() {
			return world;
		}
		/// <summary>
		/// Location of this instance.
		/// </summary>
		public Location Location() {
			return loc;
		}
	}

	/// <summary>
	/// An instance of World represents the game world and contains references to
	/// all localisable game objects within it.
	/// </summary>
	public class World
	{
		private string _name;
		private uint _width, _height;
		private Dictionary<string,LocalObject> _game_objects = new Dictionary<string, LocalObject>();

		public static string[] WorldNames = new string[] {"Endaril","Westmar","Vanaheim","Hof"};

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRoguelike.Models.World"/> class.
		/// Defaults to a size of 40x40.
		/// </summary>
		public World()
		{
			/// Default size is 40x40
			_NewRandomName ();
			this._width = 40;
			this._height = 40;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRoguelike.Models.World"/> class.
		/// </summary>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public World (uint w, uint h)
		{
			_NewRandomName ();
			this._width = w;
			this._height = h;
		}
		/// <summary>
		/// Name of this instance.
		/// </summary>
		public string Name() {
			return _name;
		}
		/// <summary>
		/// Width of this instance.
		/// </summary>
		public uint Width() { return _width; }
		/// <summary>
		/// Height of this instance.
		/// </summary>
		public uint Height() { return _height; }
		/// <summary>
		/// Determines whether this instance has a given location.
		/// </summary>
		/// <returns><c>true</c> if this instance has location the specified loc; otherwise, <c>false</c>.</returns>
		/// <param name="loc">Location.</param>
		public bool HasLocation(Location loc) {
			if (loc.x >= 0 && loc.x < _width && loc.y >= 0 && loc.y < _height) {
				return true;
			} else {
				return false;
			}
		}

		// TODO: Decide if this is the right way to implement object registration!
		/// <summary>
		/// Registers a local object in the World instance.
		/// </summary>
		/// <param name="key">The reference key of the LocalObject.</param>
		/// <param name="o">The LocalObject to be registered in the World instance.</param>
		/// <exception cref="ArgumentException">If World o.World() is not the same as this world instance.</exception>
		/// <exception cref="ArgumentException">If the key string clashes with an already registered one of this world instance.</exception>
		/// <exception cref="ArgumentOutOfRange">If the location o.Location() is outside of the world this
		/// throws ArgumentOutOfRange exception.</exception>
		public void RegisterLocalObject(string key, LocalObject o) {
			if (o.World () == this) {
				if (this.HasLocation (o.Location ())) {
					if (!_game_objects.ContainsKey (key)) {
						_game_objects.Add (key, o);
					} else {
						throw new ArgumentException ("Cannot register more than one object with the same key!", "string key");
					}
				} else {
					throw new ArgumentOutOfRangeException ("LocalObject o",
						"Location of object passed to RegisterLocalObject is outside of specified world!");

				}
			} else {
				throw new ArgumentException ("Cannot register an object to a different world!", "LocalObject o");
			}
		}

		/// <summary>
		/// Return a dictionary of game objects.
		/// </summary>
		/// <returns>The objects as a dictionary of registered (string key, LocalObject value) pairs.</returns>
		public Dictionary<string,LocalObject> GameObjects()
		{
			return _game_objects;
		}
		
		/// <summary>
		/// Generate a new random world name chosen randomly from the list Models.WorldNames
		/// </summary>
		private void _NewRandomName ()
		{
			Random rand = new Random ();
			this._name = WorldNames [rand.Next (WorldNames.Length)];
		}
	}
}

