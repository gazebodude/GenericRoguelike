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
	public class World
	{
		private string _name;
		public static string[] WorldNames = new string[] {"Endaril","Westmar","Vanaheim","Hof"};

		public World ()
		{
			Random rand = new Random ();
			this._name = WorldNames [rand.Next (WorldNames.Length)];
		}

		public string Name() {
			return _name;
		}

	}
}

