using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class UpdateBox2D_DriveBox2D: AEvent<LSWorld, UpdateBox2D>
    {
        protected override async ETTask Run(LSWorld scene, UpdateBox2D args)
        {
            long[] tLSUnitIds = args.LSUnitIds;
            
            Debug.Log("测试UpdateBox2D_DriveBox2D");
            
            // 更新BOX3D的帧
            
            // 所有人都一样，一个服务器帧，物理系统就运行 0.033f 秒
            float tFrameTime = 0.033f;
            
            // Box2D 跑物理帧
            Box3DControllerComponentSystem.RunBox3DTick();
            // BEPUPhyMgr.GetInstance().Step();
            
            // 更新每个Box2D组件, 记录所有LSUnit的帧数据
            Room tNowRoom = scene.Parent as Room;
            
            Scene tNowScene = scene.Root();
            Room room = tNowScene.GetComponent<Room>();
            
            // 更新每个Box2D组件
            Box3DControllerComponent tBox3DController = room.GetComponent<Box3DControllerComponent>();
            foreach (long argsLsUnitId in args.LSUnitIds)
            {
                Box3DComponent tBox3DComponent = tBox3DController.GetChild<Box3DComponent>(argsLsUnitId);
                Box3DComponentSystem.LSUpdate(tBox3DComponent);
            }
            
            
            await ETTask.CompletedTask;
        }
    }
}