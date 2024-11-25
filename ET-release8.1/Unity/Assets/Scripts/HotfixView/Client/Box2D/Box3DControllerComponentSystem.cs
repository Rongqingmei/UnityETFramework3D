using TrueSync;
// using Testbed.Abstractions;
// using Testbed.TestCases;
using UnityEngine;
using Camera = UnityEngine.Camera;

// using Box2DSharp.Testbed.Unity.Inspection;

namespace ET.Client
{
    // [LSEntitySystemOf(typeof(Box3DControllerComponent))]
    [EntitySystemOf(typeof(Box3DControllerComponent))]
    [FriendOf(typeof(Box3DControllerComponent))]
    public static partial class Box3DControllerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this Box3DControllerComponent self)
        {
            int a = 10;
            Debug.LogError("打印一下为什么这里删除了");
        }

        [EntitySystem]
        private static void Awake(this Box3DControllerComponent self)
        {
            // 初始化BOX2D的帧
            // Debug.Log("加载Box2D ----------------3");
            InitBox3D();
            // Debug.Log("加载Box2D ----------------4");
            
            // 给每个角色添加BOX2D
            AddPlayerBox(self);
        }
        

        // 给每个角色添加BOX2D
        private static void AddPlayerBox(this Box3DControllerComponent self)
        {

        }

        // [EntitySystem]
        // private static void Update(this Box3DControllerComponent self)
        [EntitySystem]
        private static void Update(this Box3DControllerComponent self)
        {
        // 更新BOX2D的帧
        // Debug.Log("加载Box2D 更新 ----------------5");
        
        // HelloWorld.Instance.Render();
        }

        private static void InitBox3D()
        {
            // 初始化
            BEPUPhyMgr.GetInstance().InitBEPUPhy();
        }

        // box3d的帧
        public static void RunBox3DTick()
        {
            BEPUPhyMgr.GetInstance().Step();
        }
    }
}