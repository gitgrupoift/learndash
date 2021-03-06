﻿namespace Core.Extensions
{
    using System;
    using NLog;

    public static class NLogLogger
    {
        public static string ExtractInnerException(Exception ex)
        {
            var innerException = string.Empty;

            if (ex.InnerException != null)
            {
                innerException = ExtractInnerException(ex.InnerException);
            }

            return string.Format("{0} \r\n   {1}", ex.Message, innerException);
        }

        public static void ErrorExceptionsWithInner(this Logger logger, string message,Exception ex)
        {
            var innerExceptions = "No Inner Exception";

            if (ex.InnerException != null)
            {
                innerExceptions = ExtractInnerException(ex.InnerException);
            }

            logger.Error(string.Format("{0} Exception\r\n {1} \r\n--------------\r\n {2}", message, ex.Message, innerExceptions));
        }
    }
}