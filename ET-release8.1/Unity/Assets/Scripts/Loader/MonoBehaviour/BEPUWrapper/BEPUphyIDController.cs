using System;
using System.Collections.Generic;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using Space = BEPUphysics.Space;
using Entity = BEPUphysics.Entities.Entity;

public class BEPUphyIDController
{
    public Space Space;
    
    public BEPUphyIDController(Space pSpace)
    {
        Space = pSpace;
    }

    // 按照 id 存储
    private Dictionary<long, Entity> _entityDictById;
    // 按照 entity 存储 id (用于碰撞)
    private Dictionary<Entity, long> _entityDictByEntity;

    // 初始化
    public void InitEntity()
    {
        // 删除之前的
        if (_entityDictById != null)
        {
            foreach (var keyValuePair in this._entityDictById)
            {
                // 删除 Phisic
                Space.Remove(keyValuePair.Value);
            }
        }
        
        // 配置
        _entityDictById = new Dictionary<long, Entity>();
        _entityDictByEntity = new Dictionary<Entity, long>();
    }
    
    // 获取某个种类的所有 Entity
    public List<T1> GetAllTypeEntity<T1,T2>() where T1 : Entity<ConvexCollidable<T2>> where T2 : ConvexShape
    {
        List<T1> tRes = new List<T1>();
        foreach (var keyValuePair in this._entityDictById)
        {
            T1 entity = keyValuePair.Value as T1;
            if (entity != null && entity.GetType() == typeof(T1))
            {
                tRes.Add(entity);
            }
        }
        return tRes;
    }
    
    // 添加一个 entity
    public void AddEntityById(Entity pEntity, long pId)
    {
        if (!this._entityDictById.ContainsKey(pId))
        {
            // 添加
            _entityDictById.Add(pId, pEntity);
            _entityDictByEntity.Add(pEntity, pId);
            Space.Add(pEntity);
        }
        else
        {
            // 如果已经有了，则先删除之前的
            Space.Remove(_entityDictById[pId]);

            // 再添加现在的
            _entityDictById[pId] = pEntity;
            _entityDictByEntity[pEntity] = pId;
            Space.Add(pEntity);
        }
        
    }

    // 删除一个 entity
    public void RemoveEntityById(long pId)
    {
        // 删除之前的
        Space.Remove(_entityDictById[pId]);
        if (this._entityDictById.ContainsKey(pId))
        {
            // 删除
            _entityDictByEntity.Remove(_entityDictById[pId]);
            _entityDictById.Remove(pId);
        }
    }
}
