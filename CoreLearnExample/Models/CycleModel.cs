using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Models
{
    /// <summary>
    /// 测试依赖注入的三种什么周期
    /// </summary>

    //基类接口
    public interface ICycle
    {
        Guid CycleId { get; }
    }
    //暂时性
    public interface ICycleTransient : ICycle
    {
    }
    //作用域
    public interface ICycleScoped : ICycle
    {
    }
    //单一实例
    public interface ICycleSingleton : ICycle
    {
    }
    //实例
    public interface ICycleSingletonInstance : ICycle
    {
    }

    //测试依赖注入的生命周期实体类
    public class CycleModel : ICycleTransient, ICycleScoped,
    ICycleSingleton,
    ICycleSingletonInstance
    {
        public CycleModel() : this(Guid.NewGuid())
        {
        }

        public CycleModel(Guid id)
        {
            CycleId = id;
        }

        public Guid CycleId { get; private set; }
    }


    //服务
    public class CycleService
    {
        public CycleService(
            ICycleTransient transientCycle,
            ICycleScoped scopedCycle,
            ICycleSingleton singletonCycle,
            ICycleSingletonInstance instanceCycle)
        {
            TransientCycle = transientCycle;
            ScopedCycle = scopedCycle;
            SingletonCycle = singletonCycle;
            SingletonInstanceCycle = instanceCycle;
        }

        public ICycleTransient TransientCycle { get; }
        public ICycleScoped ScopedCycle { get; }
        public ICycleSingleton SingletonCycle { get; }
        public ICycleSingletonInstance SingletonInstanceCycle { get; }
    }
}
