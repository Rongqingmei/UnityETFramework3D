using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.All)]
    public class InputBox2D_InputAndMoveLSUnit: AEvent<Scene, InputBox2DBody>
    {
        // 发送角色控制的输入
        protected override async ETTask Run(Scene scene, InputBox2DBody args)
        {
            long tLSUnitId = args.LSUnitId;
            LSInput tLSInput= args.lsInput;

            Debug.Log("测试 InitBox2D_CreateBox3DBody");
            
            // 找到Box3DControllerSystem
            Scene tNowScene = scene.Scene();
            Room room = tNowScene.GetComponent<Room>();
            
            // Box2D 物理引擎
            Box3DControllerComponent tBox3DController = room.GetComponent<Box3DControllerComponent>();
            Box3DComponent Box3DComponent = tBox3DController.GetChild<Box3DComponent>(tLSUnitId);
            
            // 输入Box2D物理的变化
            Box3DComponentSystem.InputeAndUpdateBox2D(Box3DComponent, tLSInput);
            // Box3DComponent tBox3DComponent = tBox3DController.AddChild<Box3DComponent, InitBox3DBody>(args);

            await ETTask.CompletedTask;
        }
    }
}