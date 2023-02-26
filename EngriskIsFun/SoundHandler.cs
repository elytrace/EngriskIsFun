using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace EngriskIsFun
{
    public class SoundHandler
    {
        public static SoundPlayer correctSF = new SoundPlayer("Materials/Sounds/correct.wav");
        public static SoundPlayer incorrectSF = new SoundPlayer("Materials/Sounds/incorrect.wav");
        public static SoundPlayer winSF = new SoundPlayer("Materials/Sounds/win.wav");
        public static SoundPlayer loseSF = new SoundPlayer("Materials/Sounds/lose.wav");
    }
}
