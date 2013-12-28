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
using GenericRoguelike.Views;
using GenericRoguelike.Controllers;

namespace GenericRoguelike
{

	class MainClass
	{
		private static bool quitting;

		public static void Main (string[] args)
		{
			World game_world = new World (60, 20);
			ConsoleViewer view = new ConsoleViewer (game_world);

			Console.WriteLine ("The name of the world is " + game_world.Name());
			Console.WriteLine ("It has a width of {0} and height of {1}", game_world.Width (), game_world.Height ());
			Console.WriteLine ("Hello "+game_world.Name()+"!");

			Player player = new Player (game_world, new Location (0, 0));
			game_world.RegisterLocalObject("player",player);

			KeyController controller = new KeyController ();
			KeyController.Handler player_action_handler = new KeyController.Handler (player.TakeAction);
			KeyController.Handler quit_handler = new KeyController.Handler (Quit);
			controller.RegisterCallback (KeyController.KEY_MOVE_UP, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_DOWN, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_LEFT, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_RIGHT, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_QUIT, quit_handler);
			Console.WriteLine ("Press enter to continue...");
			Console.ReadLine ();

			quitting = false;
			while (!quitting) {
				view.Update (System.Math.Max(0,player.Location().x-10),System.Math.Max(0,player.Location().y-10));
				controller.RunOnce ();
			}

		}

		public static void Quit (string message)
		{
			quitting = true;
		}
	}
}
