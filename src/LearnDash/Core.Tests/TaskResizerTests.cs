using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public class TaskResizerTests
    {
        private TaskResizer taskResizer;

        public TaskResizerTests()
        {
                this.taskResizer = new TaskResizer();
        }

        #region Resize

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resize_if_list_null_then_throws()
        {
            // Arrange
            List<IResizable> list = null;

            // Act
            this.taskResizer.Resize(list);
        }

        [Test]
        public void Resize_if_empty_collection_then_return_empty_collection()
        {
            // Arrange
            var empty_collection = new List<IResizable>();

            // Act
            var emptyList = taskResizer.Resize(empty_collection);

            // Assert
            Assert.That(emptyList, Is.Empty);
        } 

        #endregion

        #region CalculateDifference

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateDifference_if_list_null_then_throws()
        {
            // Act
            this.taskResizer.CalculateDifference(null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateDifference_if_list_empty_then_throw()
        {
            // Act
            this.taskResizer.CalculateDifference(new List<IResizable>());
        }

        [Test]
        public void CaclulateDifference_if_one_element_then_return_his_counter_as_difference()
        {
            // Arrange
            var expectedResult = 10;

            var @element = new Mock<IResizable>();
            @element.SetupGet(x => x.Counter).Returns(expectedResult);

            var one_element_list = new List<IResizable>{ @element.Object };

            // Act
            var actualResult = this.taskResizer.CalculateDifference(one_element_list);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(10,20 ,10)]
        [TestCase(1, 20, 19)]
        [TestCase(1, 41, 40)]
        public void CalculateDifference_if_more_than_one_element_calaculate_correct_difference(int lower, int higher, int result)
        {
            // Arrange
            var expectedResult = result;

            var @element = new Mock<IResizable>();
            @element.SetupGet(x => x.Counter).Returns(lower);

            var @element1 = new Mock<IResizable>();
            @element1.SetupGet(x => x.Counter).Returns(higher);

            var list = new List<IResizable>{ @element.Object, @element1.Object };

            // Act
            var actualResult = this.taskResizer.CalculateDifference(list);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateDifference_if_any_element_negative_throws()
        {
            //Arrange
            var @element = new Mock<IResizable>();
            @element.SetupGet(x => x.Counter).Returns(-99);

            // Act
            this.taskResizer.CalculateDifference(new List<IResizable>{ @element.Object });
        }

        #endregion
		
    }
}
