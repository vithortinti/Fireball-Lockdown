using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireballLockdownGame.ScoreRanking.Exceptions
{
    public class SaveNameAndIdException : Exception
    {
        public SaveNameAndIdException(string message) : base(message)
        {
            
        }
    }
}
