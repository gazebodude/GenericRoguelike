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
using System.Collections.Generic;
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

			Assert.IsFalse (loc == loc2);
			Assert.IsTrue (loc2 == new Location ());

			Assert.AreEqual (12, loc.DistanceTo (loc2));
			Assert.AreEqual (12, loc2.DistanceTo (loc));
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
		public void TestLocalObjectConstructor()
		{
			World w = new World();
			LocalObject obj = new LocalObject(w, new Location());

			Assert.IsFalse (obj.IsRegistered ());
			Assert.IsNull (obj.Key ());
		}

		[Test ()]
		public void TestWorldRegisterLocalObject()
		{
			World w = new World();
			LocalObject obj = new LocalObject(w, new Location());

			w.RegisterLocalObject("new thingy", obj);
			Assert.IsTrue (obj.IsRegistered ());
			Assert.AreEqual ("new thingy", obj.Key ());
			Assert.IsTrue (w.HasLocalObject ("new thingy"));
			Assert.AreSame (obj, w.GetLocalObject ("new thingy"));
			Assert.IsFalse (w.HasLocalObject ("should not exist"));
			try {
				w.GetLocalObject("should not exist");
				Assert.Fail("GetLocalObject returns object with non-existent key");
			} catch (ArgumentException e) {
				Assert.IsTrue (e.Message.Contains ("World does not have a LocalObject with the given key!"));
			}
			List<LocalObject> objects_at_origin = w.GetLocalObjectsByLocation (new Location ()); //<-- returns list with obj in it
			List<LocalObject> objects_at_11 = w.GetLocalObjectsByLocation (new Location (1, 1));
			Assert.AreEqual (1, objects_at_origin.Count);
			Assert.Contains (obj, objects_at_origin);
			Assert.AreEqual (0, objects_at_11.Count);
			Assert.Contains (obj, w.GetLocalObjects ());
			// ^^^ Maybe implement a collection interface?
		}

		[Test ()]
		public void TestWorldRegisterLocalObjectExceptions()
		{
			World w = new World ();
			World w2 = new World ();
			LocalObject wrong_world_obj = new LocalObject (w2, new Location ());
			LocalObject out_of_bounds_obj = new LocalObject (w, new Location ());
			out_of_bounds_obj.Move (new Location (-1, -1));
			LocalObject double_registration_obj = new LocalObject (w, new Location ());
			LocalObject key_collision_obj = new LocalObject (w, new Location ());

			try {
				w.RegisterLocalObject("wrong world object", wrong_world_obj);
				// If it gets here then test fails
				Assert.Fail("Wrong world object registers");
			} catch (ArgumentException e) {
				Assert.IsTrue (e.Message.Contains("Cannot register an object to a different world!"));
			}
			try {
				w.RegisterLocalObject("out of bounds object", out_of_bounds_obj);
				Assert.Fail("Out of bounds object registers");
			} catch (ArgumentOutOfRangeException e) {
				Assert.IsTrue (e.Message.Contains("Location of object passed to RegisterLocalObject is outside of specified world!"));
			}
			w.RegisterLocalObject ("double registration object", double_registration_obj);
			try {
				w.RegisterLocalObject("try double registering", double_registration_obj);
				Assert.Fail("Object registers twice");
			} catch (ArgumentException e) {
				Assert.IsTrue (e.Message.Contains("LocalObject already registered in call to RegisterLocalObject"));
			}
			try {
				w.RegisterLocalObject("double registration object", key_collision_obj);
				Assert.Fail("Registered two objects with the same key");
			} catch (ArgumentException e) {
				Assert.IsTrue (e.Message.Contains ("Cannot register more than one object with the same key!"));
			}
		}

		[Test ()]
		public void TestWorldDeregisterLocalObject()
		{
			World w = new World();
			LocalObject obj = new LocalObject(w, new Location());

			w.RegisterLocalObject("new thingy", obj);
			Assert.IsTrue (obj.IsRegistered ());
			Assert.IsTrue (w.HasLocalObject ("new thingy"));
			w.DeregisterLocalObject ("new thingy");
			Assert.IsFalse (obj.IsRegistered ());
			Assert.IsFalse (w.HasLocalObject ("new thingy"));
			w.RegisterLocalObject ("second time going", obj);
			Assert.IsTrue (obj.IsRegistered ());
			Assert.IsTrue (w.HasLocalObject ("second time going"));
		}

	}
}

