using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ChildOf(typeof(LSUnitComponent))]
    [MemoryPackable]
    public partial class LSUnit: LSEntity, IAwake, ISerializeToEntity
    {
        [MemoryPackIgnore]
        public TSVector Position
        {
            get;
            set;
        }

        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get;
            set;
        }
        
        [MemoryPackIgnore]
        public TSQuaternion Rotation
        {
            get;
            set;
        }
        
        // 角度的旋转速度 AngularVelocity
        [MemoryPackIgnore]
        public FP AngularVelocity
        {
            get;
            set;
        }
        
        // 线性的旋转速度 LinearVelocity
        [MemoryPackIgnore]
        public TSVector2 LinearVelocity
        {
            get;
            set;
        }
    }
}