using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Ray = Unity.Physics.Ray;
using RaycastHit = Unity.Physics.RaycastHit;

namespace Physics_02RayCastMono
{
    public class Test01 : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(0, 50 * Time.deltaTime, 0);

            float3 RayFrom = transform.position;
            float3 RayTo = transform.forward.normalized * 10;
            Raycast(RayFrom, RayTo);
        }

        public void Raycast(float3 origin, float3 direction)
        {
            var physicsWorldSystem = World.Active.GetExistingSystem<BuildPhysicsWorld>();
            var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
            RaycastInput input = new RaycastInput()
            {
                Ray = new Ray()
                {
                    Origin = origin,
                    Direction = direction
                },
                Filter = new CollisionFilter()
                {
                    CategoryBits = ~0u, // all 1s, so all layers, collide with everything 
                    MaskBits = ~0u,
                    GroupIndex = 0
                }
            };

            RaycastHit hit = new RaycastHit();

            if (collisionWorld.CastRay(input, out hit))
            {
                // see hit.Position 
                // see hit.SurfaceNormal
                //   Entity e = physicsWorldSystem.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                Debug.Log(hit.Position);
            }
        }
    }
}

