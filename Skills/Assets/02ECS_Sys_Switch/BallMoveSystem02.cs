using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using System;
using Unity.Jobs;



public class BallMoveSystem02 : JobComponentSystem
{
    public static BallMoveSystem02 Instance;
    struct MoveJob : IJobProcessComponentData<Position, BallMoveSpeed02>
    {
        public float time;
        public void Execute(ref Position pos, ref BallMoveSpeed02 speed)
        {
            pos.Value.y = math.sin(time * speed.Value) * 5;

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MoveJob() { time = Time.timeSinceLevelLoad };
        var handle = job.Schedule(this);
        return handle;
    }

    protected override void OnStartRunning()
    {
        if (Instance == null)
            Instance = this;
        base.OnStartRunning();
    }
}
