using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Extensions
{
    public static class NLogLogger
    {
        public static string ExtractInnerException(Exception ex)
        {
            var innerException = String.Empty;
            if (ex.InnerException != null)
                innerException = ExtractInnerException(ex.InnerException);

            return String.Format("{0} \r\n   {1}", ex.Message, innerException);
        }

        public static void ErrorExceptionsWithInner(this NLog.Logger logger,string message,Exception ex)
        {
            var innerExceptions = String.Empty;
            if(ex.InnerException != null)
                innerExceptions = ExtractInnerException(ex.InnerException);

            logger.Error(String.Format("{0} Exception\r\n {1} \r\n--------------\r\n {2}", message, ex.Message, innerExceptions));
        }
    }
}
