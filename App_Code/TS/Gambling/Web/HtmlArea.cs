using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TS.Gambling.Web
{

    /// <summary>
    /// Summary description for HtmlArea
    /// </summary>
    public class HtmlArea
    {
        public HtmlArea(int top, int left, int width, int height)
        {
            _top = top;
            _left = left;
            _width = width;
            _height = height;
        }

        private int _top;
        private int _left;
        private int _width;
        private int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

    }

}