//
//  WorldTestClass.cs
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
using NUnit.Framework;

namespace GenericRoguelike
{
	using Models;

	[TestFixture ()]
	public class WorldTestClass
	{

		[Test ()]
		public void TestWorldConstructor ()
		{
			World w = new World (20, 30);

			Assert.Contains (w.Name (), World.WorldNames);
			Assert.AreEqual (20, w.Width());
			Assert.AreEqual (30, w.Height());

			World w2 = new World ();

			Assert.Contains (w2.Name (), World.WorldNames);
			Assert.AreEqual (40, w2.Width());
			Assert.AreEqual (40, w2.Height());
		}

		[Test ()]
		public void TestLocation ()
		{
			Location loc = new Location (5, 7);

			Assert.AreEqual (5, loc.x);
			Assert.AreEqual (7, loc.y);
			Assert.AreEqual ("(5,7)",loc.ToString());

			Location loc2 = new Location ();

			Assert.AreEqual (0, loc2.x);
			Assert.AreEqual (0, loc2.y);
			Assert.AreEqual ("(0,0)", loc2.ToString());
		}

		[Test ()]
		public void TestWorldHasLocation ()
		{
			World w = new World (20, 30);
			Location middle_location = new Location (5, 7);
			Location origin_location = new Location ();
			Location bottom_right_corner_location = new Location ((int)w.Width() - 1, (int)w.Height () - 1);
			Location outside_bottom_right_corner_location = new Location (20,30);
			Location outside_top_left_corner_location = new Location (-1, -1);

			Assert.IsTrue (w.HasLocation (middle_location));
			Assert.IsTrue (w.HasLocation (origin_location));
			Assert.IsTrue (w.HasLocation (bottom_right_corner_location));
			Assert.IsFalse (w.HasLocation (outside_bottom_right_corner_location));
			Assert.IsFalse (w.HasLocation (outside_top_left_corner_location));
		}

		[Test ()]
		public void TestWorldRegisterLocalObject()
		{
			// TODO: code out stub; test currently fails
			Assert.Fail ("TestWorldRegisterLocalObject not implemented yet!");

			// What I would like to work:
			// World w = new World();
			// LocalObject obj = new LocalObject(w, new Location());
			// obj.IsRegistered() <-- returns False
			// obj.Key() <-- returns null
			// w.RegisterLocalObject("new thingy", obj); <-- raises exception if loc out of bounds, key collision or obj already registered
			// obj.IsRegistered() <-- returns true
			// obj.Key() <-- returns "new thingy"
			// w.HasLocalObject("new thingy") <-- returns true
			// w.GetLocalObject("new thingy") <-- returns obj
			// w.GetLocalObjectsByLocation(new Location()) <-- returns list with obj in it
			// w.GetLocalObjects() <-- returns list with obj in it
			// ^^^ Maybe implement a collection interface?
		}

	}
}

