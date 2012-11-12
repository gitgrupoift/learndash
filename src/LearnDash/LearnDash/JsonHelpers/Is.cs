namespace LearnDash.JsonHelpers
{
    public static class Is
    {
        public class Success
        {
            public static JsonMessage Empty
            {
                get
                {
                    return new JsonMessage
                               {
                                   isSuccess = true,
                                   message = string.Empty
                               };
                }
            }

            public static JsonMessage Message(string message)
            {
                return new JsonMessage 
                           {
                               isSuccess = true,
                               message = message
                           };
            }
        }

        public class Fail
        {
            public static JsonMessage Message(string message)
            {
                return new JsonMessage 
                           {
                               isSuccess = false,
                               message = message
                           };
            }
        }
    }
}