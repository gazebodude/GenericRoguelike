/*
 * Generic Roguelike
 * A silly game of monsters and mayhem and... *ahem*, coding mistakes.
 * Copyright 2013 by Michael J. Brown
 * 
 * This file is part of Generic Roguelike.
 * Generic Roguelike is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Generic Roguelike is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
 * along with Generic Roguelike.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using GenericRoguelike.Models;
//using Views;
//using Controllers;

namespace GenericRoguelike
{

	class MainClass
	{
		public static void Main (string[] args)
		{
			World game_world = new World (20, 30);
			Console.WriteLine ("The name of the world is " + game_world.Name());
			Console.WriteLine ("It has a width of {0} and height of {1}", game_world.Width (), game_world.Height ());
			Console.WriteLine ("Hello "+game_world.Name()+"!");

			try {
				LocalObject player = new LocalObject (game_world, new Location (0, 0));
				Console.WriteLine ("Player location: "+ player.Location());
				if (game_world.HasLocation(player.Location())) {
					Console.WriteLine ("Player is within the game world.");
				} else {
					Console.WriteLine ("Player is outside the game world!");
				}
			} catch (ArgumentOutOfRangeException e) {
				Console.WriteLine (e);
			}
		}
	}
}
