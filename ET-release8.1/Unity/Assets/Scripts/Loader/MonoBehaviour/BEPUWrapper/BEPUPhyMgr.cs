using System.Collections;
using System.Collections.Generic;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using FixMath.NET;
using Fix64 = TrueSync.FP;
using UnityEngine;
using Space = BEPUphysics.Space;
using Vector3 = BEPUutilities.Vector3;

public class BEPUPhyMgr
{
    private static BEPUPhyMgr Instance = null;

    public static BEPUPhyMgr GetInstance()
    {
        if (Instance == null)
        {
            Instance = new BEPUPhyMgr();
        }
        return Instance;
    }
    
    private Space Space;
    public BEPUphyIDController idController;
    
    
    // Start is called before the first frame update
    public void InitBEPUPhy()
    {
        InitSpace();
        
        // 配置 id 管理
        idController = new BEPUphyIDController(Space);
        idController.InitEntity();
        
        Box ground = new Box(Vector3.Zero, 30, 1, 30);
        idController.AddEntityById(ground, 1);
        
        // 配置碰撞事件
        ground.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
        
        // 添加 BOX
        idController.AddEntityById(new Box(new Vector3(0, 0.9, 0), 1, 1, 1, 1), 2);
        idController.AddEntityById(new Box(new Vector3(0, 20, 0), 1, 1, 1, 1), 3);
        idController.AddEntityById(new Box(new Vector3(0, 40, 0), 1, 1, 1, 1), 4);
    }

    void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair){
        var otherEntityInformation = other as EntityCollidable;

        if (otherEntityInformation != null)
        {
            int a = 10;
            Debug.Log("发生了碰撞事件");
        }
        
    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitSpace()
    {
        Space = new Space();
        Space.ForceUpdater.Gravity = new Vector3(0, (Fix64)(-10), 0);
        Space.TimeStepSettings.TimeStepDuration = (TrueSync.FP)1 / (TrueSync.FP)60;
    }
    
    public void Step()
    {
        if (Space != null)
        {
            Space.Update();
        }
    }

    /// <summary>
    /// 创建一个 BOX
    /// </summary>
    /// <param name="pX"></param>
    /// <param name="pY"></param>
    /// <param name="pZ"></param>
    public Entity CreateOneBox3D(Fix64 pX, Fix64 pY, Fix64 pZ, long pId)
    {
        Box tBox = new Box(new Vector3(pX, pY, pZ), 1, 1, 1, 1);
        idController.AddEntityById(tBox, pId);
        return tBox;
    }
}

