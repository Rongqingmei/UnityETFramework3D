// using UnityEngine;
// using UnityEditor;
// using System;
// using System.Collections.Generic;
// using BEPUphysics.BroadPhaseEntries.MobileCollidables;
// using BEPUphysics.CollisionShapes.ConvexShapes;
// using BEPUphysics.Entities;
// using BEPUphysics.Entities.Prefabs;
// using Box = BEPUphysics.Entities.Prefabs.Box;
//
// [CustomEditor(typeof(BEPUPhyMgr))]
// public class BEPUPhyMgrEditor : Editor
// {
//     BEPUPhyMgr bEPUPhyMgr;
//
//     void OnEnable()
//     {
//         bEPUPhyMgr = serializedObject.targetObject as BEPUPhyMgr;
//     }
//
//     void OnSceneGUI()
//     {
//         if (bEPUPhyMgr.idController != null)
//         {
//             // 绘制 Box
//             List<Box> boxList = bEPUPhyMgr.idController.GetAllTypeEntity<Box, BoxShape>();
//             foreach (Box entity in boxList)
//             {
//                 Handles.color = Color.cyan;
//                 Vector3 tPosition = new Vector3((float)entity.Position.X, (float)entity.Position.Y, (float)entity.Position.Z);
//                 Quaternion tRotation = new Quaternion((float)entity.Orientation.X, (float)entity.Orientation.Y, (float)entity.Orientation.Z, (float)entity.Orientation.W);
//             
//                 Matrix4x4 rotationMatrix = Matrix4x4.TRS(tPosition, tRotation, Vector3.one);
//                 Handles.matrix = rotationMatrix;
//                 Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
//
//                 Vector3 tSize = new Vector3((float)entity.Width, (float)entity.Height, (float)entity.Length);
//             
//                 // 这里的位置，已经用 TRS 来处理了位置
//                 Handles.DrawWireCube(Vector3.zero, tSize);
//                 Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
//                 Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
//                 Handles.DrawWireCube(Vector3.zero, tSize);
//             }
//             
//             // 绘制 Sphere
//             List<Sphere> sphereList = bEPUPhyMgr.idController.GetAllTypeEntity<Sphere, SphereShape>();
//             foreach (Sphere entity in sphereList)
//             {
//                 Vector3 tPosition = new Vector3((float)entity.Position.X, (float)entity.Position.Y, (float)entity.Position.Z);
//                 
//                 // 计算 EularAngle
//                 Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
//                 Handles.color = Color.cyan;
//                 Handles.DrawWireDisc(tPosition, Vector3.up, (float)entity.Radius);
//                 Handles.DrawWireDisc(tPosition, Vector3.right, (float)entity.Radius);
//                 Handles.DrawWireDisc(tPosition, Vector3.forward, (float)entity.Radius);
//                 Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
//                 Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
//                 Handles.DrawWireDisc(tPosition, Vector3.up, (float)entity.Radius);
//                 Handles.DrawWireDisc(tPosition, Vector3.right, (float)entity.Radius);
//                 Handles.DrawWireDisc(tPosition, Vector3.forward, (float)entity.Radius);
//             }
//             
//         }
//     }
// }
