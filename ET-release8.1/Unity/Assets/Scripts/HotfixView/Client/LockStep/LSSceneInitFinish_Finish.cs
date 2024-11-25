using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneInitFinish_Finish: AEvent<Scene, LSSceneInitFinish>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneInitFinish args)
        {
            Room room = clientScene.GetComponent<Room>();
            
            Debug.Log("加载 Box3D 物理引擎 ----------------1");
            
            // 加载 Box2D 物理引擎
            // LSWorld lsWorld = room.LSWorld;
            Box3DControllerComponent tBoxController = room.AddComponent<Box3DControllerComponent>();
            
            // 记录当前的主要 MainScene, 用来LSScene给普通MainScene发消息通知
            LSSingleton.mainScene = clientScene;
                
            // 给每个物体挂上Box2D组件
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            foreach (long playerId in room.PlayerIds)
            {
                LSUnit lsUnit = lsUnitComponent.GetChild<LSUnit>(playerId);
                // 发送消息添加物理系统对象
                EventSystem.Instance.Publish(clientScene, new InitBox3DBody()
                {
                    LSUnitId = playerId,
                    Position = lsUnit.Position,
                    Rotation = lsUnit.Rotation,
                });
            }

            // 加载 Box2D 显示部分
            Debug.Log("加载 Box2D 显示部分 ----------------2");
            room.AddComponent<Box3DShowComponent>();
            
            // 这里有AB加载的等待逻辑
            // 这里给每个物体都加载各自的组件，包括每个物体挂上 Box2D组件
            await room.AddComponent<LSUnitViewComponent>().InitAsync();
            
            room.AddComponent<LSCameraComponent>();

            if (!room.IsReplay)
            {
                room.AddComponent<LSOperaComponent>();
            }

            await UIHelper.Remove(clientScene, UIType.UILSLobby);
            await ETTask.CompletedTask;
        }
    }
}