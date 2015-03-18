using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Gambling.Core
{

    /// <summary>
    /// Summary description for IdGenerator
    /// </summary>
    public class IdGenerator
    {
        public IdGenerator()
        {
        }

        private static int currentId = 0;

        public static int NextValue
        {
            get
            {
                currentId++;
                return currentId;
            }
        }
    }

    public class GameIdGenerator : IdGenerator
    {
        public GameIdGenerator()
        {
        }

    }

}