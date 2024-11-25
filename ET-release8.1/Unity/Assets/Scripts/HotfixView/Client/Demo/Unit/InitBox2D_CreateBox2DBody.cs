using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.All)]
    public class InitBox2D_CreateBox2DBody: AEvent<Scene, InitBox3DBody>
    {
        // 创建和角色绑定的Box3D的方块
        protected override async ETTask Run(Scene scene, InitBox3DBody args)
        {
            long tLSUnitId = args.LSUnitId;

            Debug.Log("测试 InitBox2D_CreateBox3DBody");
            
            // 找到Box3DControllerSystem
            // Scene tNowScene = scene.Root();
            Scene tNowScene = LSSingleton.mainScene;
            Room room = tNowScene.GetComponent<Room>(); 
            // Room room = scene.Parent as Room;
            
            // Box3D 物理引擎
            Box3DControllerComponent tBox3DController = room.GetComponent<Box3DControllerComponent>();
            Box3DComponent tBox3DComponent = tBox3DController.AddChildWithId<Box3DComponent, InitBox3DBody>(tLSUnitId, args);

            await ETTask.CompletedTask;
        }
    }
}