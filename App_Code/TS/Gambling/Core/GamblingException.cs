using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for GamblingException
    /// </summary>
    public class GamblingException : Exception
    {
        public GamblingException(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }

}