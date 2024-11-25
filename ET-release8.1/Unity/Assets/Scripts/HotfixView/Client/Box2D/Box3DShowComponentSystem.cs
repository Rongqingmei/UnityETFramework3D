// using Testbed.Abstractions;
// using Testbed.TestCases;

using System.Collections.Generic;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;
using UnityEditor;
using UnityEngine;
using Camera = UnityEngine.Camera;

// using Box2DSharp.Testbed.Unity.Inspection;

namespace ET.Client
{
    [EntitySystemOf(typeof(Box3DShowComponent))]
    [FriendOf(typeof(Box3DShowComponent))]
    public static partial class Box3DShowComponentSystem
    {
        [EntitySystem]
        private static void Awake(this Box3DShowComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this Box3DShowComponent self)
        {
            // Client 绘制Box2D物体
            // Debug.Log("渲染Box2D 画面 -----------------");

            // if (HelloWorld.Instance != null)
            // {
            //     HelloWorld.Instance.Render();
            // }
            if (BEPUPhyMgr.GetInstance().idController != null)
            {
                // 绘制 Box
                List<Box> boxList = BEPUPhyMgr.GetInstance().idController.GetAllTypeEntity<Box, BoxShape>();
                foreach (Box entity in boxList)
                {
                    // 位置
                    Vector3 tPosition = new Vector3((float) entity.Position.X, (float) entity.Position.Y, (float) entity.Position.Z);
                    
                    // 旋转
                    Quaternion tRotation = new Quaternion((float) entity.Orientation.X, (float) entity.Orientation.Y, (float) entity.Orientation.Z,
                        (float) entity.Orientation.W);
                    
                    // 大小
                    Vector3 tSize = new Vector3((float) entity.Width, (float) entity.Height, (float) entity.Length);
                    
                    // 颜色
                    Color tColor = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
                    
                    // 这里的位置，已经用 TRS 来处理了位置
                    DrawWireCube(tPosition, tRotation, tSize, tColor);
                }

                // 绘制 Sphere
                // List<Sphere> sphereList = BEPUPhyMgr.GetInstance().idController.GetAllTypeEntity<Sphere, SphereShape>();
                // foreach (Sphere entity in sphereList)
                // {
                //     Vector3 tPosition = new Vector3((float) entity.Position.X, (float) entity.Position.Y, (float) entity.Position.Z);
                //
                //     // 计算 EularAngle
                //     Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
                //     Handles.color = Color.cyan;
                //     Handles.DrawWireDisc(tPosition, Vector3.up, (float) entity.Radius);
                //     Handles.DrawWireDisc(tPosition, Vector3.right, (float) entity.Radius);
                //     Handles.DrawWireDisc(tPosition, Vector3.forward, (float) entity.Radius);
                //     Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
                //     Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
                //     Handles.DrawWireDisc(tPosition, Vector3.up, (float) entity.Radius);
                //     Handles.DrawWireDisc(tPosition, Vector3.right, (float) entity.Radius);
                //     Handles.DrawWireDisc(tPosition, Vector3.forward, (float) entity.Radius);
                // }
            }
        }
        
        // 绘制线框立方体的方法
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, float duration = 0.0f)
        {
            Vector3 halfSize = size / 2;

            // 计算立方体的8个顶点
            Vector3[] points = new Vector3[8];
            points[0] = rotation * new Vector3(-halfSize.x, -halfSize.y, -halfSize.z) + position;
            points[1] = rotation * new Vector3(halfSize.x, -halfSize.y, -halfSize.z) + position;
            points[2] = rotation * new Vector3(halfSize.x, -halfSize.y, halfSize.z) + position;
            points[3] = rotation * new Vector3(-halfSize.x, -halfSize.y, halfSize.z) + position;
            points[4] = rotation * new Vector3(-halfSize.x, halfSize.y, -halfSize.z) + position;
            points[5] = rotation * new Vector3(halfSize.x, halfSize.y, -halfSize.z) + position;
            points[6] = rotation * new Vector3(halfSize.x, halfSize.y, halfSize.z) + position;
            points[7] = rotation * new Vector3(-halfSize.x, halfSize.y, halfSize.z) + position;

            // 绘制立方体的12条边
            
            // 第一圈
            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[3], points[0], color, duration);

            // 第二圈
            Debug.DrawLine(points[4], points[5], color, duration);
            Debug.DrawLine(points[5], points[6], color, duration);
            Debug.DrawLine(points[6], points[7], color, duration);
            Debug.DrawLine(points[7], points[4], color, duration);
            
            // 第一圈 链接 第二圈
            Debug.DrawLine(points[0], points[4], color, duration);
            Debug.DrawLine(points[1], points[5], color, duration);
            Debug.DrawLine(points[2], points[6], color, duration);
            Debug.DrawLine(points[3], points[7], color, duration);
        }
    }
}