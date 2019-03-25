using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;

namespace _01Spawn
{
    /// <summary>
    /// 开一个或多个子线程
    /// </summary>
    public class BallMoveSystem : JobComponentSystem
    {
       // [BurstCompile]
        struct MoveJob : IJobProcessComponentData<Translation, BallMoveSpeed>
        {
            public float time;
            public void Execute( ref Translation pos, [ReadOnly] ref BallMoveSpeed speed)
            {
                pos.Value.y = math.sin(time * speed.Value) * 5;
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new MoveJob()
            {
                time = Time.timeSinceLevelLoad
            };
            return job.Schedule(this, inputDeps); //schedules parallel for jobs

            //*****execute in a single job************
            //return job.ScheduleSingle(this, inputDeps);
        }
    }
}