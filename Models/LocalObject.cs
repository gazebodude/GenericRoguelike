//
//  LocalObject.cs
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
	/// Local object represents an object with a location within the
	/// game world. Has references to the world and its location within it.
	/// </summary>
	public class LocalObject {
		protected World world;
		protected Location loc;
		protected bool is_registered;
		protected string key;
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRoguelike.Models.LocalObject"/> class.
		/// </summary>
		/// <param name="w">The containing World instance.</param>
		/// <param name="l">The Location of the LocalObject within the world.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the location l is outside of the world w throws ArgumentOutOfRange exception.</exception>
		public LocalObject(World w, Location l) {
			this.world = w;
			this.loc = l;
			this.is_registered = false;
			this.key = null;
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
		public void Move(Location new_loc) {
			this.loc = new_loc;
		}

		public bool IsRegistered ()
		{
			return is_registered;
		}
		public void Register(string k)
		{
			if (!this.is_registered) {
				this.is_registered = true;
				this.key = k;
			}
		}
		public string Key ()
		{
			return key;
		}
		/// <summary>
		/// Character representation of this instance.
		/// </summary>
		public virtual char Char()
		{
			return ' ';
		}
	}

	public class Destructible:LocalObject
	{
		protected int health;
		protected bool alive;

		public Destructible(World w, Location l) : base(w,l) {
			this.health = 100;
			this.alive = true;
		}
		public Destructible(World w, Location l, int maxHealth, bool isAlive) : base(w,l) {
			this.health = maxHealth;
			this.alive = isAlive;
		}

		public bool isAlive() {
			return this.alive;
		}
		public void takeDamage(int damage) {
			this.health -= damage;
			if (this.health <= 0) {
				this.alive = false;
			}
		}
	}

	public abstract class Agent:Destructible {
		public abstract void TakeAction(string message);
		public Agent(World w, Location l):base(w,l) {

		}
	}

	public class Player:Agent {
		private static bool player_exists;

		public Player(World w, Location start):base(w,start) {
			if (player_exists) {
				throw new InvalidOperationException ("Cannot create more than one Player object!");
			}
			player_exists = true;
		}
		public override void TakeAction(string message) {
			if (message.StartsWith("MOVE_")) {
				Location loc = this.Location();
				switch (message) {
				case "MOVE_UP":
					loc = this.Location ().Up ();
					break;
				case "MOVE_DOWN":
					loc = this.Location ().Down ();
					break;
				case "MOVE_RIGHT":
					loc = this.Location ().Right ();
					break;
				case "MOVE_LEFT":
					loc = this.Location ().Left ();
					break;
				}
				if(this.World().HasLocation(loc)) {
					this.Move(loc);
				}
			}
		}
		public override string ToString ()
		{
			return String.Format ("Health: {0}", this.health);
		}
		public override char Char() {
			return 'p';
		}
	}
}
