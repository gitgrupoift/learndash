using System.Collections.Generic;
using LearnDash.Dal.Models;
using ServiceStack.Redis;

namespace LearnDash.Dal.Redis.Repositories
{
    //public static class LearningFlowRepository
    //{
    //    /// <summary>
    //    /// This method creates new entity if Id <=0
    //    /// </summary>
    //    /// <param name="flow"></param>
    //    /// <returns></returns>
    //    public static long Save(LearningFlow flow)
    //    {
    //        var redisManager = RedisDal.GetClient();

    //        redisManager.ExecAs<LearningFlow>(redisProject =>
    //        {
    //            if (flow.ID <= 0)
    //            {
    //                flow.ID = redisProject.GetNextSequence();
    //            }
    //            redisProject.Store(flow);
    //        });

    //        return flow.ID;
    //    }

    //    public static LearningFlow Get(long Id)
    //    {
    //        //Thread-safe client factory
    //        var redisManager = RedisDal.GetClient();
    //        LearningFlow flow = null;

    //        redisManager.ExecAs<LearningFlow>(redisProject =>
    //                                                  {
    //                                                      flow = redisProject.GetById(Id);
    //                                                  });
    //        return flow;
    //    }

    //    public static List<LearningFlow> GetAll()
    //    {
    //        var redisManager = RedisDal.GetClient();
    //        IList<LearningFlow> flows = null;

    //        redisManager.ExecAs<LearningFlow>(redisProject =>
    //                                                  {
    //                                                      flows = redisProject.GetAll();
    //                                                  });

    //        return (List<LearningFlow>)flows;
    //    }

    //    public static void Remove(long Id)
    //    {
    //        var redisManager = RedisDal.GetClient();

    //        redisManager.ExecAs<LearningFlow>(redisProject => redisProject.DeleteById(Id));
    //    }
    //}
}
