using System;

namespace InrapporteringsPortal.Web.Helpers
{
    public class ErrorManager
    {
         #region Public Methods

        public static void WriteToErrorLog(string className, string methodName, string message, int errorcode = 0)
        {

            try
            {
                string errorMessage = "";
                if (errorcode != 0)
                {
                    errorMessage = "Code: " + errorcode + "\n" + "Message: " + message;
                }
                else
                {
                    errorMessage = "Message: " + message;
                }
                FileLogWriter _log = new FileLogWriter();
           
                _log.WriteExceptionLog("An exception occurred in " + className + ", " + methodName + ". Error: " + errorMessage);
            }
            catch (Exception e)
            {
                //ToDO..
                var t = e.ToString();
            }
        }

        #endregion
    }
    
}