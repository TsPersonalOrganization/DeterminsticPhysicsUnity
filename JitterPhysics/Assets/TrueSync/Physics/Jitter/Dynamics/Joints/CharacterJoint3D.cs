﻿/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

namespace TrueSync.Physics3D
{

	/// <summary>
	/// Connects to bodies with a hinge joint.
	/// </summary>
	public class CharacterJoint3D : Joint3D
	{
		FixedAngle fixedAngle;
		PointOnLine pointOnLine;

		private PointOnPoint[] worldPointConstraint;

		private IBody3D firstBody;
		private IBody3D secondBody;

		private TSVector hingeA;

		/// <summary>
		/// Initializes a new instance of the HingeJoint class.
		/// </summary>
		/// <param name="world">The world class where the constraints get added to.</param>
		/// <param name="body1">The first body connected to the second one.</param>
		/// <param name="body2">The second body connected to the first one.</param>
		/// <param name="position">The position in world space where both bodies get connected.</param>
		/// <param name="hingeAxis">The axis if the hinge.</param>
		/// 
		public static void Create (IWorld world, IBody3D body1, IBody3D body2, TSVector position, TSVector hingeAxis)
		{
			new CharacterJoint3D (world, body1, body2, position, hingeAxis);
		}

		public CharacterJoint3D (IWorld world, IBody3D body1, IBody3D body2, TSVector position, TSVector hingeAxis) : base ((World)world)
		{
			RigidBody rigid1 = (RigidBody)body1;
			RigidBody rigid2 = (RigidBody)body2;

			fixedAngle = new FixedAngle (rigid1, rigid2);
			pointOnLine = new PointOnLine (rigid1, rigid2, rigid1.position, rigid2.position);

			firstBody = body1;
			secondBody = body2;
			hingeA = hingeAxis;
			worldPointConstraint = new PointOnPoint[2];
			hingeAxis *= FP.Half;
			TSVector anchor = position;
			TSVector.Add (ref anchor, ref hingeAxis, out anchor);
			TSVector anchor2 = position;
			TSVector.Subtract (ref anchor2, ref hingeAxis, out anchor2);
			worldPointConstraint[0] = new PointOnPoint ((RigidBody)body1, (RigidBody)body2, anchor);
			worldPointConstraint[1] = new PointOnPoint ((RigidBody)body2, (RigidBody)body1, anchor2);
			StateTracker.AddTracking (worldPointConstraint[0]);
			StateTracker.AddTracking (worldPointConstraint[1]);
			//UnityEngine.CharacterJoint;
			Activate ();
		}

		public PointOnPoint PointOnPointConstraint1 { get { return worldPointConstraint[0]; } }

		public PointOnPoint PointOnPointConstraint2 { get { return worldPointConstraint[1]; } }

		public FP AppliedImpulse { get { return worldPointConstraint[0].AppliedImpulse + worldPointConstraint[1].AppliedImpulse; } }

		public FP getHingeAngle ()
		{
			return TSVector.Dot (hingeA, (firstBody.TSOrientation.eulerAngles - secondBody.TSOrientation.eulerAngles));
		}

		public FP getAngularVel ()
		{
			return TSVector.Dot (hingeA, (firstBody.TSAngularVelocity - secondBody.TSAngularVelocity));
		}


		/// <summary>
		/// Adds the internal constraints of this joint to the world class.
		/// </summary>
		public override void Activate ()
		{

			//World.AddConstraint (fixedAngle);
			//World.AddConstraint (pointOnLine);

			World.AddConstraint (worldPointConstraint[0]);
			World.AddConstraint (worldPointConstraint[1]);
		}

		/// <summary>
		/// Removes the internal constraints of this joint from the world class.
		/// </summary>
		public override void Deactivate ()
		{

			//World.RemoveConstraint (fixedAngle);
			//World.RemoveConstraint (pointOnLine);

			World.RemoveConstraint (worldPointConstraint[0]);
			World.RemoveConstraint (worldPointConstraint[1]);
		}
	}
}
