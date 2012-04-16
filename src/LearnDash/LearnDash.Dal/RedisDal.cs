using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace LearnDash.Dal
{
    /// <summary>
    /// This class should be used to store Redis specific properties that can be 
    /// inherited by other entities 
    /// </summary>
    public class RedisEntity
    {
        /// <summary>
        /// Id used by redis dont use ID
        /// </summary>
        public long Id { get; set; }

        public RedisEntity()
        {
            Id = -1;
        }
    }

    public static class RedisDal
    {
        /// <summary>
        /// This emthod creates new entity if Id <=0
        /// </summary>
        /// <param name="flow"></param>
        /// <returns></returns>
        public static long Save(LearningFlow flow)
        {
            var redisManager = new PooledRedisClientManager("localhost:6379");
            redisManager.ExecAs<LearningFlow>(redisProject =>
                                                      {
                                                          if (flow.Id <= 0)
                                                          {
                                                              flow.Id = redisProject.GetNextSequence();
                                                          }
                                                          redisProject.Store(flow);
                                                      });

            return flow.Id;
        }

        public static LearningFlow Get(long Id)
        {
            //Thread-safe client factory
            var redisManager = new PooledRedisClientManager("localhost:6379");
            LearningFlow flow = null;

            redisManager.ExecAs<LearningFlow>(redisProject =>
                                                      {
                                                          flow = redisProject.GetById(Id);
                                                      });

            return flow;
        }

        //todo : this should not get all the data about projects only the ones that are needed in the menu
        public static List<LearningFlow> GetAll()
        {
            var redisManager = new PooledRedisClientManager("localhost:6379");
            IList<LearningFlow> flows = null;

            redisManager.ExecAs<LearningFlow>(redisProject =>
                                                      {
                                                          flows = redisProject.GetAll();
                                                      });

            return (List<LearningFlow>)flows;
        }

        public static void Remove(long Id)
        {
            var redisManager = new PooledRedisClientManager("localhost:6379");

            redisManager.ExecAs<LearningFlow>(redisProject => redisProject.DeleteById(Id));
        }
    }
}
