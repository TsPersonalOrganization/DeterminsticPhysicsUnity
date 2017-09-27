using UnityEngine;
using System.Collections;
using TrueSync;

namespace Dynamics {

	public static class JitterPhysicsTools
    {
		///// <summary>
		///// Gets the center of mass of an array of Rigidbodies.
		///// </summary>
		//public static Vector3 GetCenterOfMass(Rigidbody[] rigidbodies) {
		//	Vector3 CoM = Vector3.zero;
		//	float c = 0f;
			
		//	for (int i = 0; i < rigidbodies.Length; i++) {
		//		if (rigidbodies[i].gameObject.activeInHierarchy) {
		//			CoM += rigidbodies[i].worldCenterOfMass * rigidbodies[i].mass;
					
		//			c += rigidbodies[i].mass;
		//		}
		//	}
			
		//	return CoM / c;
		//}

		///// <summary>
		///// Gets the velocity of the center of mass of an array of Rigidbodies.
		///// </summary>
		//public static Vector3 GetCenterOfMassVelocity(Rigidbody[] rigidbodies) {
		//	Vector3 CoM = Vector3.zero;
		//	float c = 0f;
			
		//	for (int i = 0; i < rigidbodies.Length; i++) {
		//		if (rigidbodies[i].gameObject.activeInHierarchy) {
		//			CoM += rigidbodies[i].velocity * rigidbodies[i].mass;
					
		//			c += rigidbodies[i].mass;
		//		}
		//	}
			
		//	return CoM / c;
		//}

		///// <summary>
		///// Divides an angular acceleration by an inertia tensor.
		///// </summary>
		//public static void DivByInertia(ref Vector3 v, Quaternion rotation, Vector3 inertiaTensor) {
		//	v = rotation * Div(Quaternion.Inverse(rotation) * v, inertiaTensor);
		//}
		
		///// <summary>
		///// Scales an angular acceleration by an inertia tensor
		///// </summary>
		//public static void ScaleByInertia(ref Vector3 v, Quaternion rotation, Vector3 inertiaTensor) {
		//	v = rotation * Vector3.Scale(Quaternion.Inverse(rotation) * v, inertiaTensor);
		//}
		
		///// <summary>
		///// Returns the angular acceleration from one vector to another.
		///// </summary>
		//public static Vector3 GetFromToAcceleration(Vector3 fromV, Vector3 toV) {
		//	Quaternion fromTo = Quaternion.FromToRotation(fromV, toV);
		//	float requiredAccelerationDeg = 0f;
		//	Vector3 axis = Vector3.zero;
		//	fromTo.ToAngleAxis(out requiredAccelerationDeg, out axis);
			
		//	Vector3 requiredAcceleration = requiredAccelerationDeg * axis * Mathf.Deg2Rad;
			
		//	return requiredAcceleration / Time.fixedDeltaTime;
		//}
		
		///// <summary>
		///// Returns the angular acceleration from the current rigidbody rotation to Quaternion.identity. 
		///// Does not guarantee full accuracy with rotations around multiple axes).
		///// </summary>
		//public static Vector3 GetAngularAcceleration(Quaternion fromR, Quaternion toR) {
		//	Vector3 axis = Vector3.Cross(fromR * Vector3.forward, toR * Vector3.forward);
		//	Vector3 axis2 = Vector3.Cross(fromR * Vector3.up, toR * Vector3.up);
		//	float angle = Quaternion.Angle(fromR, toR);
		//	Vector3 acc = Vector3.Normalize(axis + axis2) * angle * Mathf.Deg2Rad;
			
		//	return acc / Time.fixedDeltaTime;
		//}
				
		///// <summary>
		///// Returns the linear acceleration from one point to another.
		///// </summary>
		//public static Vector3 GetLinearAcceleration(Vector3 fromPoint, Vector3 toPoint) {
		//	return (toPoint - fromPoint) / Time.fixedDeltaTime;
		//}


  //      /// <summary>
  //      /// The rotation expressed by the joint's axis and secondary axis
  //      /// </summary>
  //      public static Quaternion ToJointSpace(ConfigurableJoint joint) {
		//	Vector3 forward = Vector3.Cross (joint.axis, joint.secondaryAxis);
		//	Vector3 up = Vector3.Cross (forward, joint.axis);
		//	return Quaternion.LookRotation (forward, up);
		//}

		///// <summary>
		///// Calculates the inertia tensor for a cuboid.
		///// </summary>
		//public static Vector3 CalculateInertiaTensorCuboid(Vector3 size, float mass) {
		//	float x2 = Mathf.Pow(size.x, 2);
		//	float y2 = Mathf.Pow(size.y, 2);
		//	float z2 = Mathf.Pow(size.z, 2);
			
		//	float mlp = 1f/12f * mass;
			
		//	return new Vector3(
		//		mlp * (y2 + z2),
		//		mlp * (x2 + z2),
		//		mlp * (x2 + y2)); 
		//}

		///// <summary>
		///// Divide all the values in v by the respective values in v2.
		///// </summary>
		//public static Vector3 Div(Vector3 v, Vector3 v2) {
		//	return new Vector3(v.x / v2.x, v.y / v2.y, v.z / v2.z);
		//}


        /// <summary>
        /// align to vector
        /// </summary>
        /// <param name="r"></param>
        /// <param name="alignmentVector"></param>
        /// <param name="targetVector"></param>
        /// <param name="stability"></param>
        /// <param name="speed"></param>
        public static void AlignToVector(TSRigidBody r, Vector3 alignmentVector, Vector3 targetVector, float stability, FP speed, bool debug = false)
        {
            if (r == null)
            {
                return;
            }

            // 围绕着某个轴进行了一个角度的旋转 angleAxis(x,y). x = 角度 y = 轴
            // x 角度值为 当前转向速度 * Stability 是一个 radian 值， * 57.29 转为角度值， 除以 speed 后 才是真正的角度值
            // part.angularVelocity 是当前旋转速度， 把旋转速度变为轴？
            Quaternion angleAxis = Quaternion.AngleAxis( r.angularVelocity.magnitude.AsFloat() * 57.29578f * stability / speed.AsFloat(), r.angularVelocity.ToVector());

            // Vector3.Cross 是叉乘， alignmentVector 是当前我们把part 某个轴要对准target Vector 的向量， angleAxis 四元素乘以AlignmentVector 是把alignmentVector 方向调整为四元素方向
            // targetVector 是我们想要对准的角度
            // 叉乘出来的 a 就是 两个向量的九十度角的向量用来作为我们的旋转轴
            Vector3 a = Vector3.Cross(angleAxis * alignmentVector, targetVector * 10f);
            
            if (!float.IsNaN(a.x) && !float.IsNaN(a.y) && !float.IsNaN(a.z))
            {

                r.AddTorque(a.ToTSVector() * speed * speed);

                if (debug)
                {
                    Debug.DrawRay(r.position.ToVector(), alignmentVector * 0.3f, Color.red, 0f, false);
                    Debug.DrawRay(r.position.ToVector(), targetVector * 0.3f, Color.green, 0f, false);
                }

            }
        }

        ///// <summary>
        ///// Adds a force to a Rigidbody that gets it from one place to another within a single simulation step using any force mode.
        ///// </summary>
        //public static void AddFromToForce(Rigidbody r, Vector3 toV, float forcePercent, ForceMode forceMode)
        //{
        //    Vector3 requiredVelocity = GetLinearAcceleration(r.position, toV); // required velocity.

        //    // 去除或者加上已经有的velocity.
        //    requiredVelocity -= r.velocity;
            
        //    requiredVelocity *= forcePercent;

        //    switch (forceMode)
        //    {
        //        case ForceMode.Acceleration:
        //            // 因为acceleration Mode, 忽略重量， 所以我们只需要 在force 这里传入Acceleration.
        //            r.AddForce(requiredVelocity / Time.fixedDeltaTime, forceMode);                    
        //            break;

        //        case ForceMode.Force:
        //            Vector3 force = requiredVelocity / Time.fixedDeltaTime;
        //            // 不忽略重量， 所以我们要在这里加入* r.mass
        //            force *= r.mass;                    
        //            r.AddForce(force, forceMode);
        //            break;

        //        case ForceMode.Impulse:
        //            Vector3 impulse = requiredVelocity;
        //            // 不忽略重量， 所以我们要在这里加入* r.mass
        //            impulse *= r.mass;
        //            r.AddForce(impulse, forceMode);
        //            break;

        //        case ForceMode.VelocityChange:
        //            // 因为acceleration Mode, 忽略重量， 所以我们只需要 在force 这里传入Velocity
        //            r.AddForce(requiredVelocity, forceMode);
        //            break;
        //    }

            
        //}

        ///// <summary>
        ///// Adds torque to the Ridigbody that accelerates it from it's current rotation to another using any force mode.
        ///// </summary>
        //public static void AddFromToTorque(Rigidbody r, Quaternion toR, ForceMode forceMode)
        //{
        //    // 获取一针时我们需要的转力加速 可以达到我们 toR 的转力.
        //    Vector3 requiredAngularVelocity = GetAngularAcceleration(r.rotation, toR);

        //    // Compensate for angular velocity
        //    requiredAngularVelocity -= r.angularVelocity; 

        //    switch (forceMode)
        //    {
        //        case ForceMode.Acceleration:
        //            // 因为我们不需要angular mass, 所以直接加速angular acceleration
        //            r.AddTorque(requiredAngularVelocity / Time.fixedDeltaTime, forceMode);
        //            break;

        //        case ForceMode.Force:
        //            // 我们需要 angular mass(inertia)所以通过 puppet master里一个计算inertia 的方法.
        //            Vector3 force = requiredAngularVelocity / Time.fixedDeltaTime;
        //            ScaleByInertia(ref force, r.rotation, r.inertiaTensor);
        //            r.AddTorque(force, forceMode);
        //            break;

        //        case ForceMode.Impulse:
        //            // 我们需要 angular mass(inertia)所以通过 puppet master里一个计算inertia 的方法. 
        //            Vector3 impulse = requiredAngularVelocity;
        //            ScaleByInertia(ref impulse, r.rotation, r.inertiaTensor);
        //            r.AddTorque(impulse, forceMode);
        //            break;

        //        case ForceMode.VelocityChange:
        //            r.AddTorque(requiredAngularVelocity, forceMode);
        //            break;
        //    }
        //}
    }
}
