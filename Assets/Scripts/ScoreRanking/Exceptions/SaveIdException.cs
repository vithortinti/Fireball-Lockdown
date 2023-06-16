using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireballLockdownGame.ScoreRanking.Exceptions
{
    internal class SaveIdException : Exception
    {
        public SaveIdException(string message) : base(message)
        {
            
        }
    }
}
