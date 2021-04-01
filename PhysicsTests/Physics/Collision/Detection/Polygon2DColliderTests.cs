﻿using System.Collections.Generic;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using Logging;
using NUnit.Framework;
using Physics;
using Physics.Bodies;
using Physics.Collision.Detection;

namespace PhysicsTests.Collision.Detection
{
    [TestFixture]
    public class Polygon2DColliderTests
    {
        [Test]
        [TestCaseSource("TestCases")]
        public void CollisionIsDetectedProperly(RigidBody body, RigidBody other, bool collisionExpected)
        {
            var collider = new PolygonCollider(new SATInterpenetrationChecker(new OneDimensionIntersectionChecker()), 
                new CollisionPointsFinder(new ConsoleLogger()));
            var collisionDetected = false;
            collider.ObjectsColliding += (x, y) =>
            {
                collisionDetected = true;
            };

            collider.Collide(body, other);

            Assert.That(collisionDetected, Is.EqualTo(collisionExpected));
        }

        // Generated code. Look in IntersectionTestCases dir in collision detection tests dir
        private IEnumerable<TestCaseData> TestCases()
        {
            var i = 0;
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((7, 12)).WithShape(new Polygon(new[] { new Vector2(-80, -79), new Vector2(89, -66), new Vector2(67, 0), new Vector2(-86, 49) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-34, -17)).WithShape(new Polygon(new[] { new Vector2(-38, -66), new Vector2(91, -34), new Vector2(37, 25), new Vector2(-84, 4) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((46, 44)).WithShape(new Polygon(new[] { new Vector2(-9, -32), new Vector2(91, -99), new Vector2(89, 4), new Vector2(-64, 11) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-33, -54)).WithShape(new Polygon(new[] { new Vector2(-77, -94), new Vector2(62, -90), new Vector2(93, 5), new Vector2(-51, 1) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((52, 53)).WithShape(new Polygon(new[] { new Vector2(-80, -30), new Vector2(90, -70), new Vector2(60, 84), new Vector2(-15, 93) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-46, -37)).WithShape(new Polygon(new[] { new Vector2(-21, -12), new Vector2(43, -22), new Vector2(62, 21), new Vector2(-18, 47) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((25, 12)).WithShape(new Polygon(new[] { new Vector2(-36, -75), new Vector2(71, -44), new Vector2(3, 0), new Vector2(-40, 32) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-23, -3)).WithShape(new Polygon(new[] { new Vector2(-74, -82), new Vector2(26, -45), new Vector2(74, 95), new Vector2(-63, 79) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((35, 27)).WithShape(new Polygon(new[] { new Vector2(-88, -71), new Vector2(89, -65), new Vector2(51, 39), new Vector2(-27, 71) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-39, -35)).WithShape(new Polygon(new[] { new Vector2(-87, -3), new Vector2(14, -87), new Vector2(98, 93), new Vector2(-55, 4) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((51, 3)).WithShape(new Polygon(new[] { new Vector2(-43, -83), new Vector2(19, -38), new Vector2(85, 98), new Vector2(-48, 67) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-55, -38)).WithShape(new Polygon(new[] { new Vector2(-57, -38), new Vector2(5, -66), new Vector2(50, 71), new Vector2(-74, 65) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((31, 30)).WithShape(new Polygon(new[] { new Vector2(-72, -68), new Vector2(44, -71), new Vector2(68, 39), new Vector2(-46, 81) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-47, -31)).WithShape(new Polygon(new[] { new Vector2(-40, -58), new Vector2(21, -96), new Vector2(54, 78), new Vector2(-62, 5) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((23, 28)).WithShape(new Polygon(new[] { new Vector2(-91, -60), new Vector2(14, -49), new Vector2(97, 47), new Vector2(-36, 26) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-42, -27)).WithShape(new Polygon(new[] { new Vector2(-18, -90), new Vector2(18, -52), new Vector2(52, 1), new Vector2(-87, 74) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((32, 57)).WithShape(new Polygon(new[] { new Vector2(-51, -66), new Vector2(27, -98), new Vector2(17, 8), new Vector2(-11, 71) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-43, -12)).WithShape(new Polygon(new[] { new Vector2(-9, -69), new Vector2(13, -35), new Vector2(26, 40), new Vector2(-73, 12) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((20, 9)).WithShape(new Polygon(new[] { new Vector2(-2, -20), new Vector2(62, -49), new Vector2(40, 68), new Vector2(-44, 53) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-19, -53)).WithShape(new Polygon(new[] { new Vector2(-91, -88), new Vector2(66, -3), new Vector2(80, 33), new Vector2(-1, 24) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((2, 46)).WithShape(new Polygon(new[] { new Vector2(-36, -64), new Vector2(17, -49), new Vector2(77, 83), new Vector2(-26, 11) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-35, -29)).WithShape(new Polygon(new[] { new Vector2(-28, -75), new Vector2(19, -43), new Vector2(74, 49), new Vector2(-43, 31) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((1, 8)).WithShape(new Polygon(new[] { new Vector2(-64, -58), new Vector2(80, -21), new Vector2(2, 9), new Vector2(-55, 92) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-43, -34)).WithShape(new Polygon(new[] { new Vector2(-39, -49), new Vector2(78, -39), new Vector2(63, 84), new Vector2(-84, 37) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((50, 58)).WithShape(new Polygon(new[] { new Vector2(-41, -32), new Vector2(24, -16), new Vector2(68, 23), new Vector2(-28, 30) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-27, -40)).WithShape(new Polygon(new[] { new Vector2(-89, -69), new Vector2(19, -56), new Vector2(56, 83), new Vector2(-24, 78) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((20, 38)).WithShape(new Polygon(new[] { new Vector2(-27, -34), new Vector2(72, -78), new Vector2(96, 80), new Vector2(-79, 90) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-31, -47)).WithShape(new Polygon(new[] { new Vector2(-65, -6), new Vector2(28, -90), new Vector2(4, 1), new Vector2(-6, 32) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((35, 27)).WithShape(new Polygon(new[] { new Vector2(-80, -25), new Vector2(2, -69), new Vector2(29, 21), new Vector2(-82, 80) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-11, -52)).WithShape(new Polygon(new[] { new Vector2(-59, -5), new Vector2(52, -5), new Vector2(66, 5), new Vector2(-29, 7) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((18, 1)).WithShape(new Polygon(new[] { new Vector2(-43, -38), new Vector2(64, -83), new Vector2(41, 29), new Vector2(-52, 54) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-29, -45)).WithShape(new Polygon(new[] { new Vector2(-76, -92), new Vector2(45, -21), new Vector2(44, 66), new Vector2(-87, 1) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((36, 11)).WithShape(new Polygon(new[] { new Vector2(-97, -99), new Vector2(31, -60), new Vector2(53, 22), new Vector2(-23, 99) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-16, 0)).WithShape(new Polygon(new[] { new Vector2(-72, -98), new Vector2(72, -87), new Vector2(44, 64), new Vector2(-36, 42) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((3, 2)).WithShape(new Polygon(new[] { new Vector2(-69, -28), new Vector2(3, -66), new Vector2(60, 31), new Vector2(-61, 99) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-18, -11)).WithShape(new Polygon(new[] { new Vector2(-18, -27), new Vector2(76, -56), new Vector2(96, 44), new Vector2(-91, 44) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((57, 38)).WithShape(new Polygon(new[] { new Vector2(-5, -53), new Vector2(82, -77), new Vector2(19, 57), new Vector2(-77, 79) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-59, -12)).WithShape(new Polygon(new[] { new Vector2(-30, -44), new Vector2(41, -12), new Vector2(75, 48), new Vector2(-5, 68) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((27, 15)).WithShape(new Polygon(new[] { new Vector2(-61, -24), new Vector2(80, -44), new Vector2(22, 15), new Vector2(-90, 99) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-28, -1)).WithShape(new Polygon(new[] { new Vector2(-73, -91), new Vector2(35, -55), new Vector2(88, 9), new Vector2(-95, 16) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((43, 22)).WithShape(new Polygon(new[] { new Vector2(-54, -81), new Vector2(69, -73), new Vector2(86, 63), new Vector2(-81, 47) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-33, -2)).WithShape(new Polygon(new[] { new Vector2(-39, -22), new Vector2(47, -42), new Vector2(5, 9), new Vector2(-57, 81) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((32, 4)).WithShape(new Polygon(new[] { new Vector2(-66, -77), new Vector2(6, -38), new Vector2(8, 85), new Vector2(-24, 65) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-34, -35)).WithShape(new Polygon(new[] { new Vector2(-53, -88), new Vector2(22, -70), new Vector2(17, 96), new Vector2(-66, 79) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((44, 18)).WithShape(new Polygon(new[] { new Vector2(-34, -43), new Vector2(60, -50), new Vector2(44, 93), new Vector2(-50, 79) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-38, 0)).WithShape(new Polygon(new[] { new Vector2(-52, -14), new Vector2(44, -8), new Vector2(70, 43), new Vector2(-36, 77) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((10, 33)).WithShape(new Polygon(new[] { new Vector2(-39, -16), new Vector2(94, -50), new Vector2(0, 76), new Vector2(-24, 9) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-56, -9)).WithShape(new Polygon(new[] { new Vector2(-42, -78), new Vector2(26, -65), new Vector2(79, 96), new Vector2(-69, 23) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((29, 5)).WithShape(new Polygon(new[] { new Vector2(-35, -44), new Vector2(13, -38), new Vector2(44, 13), new Vector2(0, 73) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-30, -40)).WithShape(new Polygon(new[] { new Vector2(-59, -89), new Vector2(4, -24), new Vector2(42, 57), new Vector2(-19, 18) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((1, 33)).WithShape(new Polygon(new[] { new Vector2(-68, -41), new Vector2(36, -73), new Vector2(88, 78), new Vector2(-29, 74) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-3, -52)).WithShape(new Polygon(new[] { new Vector2(-73, -13), new Vector2(59, -18), new Vector2(35, 49), new Vector2(-65, 94) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((4, 23)).WithShape(new Polygon(new[] { new Vector2(-75, -50), new Vector2(49, -52), new Vector2(73, 1), new Vector2(-23, 21) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-30, -39)).WithShape(new Polygon(new[] { new Vector2(-76, -51), new Vector2(74, -7), new Vector2(6, 27), new Vector2(-46, 93) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((55, 50)).WithShape(new Polygon(new[] { new Vector2(-84, -26), new Vector2(0, -46), new Vector2(29, 86), new Vector2(-88, 92) })).Build(), RigidBody.Create().WithMass(0).WithLocation((0, -51)).WithShape(new Polygon(new[] { new Vector2(-91, -25), new Vector2(15, -92), new Vector2(4, 63), new Vector2(-91, 17) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((30, 1)).WithShape(new Polygon(new[] { new Vector2(-55, -7), new Vector2(98, -22), new Vector2(38, 51), new Vector2(-11, 8) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-13, -52)).WithShape(new Polygon(new[] { new Vector2(-3, -71), new Vector2(86, -63), new Vector2(5, 39), new Vector2(-2, 48) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((17, 46)).WithShape(new Polygon(new[] { new Vector2(-12, -31), new Vector2(29, -50), new Vector2(35, 82), new Vector2(-3, 42) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-4, -46)).WithShape(new Polygon(new[] { new Vector2(-36, -28), new Vector2(1, -23), new Vector2(39, 64), new Vector2(-10, 76) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((46, 28)).WithShape(new Polygon(new[] { new Vector2(-74, -80), new Vector2(88, -20), new Vector2(39, 52), new Vector2(-87, 64) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-52, -33)).WithShape(new Polygon(new[] { new Vector2(-60, -42), new Vector2(66, -88), new Vector2(61, 12), new Vector2(-73, 24) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((25, 5)).WithShape(new Polygon(new[] { new Vector2(-48, -69), new Vector2(71, -74), new Vector2(70, 62), new Vector2(-23, 40) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-6, -49)).WithShape(new Polygon(new[] { new Vector2(-28, -26), new Vector2(20, -73), new Vector2(94, 44), new Vector2(-13, 32) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((5, 8)).WithShape(new Polygon(new[] { new Vector2(-94, -38), new Vector2(82, -93), new Vector2(61, 12), new Vector2(-62, 26) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-29, -45)).WithShape(new Polygon(new[] { new Vector2(-90, -86), new Vector2(98, -81), new Vector2(89, 51), new Vector2(-30, 5) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((52, 46)).WithShape(new Polygon(new[] { new Vector2(-28, -46), new Vector2(36, -34), new Vector2(22, 93), new Vector2(-93, 16) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-8, -34)).WithShape(new Polygon(new[] { new Vector2(-99, -41), new Vector2(82, -68), new Vector2(36, 4), new Vector2(-61, 9) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((1, 28)).WithShape(new Polygon(new[] { new Vector2(0, -97), new Vector2(50, -42), new Vector2(27, 38), new Vector2(-83, 43) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-42, -40)).WithShape(new Polygon(new[] { new Vector2(-14, -7), new Vector2(23, -28), new Vector2(59, 35), new Vector2(-54, 47) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((58, 56)).WithShape(new Polygon(new[] { new Vector2(-19, -46), new Vector2(26, -49), new Vector2(4, 30), new Vector2(-45, 85) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-4, -50)).WithShape(new Polygon(new[] { new Vector2(-90, -20), new Vector2(18, -30), new Vector2(0, 92), new Vector2(-76, 31) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((42, 0)).WithShape(new Polygon(new[] { new Vector2(-9, -50), new Vector2(2, -91), new Vector2(18, 53), new Vector2(-8, 0) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-22, -58)).WithShape(new Polygon(new[] { new Vector2(-49, -14), new Vector2(6, -39), new Vector2(6, 45), new Vector2(-38, 54) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((26, 21)).WithShape(new Polygon(new[] { new Vector2(-93, -97), new Vector2(52, -93), new Vector2(89, 92), new Vector2(-27, 66) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-18, -30)).WithShape(new Polygon(new[] { new Vector2(-59, -23), new Vector2(50, -8), new Vector2(40, 87), new Vector2(-58, 68) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((58, 5)).WithShape(new Polygon(new[] { new Vector2(-22, -44), new Vector2(36, -75), new Vector2(14, 79), new Vector2(-23, 55) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-1, -15)).WithShape(new Polygon(new[] { new Vector2(-54, -21), new Vector2(60, -95), new Vector2(56, 10), new Vector2(-89, 11) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((36, 46)).WithShape(new Polygon(new[] { new Vector2(-25, -41), new Vector2(41, -25), new Vector2(63, 98), new Vector2(-98, 18) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-47, -6)).WithShape(new Polygon(new[] { new Vector2(-86, -69), new Vector2(10, -35), new Vector2(70, 87), new Vector2(-74, 49) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((1, 6)).WithShape(new Polygon(new[] { new Vector2(-68, -72), new Vector2(12, -37), new Vector2(62, 55), new Vector2(-51, 30) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-7, -9)).WithShape(new Polygon(new[] { new Vector2(-98, -46), new Vector2(70, -35), new Vector2(33, 4), new Vector2(-24, 81) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((17, 2)).WithShape(new Polygon(new[] { new Vector2(-80, -4), new Vector2(68, -85), new Vector2(11, 25), new Vector2(-25, 69) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-37, -4)).WithShape(new Polygon(new[] { new Vector2(-36, -40), new Vector2(50, -21), new Vector2(29, 91), new Vector2(-56, 53) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((2, 16)).WithShape(new Polygon(new[] { new Vector2(-46, -68), new Vector2(20, -39), new Vector2(99, 74), new Vector2(-1, 65) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-15, -22)).WithShape(new Polygon(new[] { new Vector2(-62, -93), new Vector2(47, -49), new Vector2(64, 63), new Vector2(-29, 64) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((50, 17)).WithShape(new Polygon(new[] { new Vector2(-29, -82), new Vector2(94, -57), new Vector2(95, 48), new Vector2(-6, 41) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-51, -22)).WithShape(new Polygon(new[] { new Vector2(-48, -71), new Vector2(51, -97), new Vector2(48, 4), new Vector2(-69, 12) })).Build(), false).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((44, 37)).WithShape(new Polygon(new[] { new Vector2(-23, -59), new Vector2(6, -42), new Vector2(93, 0), new Vector2(-57, 65) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-26, -30)).WithShape(new Polygon(new[] { new Vector2(-62, -92), new Vector2(50, -2), new Vector2(79, 25), new Vector2(-78, 63) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((45, 30)).WithShape(new Polygon(new[] { new Vector2(-68, -60), new Vector2(17, -91), new Vector2(41, 49), new Vector2(-1, 60) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-38, -57)).WithShape(new Polygon(new[] { new Vector2(-94, -17), new Vector2(79, -74), new Vector2(41, 88), new Vector2(-13, 70) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((25, 9)).WithShape(new Polygon(new[] { new Vector2(-91, -40), new Vector2(79, -55), new Vector2(21, 49), new Vector2(-18, 22) })).Build(), RigidBody.Create().WithMass(0).WithLocation((0, -26)).WithShape(new Polygon(new[] { new Vector2(-25, -5), new Vector2(38, -21), new Vector2(85, 34), new Vector2(-67, 63) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((48, 37)).WithShape(new Polygon(new[] { new Vector2(-99, -96), new Vector2(71, -45), new Vector2(12, 75), new Vector2(-60, 90) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-42, -54)).WithShape(new Polygon(new[] { new Vector2(-12, -8), new Vector2(88, -22), new Vector2(97, 48), new Vector2(-58, 28) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((41, 20)).WithShape(new Polygon(new[] { new Vector2(-30, -78), new Vector2(34, -89), new Vector2(78, 58), new Vector2(-88, 6) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-25, -27)).WithShape(new Polygon(new[] { new Vector2(-20, -49), new Vector2(27, -60), new Vector2(50, 21), new Vector2(-95, 94) })).Build(), true).SetName((i++).ToString());
            yield return new TestCaseData(RigidBody.Create().WithMass(0).WithLocation((37, 49)).WithShape(new Polygon(new[] { new Vector2(-89, -28), new Vector2(10, -17), new Vector2(17, 15), new Vector2(-23, 36) })).Build(), RigidBody.Create().WithMass(0).WithLocation((-39, -32)).WithShape(new Polygon(new[] { new Vector2(-76, -39), new Vector2(83, -80), new Vector2(45, 9), new Vector2(-12, 20) })).Build(), false).SetName((i++).ToString());
        }
    }
}