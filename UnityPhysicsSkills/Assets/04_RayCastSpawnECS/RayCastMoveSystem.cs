﻿using UnityEngine;
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

namespace Physics_04RayCastSpawnECS
{
    [UpdateAfter(typeof(SpawnSystem))]
    public class RayCastMoveSystem : JobComponentSystem
    {
        float range;
        [BurstCompile]
        struct RayCastJob : IJobForEach<Translation, Rotation, Move>
        {
            [ReadOnly] public PhysicsWorld World;
            public float time;
            public Random rand;
            public float range;
            public void Execute([ReadOnly] ref Translation pos, [ReadOnly] ref Rotation rot, ref Move move)
            {
                //射线查询
                float3 dirNormal = Math.normalize(Math.mul(rot.Value, new float3(0, 0, 1)));
                RaycastInput input = new RaycastInput()
                {
                    Ray = new Ray()
                    {
                        Origin = pos.Value + dirNormal,
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
                    move.isHit = 1;
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
                    #region 加速度
                    if (move.speed < move.maxSpeed)
                        move.speed += move.acceleratioin * time;

                    pos.Value += Math.normalize(move.targetPos - pos.Value) * move.speed * time;
                    #endregion
                }
                else //碰到障碍，右转180度/秒
                {
                    move.speed = 0; //减速
                    move.targetPos = math.rotate(quaternion.AxisAngle(new float3(0, 1, 0), (float)math.PI * time), (move.targetPos - pos.Value)) + pos.Value;
                    rot.Value = quaternion.LookRotation(move.targetPos - pos.Value, new float3(0, 1, 0));

                }
            }
            /// <summary>
            /// 随机获取下一个目标
            /// </summary>
            /// <returns></returns>
            float3 GetRandomTarget()
            {
                float3 tmp = new float3(rand.NextFloat(-range, range), 1, rand.NextFloat(-range, range));
                // Debug.Log(tmp);
                return tmp;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ref bool isSpawn = ref World.Active.GetExistingSystem<SpawnSystem>().isSpawn;
            if (isSpawn)
            {
                isSpawn = false;
                return inputDeps;
            }

            ref PhysicsWorld world = ref World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
            range = Spawn.Instance.Range;
            var job = new RayCastJob()
            {
                World = world,
                time = Time.deltaTime,
                rand = new Random((uint)Time.time * 1000 + 1),
                range = range
            };
            return job.Schedule(this, inputDeps); //schedules parallel for jobs          
        }
    }
}

