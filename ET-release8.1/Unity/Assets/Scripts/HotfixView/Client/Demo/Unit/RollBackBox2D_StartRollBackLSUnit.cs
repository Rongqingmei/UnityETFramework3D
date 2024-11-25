using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class RollBackBox2D_StartRollBackLSUnit: AEvent<LSWorld, RollBackBox2D>
    {
        // 创建和角色绑定的Box2D的方块
        protected override async ETTask Run(LSWorld scene, RollBackBox2D args)
        {
            // long tLSUnitId = args.LSUnitId;

            Debug.Log("测试 InitBox2D_CreateBox2DBody");
            
            // 找到Box3DControllerSystem
            Scene tNowScene = scene.Scene();
            Room room = tNowScene.GetComponent<Room>();
            
            // Box2D 物理引擎
            Box3DControllerComponent tBox3DController = room.GetComponent<Box3DControllerComponent>();
            foreach (long argsLsUnitId in args.LSUnitIds)
            {
                Box3DComponent tBox3DComponent = tBox3DController.GetChild<Box3DComponent>(argsLsUnitId);
                Box3DComponentSystem.RollBackUpdateBox2D(tBox3DComponent);
            }

            await ETTask.CompletedTask;
        }
    }
}