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
using GenericRoguelike.Controllers;

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
		public int attack_strength { get; set; }
		public abstract void TakeAction(string message, HandlerResult result);

		public Agent(World w, Location l):base(w,l) {

		}
	}

	public class Player:Agent {
		private static Random rand;
		private static bool player_exists;

		static Player() {
			rand = new Random();
		}

		public Player(World w, Location start):base(w,start) {
			if (player_exists) {
				throw new InvalidOperationException ("Cannot create more than one Player object!");
			}
			player_exists = true;
			this.attack_strength = 10;
		}

		public override void TakeAction(string message,HandlerResult result) {
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
					var objs_at_dest = this.World ().GetLocalObjectsByLocation (loc);
					if (objs_at_dest.Count > 0) {
						LocalObject target = objs_at_dest [0];
						if ((target is Destructible) && ((Destructible)target).isAlive()) {
							this.Attack ((Destructible)target);
							result.AddResult ("Attacking " + target.ToString ());
						}
					} else {
						this.Move(loc);
					}
				}
			}
		}

		public void Attack (Destructible target) {
			int damage = (int)((double)this.attack_strength * rand.NextDouble ());
			target.takeDamage (damage);
			if ((target is Agent) && target.isAlive()) {
				int counter_damage = (int)((double)((Agent)target).attack_strength * rand.NextDouble ());
				this.takeDamage (counter_damage);
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
	public class Mouse:Agent
	{
		private static Random rand;
		static Mouse() {
			rand = new Random();
		}
		public Mouse(World w, Location l):base(w,l) {
			this.health = 10;
			this.attack_strength = 2;
		}
		public override void TakeAction (string message, HandlerResult result)
		{
			if (!this.isAlive ()) {
				return;
			}
			Location loc = this.Location();
			switch (rand.Next (10)) {
			case 0:
				loc = this.Location ().Up ();
				break;
			case 1:
				loc = this.Location ().Down ();
				break;
			case 2:
				loc = this.Location ().Right ();
				break;
			case 3:
				loc = this.Location ().Left ();
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				break; // no move
			}
			if(this.World().HasLocation(loc)) {
				this.Move(loc);
			}
		}
		public override char Char ()
		{
			return (this.isAlive())?'m':'*';
		}
		public override string ToString ()
		{
			return string.Format ("[Mouse] Health: {0} Location: {1}", this.health, this.Location());
		}
	}
	public class BigMouse:Mouse
	{
		public BigMouse(World w, Location l):base(w,l) {
			this.health = 80;
			this.attack_strength = 15;
		}
		public override char Char ()
		{
			return (this.isAlive())?'M':'*';
		}
		public override string ToString ()
		{
			return string.Format ("[BigMouse] Health: {0} Location: {1}", this.health, this.Location());
		}
	}
}
