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
	public struct Location : IEquatable<Location>{
		public int x,y;

		public Location(int x, int y) {
			this.x = x;
			this.y = y;
		}
		public Location Right() {
			return new Location(this.x+1,this.y);
		}
		public Location Left() {
			return new Location(this.x-1,this.y);
		}
		public Location Up() {
			return new Location(this.x, this.y-1);
		}
		public Location Down() {
			return new Location(this.x, this.y+1);
		}
		public override bool Equals(object o) {
			if (o is Location) {
				return this.Equals ((Location)o);
			}
			return false;
		}
		public bool Equals(Location l) {
			return (this.x==l.x)&&(this.y==l.y);
		}
		public static bool operator ==(Location l1, Location l2) {
			return l1.Equals(l2);
		}
		public static bool operator !=(Location l1, Location l2) {
			return !l1.Equals(l2);
		}
		public override int GetHashCode() {
			return this.x ^ this.y;
		}
		public override string ToString() {
			return "("+this.x+","+this.y+")";
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
		/// <exception cref="ArgumentException">If object already registered.</exception>
		/// <exception cref="ArgumentException">If World o.World() is not the same as this world instance.</exception>
		/// <exception cref="ArgumentException">If the key string clashes with an already registered one of this world instance.</exception>
		/// <exception cref="ArgumentOutOfRange">If the location o.Location() is outside of the world this
		/// throws ArgumentOutOfRange exception.</exception>
		public void RegisterLocalObject(string key, LocalObject o) {
			if (!o.IsRegistered ()) {
				if (o.World () == this) {
					if (this.HasLocation (o.Location ())) {
						if (!_game_objects.ContainsKey (key)) {
							_game_objects.Add (key, o);
							o.Register (key);
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
			} else {
				throw new ArgumentException ("LocalObject already registered in call to RegisterLocalObject");
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

		public bool HasLocalObject (string key)
		{
			return _game_objects.ContainsKey(key);
		}

		public object GetLocalObject (string key)
		{
			if (!HasLocalObject (key)) {
				throw new ArgumentException ("World does not have a LocalObject with the given key!", "string key");
			}
			return _game_objects [key];
		}

		public System.Collections.ICollection GetLocalObjects ()
		{
			return this._game_objects.Values;
		}

		public List<LocalObject> GetLocalObjectsByLocation (Location location)
		{
			List<LocalObject> objects = new List<LocalObject>();
			foreach(LocalObject obj in this._game_objects.Values) {
				if (obj.Location() == location) {
					objects.Add(obj);
				}
			}
			return objects;
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

