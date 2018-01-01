using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    interface Iquestion
    {
        string buildQuestion();
        string buildClue();
        string[] buildAnswers();
        string buildTrueAnswer();
    }
}
