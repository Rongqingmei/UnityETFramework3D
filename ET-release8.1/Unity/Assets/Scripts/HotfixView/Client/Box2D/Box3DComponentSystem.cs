// using Box2DSharp.Common;
// using Testbed.TestCases;
using TrueSync;
using UnityEngine;

namespace ET.Client
{

    // [LSEntitySystemOf(typeof(Box3DComponent))]
    [EntitySystemOf(typeof(Box3DComponent))]
    [FriendOf(typeof(Box3DComponent))]
    public static partial class Box3DComponentSystem
    {
        [EntitySystem]
        private static void Awake(this Box3DComponent self, InitBox3DBody box2DInfo)
        {
            // 创建一个BOX2D
            self.body = BEPUPhyMgr.GetInstance().CreateOneBox3D(box2DInfo.Position.x, box2DInfo.Position.y, box2DInfo.Position.z, box2DInfo.LSUnitId);
        }

        public static void LSUpdate(this Box3DComponent self)
        {
            
        }

        /// <summary>
        /// 回退 Box2D
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pLSInput"></param>
        public static void RollBackUpdateBox2D(this Box3DComponent self)
        {
            // Debug.Log("---------------打印，RollBackUpdateBox2D------------------");
            //
            // // 获取单位的 GameObject
            // LSUnit lsUnit = GetUnit(self);
            //
            // // 强制更新位置
            // // Box2DSharp.Common.FP v2X = new Box2DSharp.Common.FP(lsUnit.Position.x._serializedValue);
            // // Box2DSharp.Common.FP v2Y = new Box2DSharp.Common.FP(lsUnit.Position.y._serializedValue);
            // FVector2 newBox2DPos = new FVector2(lsUnit.Position.x, lsUnit.Position.y);
            //
            // // 还原 Rotation
            // // Box2DSharp.Common.FP tRot = new Box2DSharp.Common.FP(lsUnit.Rotation.eulerAngles.z._serializedValue);
            //
            // // 还原 Box2D 的位置
            // self.body.SetTransform(newBox2DPos, lsUnit.Rotation.eulerAngles.z);
            //
            // // 还原 AngularVelocity
            // // Box2DSharp.Common.FP tAngularV = new Box2DSharp.Common.FP(lsUnit.AngularVelocity._serializedValue);
            // self.body.SetAngularVelocity(lsUnit.AngularVelocity);
            //
            // // 还原 SetLinearVelocity
            // // Box2DSharp.Common.FP tLinearV2X = new Box2DSharp.Common.FP(lsUnit.LinearVelocity.x._serializedValue);
            // // Box2DSharp.Common.FP tLinearV2Y = new Box2DSharp.Common.FP(lsUnit.LinearVelocity.y._serializedValue);
            // FVector2 tLinearVelocity = new FVector2(lsUnit.LinearVelocity.x, lsUnit.LinearVelocity.y);
            // self.body.SetLinearVelocity(tLinearVelocity);
            //
            // // 刷新数值
            // LSUpdate(self);
        }

        /// <summary>
        /// 输入并且更新 Box2D
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pLSInput"></param>
        public static void InputeAndUpdateBox2D(this Box3DComponent self, LSInput pLSInput)
        {
            // 获取单位的 GameObject
            LSUnit lsUnit = GetUnit(self);
            
            BEPUutilities.Vector3 tBox3DPos = self.body.Position;
            
            TSVector2 v3 = pLSInput.V * 6 * 50 / 1000;
            
            // 应用走路
            BEPUutilities.Vector3 oldBox3DPos = tBox3DPos;
            BEPUutilities.Vector3 newBox3DPos = oldBox3DPos + new BEPUutilities.Vector3(v3.x, (FP)0, v3.y);
            
            // 设置 Box2D 的位置和角度
            self.body.Position = newBox3DPos;
        }

        public static BEPUutilities.Vector3 GetBodyPos(this Box3DComponent self)
        {
            return self.body.Position;
        }
        
        /// <summary>
        /// 根据 Box2dComponet 记录的id找到对应的 LSUnit
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private static LSUnit GetUnit(this Box3DComponent self)
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