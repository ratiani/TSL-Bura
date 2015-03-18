using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace TS.Gambling.Web
{

    /// <summary>
    /// Summary description for HtmlResponse
    /// </summary>
    public class HtmlResponse
    {
        public HtmlResponse()
        {
        }

        private StringBuilder _html = new StringBuilder();
        private StringBuilder _script = new StringBuilder();

        public StringBuilder Script
        {
            get { return _script; }
            set { _script = value; }
        }

        public StringBuilder Html
        {
            get { return _html; }
            set { _html = value; }
        }
        
    }

}