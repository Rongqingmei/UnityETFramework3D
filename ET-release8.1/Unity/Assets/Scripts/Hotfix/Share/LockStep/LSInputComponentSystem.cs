using System;
using ET.Client;
// using Testbed.TestCases;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSInputComponent))]
    [LSEntitySystemOf(typeof(LSInputComponent))]
    public static partial class LSInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSInputComponent self)
        {

        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSInputComponent self)
        {
            LSUnit unit = self.GetParent<LSUnit>();

            TSVector2 v2 = self.LSInput.V * 6 * 50 / 1000;
            if (v2.LengthSquared() < 0.0001f)
            {
                return;
            }
            
            // 发生了有移动的输入
            // LogMsg.Instance.Debug(self.Fiber(), "发生有移动的输入 LSInputComponentSystem");
            // LogMsg.Instance.Debug(self.Fiber(), self.LSInput);
            
            // TSVector oldPos = unit.Position;
            // unit.Position += new TSVector(v2.x, 0, v2.y);
            // unit.Forward = unit.Position - oldPos;
            
            // 发送消息，通知Box2D进行移动，和影响对应的LSUnit
            // 发送消息添加物理系统对象
            EventSystem.Instance.Publish(LSSingleton.mainScene, new InputBox2DBody()
            {
                LSUnitId = unit.Id,
                lsInput = self.LSInput,
            });
        }
    }
}