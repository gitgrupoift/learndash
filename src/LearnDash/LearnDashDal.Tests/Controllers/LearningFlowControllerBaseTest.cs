using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;
using Moq;

namespace LearnDashDal.Tests.Controllers
{
    using System.Web.Mvc;
    using LearnDash.Controllers;
    using NUnit.Framework;
    using Is = NUnit.Framework.Is;

    public class LearningFlowControllerBaseTest
    {
        protected LearningFlowController controller;

        private IRepository<LearningTask> MoqTaskRepository
        {
            get
            {
                var @taskRepo = new Mock<IRepository<LearningTask>>();

                @taskRepo.Setup(x => x.GetById(TestConsts.NoLearningTaskId))
                    .Returns<LearningTask>(null);

                @taskRepo.Setup(x => x.GetById(TestConsts.ExistingLearningTaskId))
                    .Returns(new LearningTask
                            {
                                ID = TestConsts.ExistingLearningTaskId
                            });

                @taskRepo.Setup(x => x.GetById(TestConsts.ExistingButFailingLearningTaskId))
                    .Returns(new LearningTask
                                 {
                                     ID= TestConsts.ExistingButFailingLearningTaskId
                                 });

                @taskRepo.Setup(x => x.Update(It.Is<LearningTask>(item => item.ID == TestConsts.ExistingLearningTaskId)))
                    .Returns(true);

                @taskRepo.Setup(x => x.Update(It.Is<LearningTask>(item => item.ID == TestConsts.ExistingButFailingLearningTaskId)))
                    .Returns(false);

                return @taskRepo.Object;
            }
        }

        public LearningFlowControllerBaseTest()
        {
            this.controller = new LearningFlowController();
            this.controller.LearningTaskRepository = this.MoqTaskRepository;
        }
    }

    [TestFixture]
    public class LearningFlowTest
    {
        [TestFixture]
        public class RenameItemTest : LearningFlowControllerBaseTest
        {
            [Test]
            public void Action_allways_returns_json_result_type()
            {
                // Act
                var result = controller.RenameItem(TestConsts.ExistingLearningTaskId, string.Empty);

                Assert.That(result, Is.InstanceOf<JsonResult>());
            }

            [Test]
            public void Successfull_actions_return_success_true()
            {
                // Act
                var actualResult = controller.RenameItem(TestConsts.ExistingLearningTaskId, TestConsts.CorrectText);

                // Assert
                Assert.True(TestHelpers.ExtractIsSuccess(actualResult));
            }

            [Test]
            public void If_itemId_less_than_1_return_fail()
            {
                // Act
                var actualResult = controller.RenameItem(-1, TestConsts.CorrectText);

                // Assert
                Assert.False(TestHelpers.ExtractIsSuccess(actualResult));
            }

            [Test]
            public void If_itemname_white_null_empty_return_fail()
            {
                // Act
                var actualResult = controller.RenameItem(TestConsts.ExistingLearningTaskId, string.Empty);

                // Assert
                Assert.False(TestHelpers.ExtractIsSuccess(actualResult));
            }

            [Test]
            public void If_there_is_no_learning_item_return_fail()
            {
                // Act
                var actualResult = controller.RenameItem(TestConsts.NoLearningTaskId, TestConsts.CorrectText);

                // Assert
                Assert.False(TestHelpers.ExtractIsSuccess(actualResult));
            }

            [Test]
            public void If_update_of_item_ok_return_success()
            {
                // Act
                var actualResult = controller.RenameItem(TestConsts.ExistingLearningTaskId, TestConsts.CorrectText);

                // Assert
                Assert.True(TestHelpers.ExtractIsSuccess(actualResult));
            }

            [Test]
            public void If_update_of_item_fails_return_fail()
            {
                // Act
                var actualResult = controller.RenameItem(TestConsts.ExistingButFailingLearningTaskId, TestConsts.CorrectText);

                // Assert
                Assert.False(TestHelpers.ExtractIsSuccess(actualResult));
            }
        }
    }
}