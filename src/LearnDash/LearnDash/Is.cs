namespace LearnDash
{
    public static class Is
    {
        public class Success
        {
            public static object Empty
            {
                get
                {
                    return new { isSuccess = true, message = string.Empty };
                }
            }

            public static object Message(string message)
            {
                return new
                           {
                               isSuccess = true,
                               message
                           };
            }
        }

        public class Fail
        {
            public static object Message(string message)
            {
                return new
                           {
                               isSuccess = false,
                               message
                           };
            }
        }
    }
}