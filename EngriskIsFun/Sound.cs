using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace EngriskIsFun
{
    public class Sound
    {
        private static SoundPlayer correctSF = new SoundPlayer("Materials/Sounds/correct.wav");
        private static SoundPlayer incorrectSF = new SoundPlayer("Materials/Sounds/incorrect.wav");
        private static SoundPlayer winSF = new SoundPlayer("Materials/Sounds/win.wav");
        private static SoundPlayer loseSF = new SoundPlayer("Materials/Sounds/lose.wav");
        private static SoundPlayer hovingSF = new SoundPlayer("Materials/Sounds/hoving.wav");

        public static int CORRECT = 0;
        public static int INCORRECT = 1;
        public static int WIN = 2;
        public static int LOSE = 3;
        public static int MOUSE_ENTER = 4;

        public static void Play(int type)
        {
            if(type == CORRECT) correctSF.Play();
            else if(type == INCORRECT) incorrectSF.Play();
            else if(type == WIN) winSF.Play();
            else if(type == LOSE) loseSF.Play();
            //else if(type ==  MOUSE_ENTER) hovingSF.Play();
        }
    }
}
