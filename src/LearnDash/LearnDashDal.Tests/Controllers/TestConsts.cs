using System;

namespace LearnDashDal.Tests.Controllers
{
    public static class TestConsts
    {
        public static string CorrectText = "correctText";
        public static string FailText = "failText";
        public static int NoLearningTaskId = Int32.MaxValue;
        public static int FailingTaskId = Int32.MaxValue;
        public static int ExistingLearningTaskId = 1;
        public static int ExistingButFailingLearningTaskId = 999;
    }
}