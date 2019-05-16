using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Math = Unity.Mathematics.math;
using Ray = Unity.Physics.Ray;
using RaycastHit = Unity.Physics.RaycastHit;
using Random = Unity.Mathematics.Random;

namespace Physics_03RayCastECS
{
    public class RayCastMoveSystem : JobComponentSystem
    {
        [BurstCompile]
        struct RayCastJob : IJobForEach<Translation, Rotation, Move>
        {
            [ReadOnly] public PhysicsWorld World;
            public float time;
            public Random rand;
            public void Execute([ReadOnly] ref Translation pos, [ReadOnly] ref Rotation rot, ref Move move)
            {
                float3 dirNormal = Math.normalize(Math.mul(rot.Value, new float3(0, 0, 1)));
                RaycastInput input = new RaycastInput()
                {
                    Ray = new Ray()
                    {
                        Origin = pos.Value + dirNormal * 0.51f,
                        Direction = dirNormal * 1.1f
                    },
                    Filter = new CollisionFilter()
                    {
                        CategoryBits = ~0u, // all 1s, so all layers, collide with everything 
                        MaskBits = ~0u,
                        GroupIndex = 0
                    }
                };
                RaycastHit hit = new RaycastHit();

                if (World.CastRay(input, out hit))
                {
                    // see hit.Position 
                    move.isHit = 1;
                    // Debug.Log(hit.Position);
                }
                else
                    move.isHit = 0;


                //检测碰撞，重设目标，移动。
                if (move.isHit == 0)
                {
                    if (Math.distancesq(move.targetPos, pos.Value) < 0.01)
                    {
                        move.targetPos = GetRandomTarget();
                        rot.Value = quaternion.LookRotation(move.targetPos - pos.Value, new float3(0, 1, 0));
                    }
                    pos.Value += Math.normalize(move.targetPos - pos.Value) * time;
                }
                else
                {
                    move.targetPos = GetRandomTarget();
                    rot.Value = quaternion.LookRotation(move.targetPos - pos.Value, new float3(0, 1, 0));
                }
            }
            float3 GetRandomTarget()
            {
                float3 tmp = new float3(rand.NextFloat(-10, 10), 1, rand.NextFloat(-10, 10));
                // Debug.Log(tmp);
                return tmp;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ref PhysicsWorld world = ref World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
            var job = new RayCastJob()
            {
                World = world,
                time = Time.deltaTime,
                rand = new Random((uint)Time.time * 1000 + 1)
            };
            return job.Schedule(this, inputDeps); //schedules parallel for jobs
        }
    }
}

