namespace LearnDashDal.Tests.Controllers
{
    using System.Collections.ObjectModel;

    using LearnDash.Controllers;
    using LearnDash.Dal.Models;
    using LearnDash.Services;

    using Moq;

    using NUnit.Framework;

    public class FlowControllerTests
    {
        private const int ExistingFlowId = 1;
        private const int NonExistingFlowId = -1;
        private readonly FlowsController controller;

        private readonly Mock<ILearningFlowService> serviceMock = new Mock<ILearningFlowService>();

        public FlowControllerTests()
        {
            this.serviceMock.Setup(x => x.Get(1))
                .Returns(new LearningFlow
                {
                    Tasks = new Collection<LearningTask>()
                });

            this.controller = new FlowsController(this.serviceMock.Object);
        }

        [Test]
        public void Get_if_flow_exists_return_flow()
        {
            // Act
            var actualResult = this.controller.Get(ExistingFlowId);

            // Assert
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Tasks, Is.Not.Null);
        }

        [Test]
        public void Get_if_flow_doesnt_exists_return_null()
        {
            // Act
            var actualResult = this.controller.Get(NonExistingFlowId);

            // Assert
            Assert.That(actualResult, Is.Null);
        }
    }
}
