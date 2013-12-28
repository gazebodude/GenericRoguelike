//
//  Generic Roguelike: A (In)complete Ripoff!
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
			int turns = 0;
			Random rand = new Random ();
			World game_world = new World (60, 20);
			ConsoleViewer view = new ConsoleViewer (game_world);
			KeyController controller = new KeyController ();

			Console.WriteLine ("The name of the world is " + game_world.Name());
			Console.WriteLine ("It has a width of {0} and height of {1}", game_world.Width (), game_world.Height ());
			Console.WriteLine ("Hello "+game_world.Name()+"!");

			Player player = new Player (game_world, new Location (0, 0));
			game_world.RegisterLocalObject("player",player);
			KeyController.Handler player_action_handler = new KeyController.Handler (player.TakeAction);
			KeyController.Handler quit_handler = new KeyController.Handler (Quit);
			controller.RegisterCallback (KeyController.KEY_MOVE_UP, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_DOWN, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_LEFT, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_MOVE_RIGHT, player_action_handler);
			controller.RegisterCallback (KeyController.KEY_QUIT, quit_handler);
			// create 5 mice & 2 big mice:
			for (int i = 0; i < 5; i++) {
				Mouse m = new Mouse (game_world, new Location (rand.Next (60), rand.Next (20)));
				game_world.RegisterLocalObject ("mouse" + i, m);
				KeyController.Handler mouse_action = new KeyController.Handler (m.TakeAction);
				controller.RegisterCallback (KeyController.KEY_MOVE_UP, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_DOWN, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_LEFT, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_RIGHT, mouse_action);
			}
			for (int i = 0; i < 2; i++) {
				BigMouse m = new BigMouse (game_world, new Location (rand.Next (60), rand.Next (20)));
				game_world.RegisterLocalObject ("bigmouse" + i, m);
				KeyController.Handler mouse_action = new KeyController.Handler (m.TakeAction);
				controller.RegisterCallback (KeyController.KEY_MOVE_UP, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_DOWN, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_LEFT, mouse_action);
				controller.RegisterCallback (KeyController.KEY_MOVE_RIGHT, mouse_action);
			}

			Console.WriteLine ("Press enter to continue...");
			Console.ReadLine ();

			quitting = false;
			while (!quitting) {
				++turns;
				view.Update (System.Math.Max(0,player.Location().x-10),System.Math.Max(0,player.Location().y-10),turns);
				controller.RunOnce ();
				if (!player.isAlive ()) {
					view.DeathScreen (player,turns);
					Console.WriteLine ("Press enter to continue...");
					Console.ReadLine ();
					break;
				}
			}

		}

		public static void Quit (string message)
		{
			quitting = true;
		}
	}
}
