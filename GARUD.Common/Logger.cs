using System;
using System.Text;
using log4net;

namespace GARUD.Common
{
    public class Logger
    {
        private static ILog _logger = null;

        public enum LogTypeEnum { DEBUG, INFO, ERROR, FATAL };

        public static void Log(string message, Exception ex)
        {
            if (_logger == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                _logger = LogManager.GetLogger("default");
            }

            var fullMessage = new StringBuilder(message);
            int i = 1;

            while (ex != null)
            {
                fullMessage.AppendFormat("Exception Type: {0}\n\nMessage:\n========\n{1}\n\nStack Trace:\n============\n{2}",
                    ex.GetType().FullName, ex.Message, ex.StackTrace);

                ex = ex.InnerException;

                fullMessage.Append("InnerException " + (i++).ToString() + "\n");
            }

            _logger.Error(fullMessage);
        }

        public static void Log(string message)
        {
            Log(message, null);
        }

        public static void Log(Exception ex)
        {
            Log(string.Empty, ex);
        }
    }
}
