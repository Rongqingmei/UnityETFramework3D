using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSUnitView))]
    [LSEntitySystemOf(typeof(LSUnitView))]
    [FriendOf(typeof(LSUnitView))]
    public static partial class LSUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this LSUnitView self, GameObject go)
        {
            self.GameObject = go;
            self.Transform = go.transform;

        }

        [LSEntitySystem]
        private static void LSRollback(this LSUnitView self)
        {
            //LSUnit unit = self.GetUnit();
            //self.Transform.position = unit.Position.ToVector();
            //self.Transform.rotation = unit.Rotation.ToQuaternion();
            //self.t = 0;
            //self.totalTime = 0;
        }

        [EntitySystem]
        private static void Update(this LSUnitView self)
        {
            LSUnit unit = self.GetUnit();
            
            // 这里改为BOX2D
            // 找到Box3DControllerSystem
            Scene tNowScene = self.Root();
            Room room = tNowScene.GetComponent<Room>();
            
            // Box2D 物理引擎
            Box3DControllerComponent tBox3DController = room.GetComponent<Box3DControllerComponent>();
            
            // 普通物体，直接跳过操控
            if (unit.Id < 500000)
            {
                return;
            }
            
            Box3DComponent Box3DComponent = tBox3DController.GetChild<Box3DComponent>(unit.Id);
            
            BEPUutilities.Vector3 tPos = Box3DComponentSystem.GetBodyPos(Box3DComponent);
            Vector3 unitPos = new Vector3(tPos.X.AsFloat(), tPos.Y.AsFloat(), tPos.Z.AsFloat());
            
            
            const float speed = 6f;
            float speed2 = speed;// * self.Room().SpeedMultiply;
            
            if (unitPos != self.Position)
            {
                float distance = (unitPos - self.Position).magnitude;
                self.totalTime = distance / speed2;
                self.t = 0;
                self.Position = unitPos;
                // self.Rotation = unit.Rotation.ToQuaternion();
            }
            
            
            LSInput input = unit.GetComponent<LSInputComponent>().LSInput;
            if (input.V != TSVector2.zero)
            {
                // self.GetComponent<LSAnimatorComponent>().SetFloatValue("Speed", speed2);
                LSAnimatorComponent animator = self.GetComponent<LSAnimatorComponent>();
                if (animator != null)
                {
                    animator.SetFloatValue("Speed", speed2);
                }
            }
            else
            {
                // self.GetComponent<LSAnimatorComponent>().SetFloatValue("Speed", 0);
                LSAnimatorComponent animator = self.GetComponent<LSAnimatorComponent>();
                if (animator != null)
                {
                    animator.SetFloatValue("Speed", 0);
                }
            }
            self.t += Time.deltaTime;
            // rongqingmei 临时屏蔽
            // self.Transform.rotation = Quaternion.Lerp(self.Transform.rotation, self.Rotation, self.t / 1f);
            self.Transform.position = Vector3.Lerp(self.Transform.position, self.Position, self.t / self.totalTime);
        }

        private static LSUnit GetUnit(this LSUnitView self)
        {
            LSUnit unit = self.Unit;
            if (unit != null)
            {
                return unit;
            }

            self.Unit = (self.IScene as Room).LSWorld.GetComponent<LSUnitComponent>().GetChild<LSUnit>(self.Id);
            return self.Unit;
        }
    }
}