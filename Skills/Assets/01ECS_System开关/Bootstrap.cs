using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary>
/// 启动程序类
/// </summary>
public class Bootstrap
{
    /// <summary>
    /// This attribute allows us to not use MonoBehaviours to instantiate entities
    /// 该函数在场景加载之后执行，不使用MonoBehaviours来初始化实体
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeAfterScene()
    {
        var em = World.Active.GetOrCreateManager<EntityManager>();
        GameObject prefab = Resources.Load<GameObject>("BallArchetype");


        NativeArray<Entity> balls = new NativeArray<Entity>(1000, Allocator.TempJob);

        
         
    }
}
