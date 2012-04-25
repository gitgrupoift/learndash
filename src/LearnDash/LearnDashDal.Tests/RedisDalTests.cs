using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LearnDash.Dal;
using NUnit.Framework;

namespace LearnDashDal.Tests
{
    [TestFixture]
    public class RedisDalTests
    {
        [Test]
        public  void Learning_flow_dal_test()
        {
            #region Arrange

            var learningFlow = new LearningFlow("Test")
                                   {
                                       Tasks = new Collection<LearningTask>
                                                   {
                                                       new LearningTask(),
                                                       new LearningTask(),
                                                       new LearningTask()
                                                   }
                                   };


            #endregion

            #region Act Assert

            var returnedId = RedisDal.Save(learningFlow);
            Assert.That(returnedId,Is.GreaterThan(0));

            var currentFlow = RedisDal.GetAll();

            Assert.That(currentFlow.Count,Is.EqualTo(1));

            var flow = RedisDal.Get(returnedId);
            Assert.That(flow.Id,Is.EqualTo(returnedId));
            Assert.That(flow.Tasks.Count,Is.EqualTo(3));
            Assert.That(flow.Name, Is.EqualTo("Test"));

            RedisDal.Remove(returnedId);

            var currentFlows = RedisDal.GetAll();
            Assert.That(currentFlows.Count,Is.EqualTo(0));


            #endregion
        }
			
    }
}
