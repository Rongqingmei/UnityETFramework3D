using System;
using System.Collections.Generic;
using System.IO;
using TrueSync;

namespace ET
{
    [FriendOf(typeof(Room))]
    public static partial class RoomSystem
    {
        public static Room Room(this Entity entity)
        {
            return entity.IScene as Room;
        }
        
        public static void Init(this Room self, List<LockStepUnitInfo> unitInfos, long startTime, int frame = -1)
        {
            LogMsg.Instance.Debug(self.Fiber(), "LSWorld发生了初始化");
            
            self.StartTime = startTime;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.Replay.UnitInfos = unitInfos;
            self.FrameBuffer = new FrameBuffer(frame);
            self.FixedTimeCounter = new FixedTimeCounter(self.StartTime, 0, LSConstValue.UpdateInterval);
            LSWorld lsWorld = self.LSWorld;
            lsWorld.Frame = frame + 1;
            lsWorld.AddComponent<LSUnitComponent>();
            for (int i = 0; i < unitInfos.Count; ++i)
            {
                LockStepUnitInfo unitInfo = unitInfos[i];
                LSUnitFactory.Init(lsWorld, unitInfo);
                self.PlayerIds.Add(unitInfo.PlayerId);
            }
            

            // 添加一个特殊地板对象, 和一个盒子对象
            // 地板对象
            TSVector Position = new TSVector(0,0,0);
            TSQuaternion Rotation = TSQuaternion.identity;
            LSUnitFactory.InitCommonThing(lsWorld, Position, Rotation, 100001);
            
            // 盒子对象
            TSVector Position2 = new TSVector(0,0,0);
            TSQuaternion Rotation2 = TSQuaternion.identity;
            LSUnitFactory.InitCommonThing(lsWorld, Position2, Rotation2, 100002);
        }

        public static void Update(this Room self, OneFrameInputs oneFrameInputs)
        {
            LSWorld lsWorld = self.LSWorld;
            
            // 设置输入到每个LSUnit身上
            LSUnitComponent unitComponent = lsWorld.GetComponent<LSUnitComponent>();
            foreach (var kv in oneFrameInputs.Inputs)
            {
                LSUnit lsUnit = unitComponent.GetChild<LSUnit>(kv.Key);
                LSInputComponent lsInputComponent = lsUnit.GetComponent<LSInputComponent>();
                lsInputComponent.LSInput = kv.Value;
            }
            
            if (!self.IsReplay)
            {
                // 保存当前帧场景数据
                self.SaveLSWorld();
                self.Record(self.LSWorld.Frame);
            }

            lsWorld.Update();
            
            // 发送消息更新物理世界
            List<long> LSUnitIdList = new List<long>();
            foreach (var kv in oneFrameInputs.Inputs)
            {
                LSUnitIdList.Add(kv.Key);
            }
            EventSystem.Instance.Publish(lsWorld, new UpdateBox2D() { LSUnitIds = LSUnitIdList.ToArray() });
        }
        
        public static LSWorld GetLSWorld(this Room self, SceneType sceneType, int frame)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof (LSWorld), memoryBuffer) as LSWorld;
            lsWorld.SceneType = sceneType;
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            return lsWorld;
        }

        private static void SaveLSWorld(this Room self)
        {
            int frame = self.LSWorld.Frame;
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            
            MemoryPackHelper.Serialize(self.LSWorld, memoryBuffer);
            memoryBuffer.Seek(0, SeekOrigin.Begin);

            long hash = memoryBuffer.GetBuffer().Hash(0, (int) memoryBuffer.Length);
            
            self.FrameBuffer.SetHash(frame, hash);
        }

        // 记录需要存档的数据
        public static void Record(this Room self, int frame)
        {
            if (frame > self.AuthorityFrame)
            {
                return;
            }
            OneFrameInputs oneFrameInputs = self.FrameBuffer.FrameInputs(frame);
            OneFrameInputs saveInput = OneFrameInputs.Create();
            oneFrameInputs.CopyTo(saveInput);
            self.Replay.FrameInputs.Add(saveInput);
            if (frame % LSConstValue.SaveLSWorldFrameCount == 0)
            {
                MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
                byte[] bytes = memoryBuffer.ToArray();
                self.Replay.Snapshots.Add(bytes);
            }
        }
    }
}