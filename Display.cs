using System.Text;

public partial class Game
{
    /// <summary>
    /// 화면을 나누어 글을 뿌린다.
    /// </summary>
    public class Display
    {
        static eWindowType currentWindow = eWindowType.Full;
        static Stack<eWindowType> windowStack = new Stack<eWindowType>();
        static StringBuilder[] SBList = new StringBuilder[(int)eWindowType.End] {
            new StringBuilder() { },
            new StringBuilder() { },
            new StringBuilder() { },
            new StringBuilder() { },
            new StringBuilder() { },
            new StringBuilder() { }};
        /// <summary>
        /// 해당 윈도우 StringBuilder에 더한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        /// <param name="cout">출력할 문자열</param>
        public static void AddSBToWindow(eWindowType window, StringBuilder cout)
        {
            SBList[(int)window].Append(cout);
        }
        /// <summary>
        /// 해당 윈도우 StringBuilder에 배열을 더한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        /// <param name="cout">출력할 문자열들</param>
        public static void AddSBToWindow(eWindowType window, StringBuilder[] cout)
        {
            foreach (StringBuilder sb in cout)
            {
                SBList[(int)window].Append(sb);
            }
        }
        /// <summary>
        /// 해당 윈도우 StringBuilder 문자열을 clear한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        public static void ClearSBWindow(eWindowType window)
        {
            SBList[(int)window].Clear();
        }
        /// <summary>
        /// 해당 윈도우에 업데이트된 정보를 출력한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        public static void UpdateWindow(eWindowType window)
        {
            ClearWindow(window);
            StringBuilder[] sbs = FixLine(window);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)window].StartX, mWindowSizes[(int)window].StartY + i);
                Console.Write(sbs[i]);
            }
        }
        /// <summary>
        /// 현재 윈도우가 아닌 모든 윈도우를 다시 업데이트된 정보로 그린다.
        /// </summary>
        public static void UpdateAllBackWindow()
        {
            Stack<eWindowType> tempWindowStack = new Stack<eWindowType>();

            while (windowStack.Count > 0)
            {
                tempWindowStack.Push(windowStack.Pop());
            }
            while (tempWindowStack.Count > 0)
            {
                var window = tempWindowStack.Pop();
                UpdateWindow(window);
                windowStack.Push(window);
            }

        }
        /// <summary>
        /// 현재 윈도우를 업데이트된 정보로 출력한다.
        /// </summary>
        public static void UpdateCurrentWindow()
        {
            ClearWindow(currentWindow);
            StringBuilder[] sbs = FixLine(currentWindow);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)currentWindow].StartX, mWindowSizes[(int)currentWindow].StartY + i);
                Console.Write(sbs[i]);
            }
        }

        /// <summary>
        /// 해당 윈도우를 공백으로 출력한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        public static void ClearWindow(eWindowType window)
        {
            // 해당 윈도우 사이즈만큼 ' '을 넣고 출력을 해버린다.
            StringBuilder[] sbs = new StringBuilder[mWindowSizes[(int)window].Height];
            for (int i = 0; i < mWindowSizes[(int)window].Height; ++i)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < mWindowSizes[(int)window].Width; ++j)
                {
                    sb.Append(' ');
                }
                sbs[i] = sb;
            }
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)window].StartX, mWindowSizes[(int)window].StartY + i);
                Console.Write(sbs[i]);
            }
        }

        struct sWindowSize
        {
            public int Width;
            public int Height;
            public int StartX;
            public int StartY;
        }
        // sWindowSize에서 사용되기에 위쪽에 있어야 적용이 된다.
        public static int ConsoleWidth = 150;
        public static int ConsoleHeight = 60;

        /// <summary>
        /// 각 윈도우에 대한 위치와 사이즈에 대한 정보.
        /// 윈도우 종류에 대해서는 eWindowType를 참고.
        /// </summary>
        private static sWindowSize[] mWindowSizes = new sWindowSize[(int)eWindowType.End] {
        // FULL
        new sWindowSize() { Width=ConsoleWidth-4, Height=ConsoleHeight-2, StartX=+2, StartY=0+1 },
        // TOP
        new sWindowSize() { Width=ConsoleWidth-4, Height=ConsoleHeight*1/3-2, StartX=+2, StartY=0+1 },
        // BOTTON
        new sWindowSize() { Width=ConsoleWidth-4, Height=ConsoleHeight*1/3-2, StartX=+2, StartY=ConsoleHeight*2/3+1 },
        // LEFT
        new sWindowSize() { Width=ConsoleWidth/2-4, Height=ConsoleHeight*2/3-2, StartX=+2, StartY=0+1 },
        // RIGHT
        new sWindowSize() { Width=ConsoleWidth/2-4, Height=ConsoleHeight*2/3-2, StartX=ConsoleWidth/2+2, StartY=0+1 },
        // CENTER
        new sWindowSize() { Width=ConsoleWidth/3-4, Height=ConsoleHeight/3-2, StartX=ConsoleWidth/3+2, StartY=ConsoleHeight/3+1 },
        };
        /* TOP은 LEFT,RIGHT랑 영역이 중복됨.
         * CENTER는 LEFT, RIGHT랑 영역이 중복됨.
         * _______________________________________________
         * |                     |                       |
         * |                    TOP                      |
         * |_____________________|_______________________|
         * |            |        |          |            |
         * |    LEFT    |        |          |   RIGHT    |
         * |            |      CENTER       |            |
         * |            |        |          |            |
         * |____________|________|__________|____________|
         * |                                             |
         * |                   BOTTON                    |
         * |_____________________________________________|
         */

        /// <summary>
        /// Width를 넘는지 계산해서 1개의 StringBuilder마다 1줄로 리턴한다. Height를 넘은 줄은 리턴하지 않는다.
        /// </summary>
        /// <param name="window">줄을 고칠 윈도우</param>
        /// <returns></returns>
        public static StringBuilder[] FixLine(eWindowType window)
        {
            int lineCount = 1;
            int charCount = 0;
            List<StringBuilder> sb = new List<StringBuilder>();
            StringBuilder currentSb = new StringBuilder();
            foreach (var c in SBList[(int)window].ToString())
            {
                if (c == '\n')
                {
                    charCount = 0;
                    sb.Add(currentSb);
                    currentSb = new StringBuilder();
                    ++lineCount;
                    continue;
                }
                if (lineCount >= mWindowSizes[(int)window].Height)
                    break;

                if (c > 'Z')
                    charCount += 2;
                else
                    charCount += 1;
                if (charCount >= mWindowSizes[(int)window].Width)
                {
                    sb.Add(currentSb);
                    currentSb = new StringBuilder();
                    charCount = c > 'z' ? 2 : 1;
                    ++lineCount;
                }
                currentSb.Append(c);
            }
            return sb.ToArray();
        }

        public static void OpenWindow(eWindowType window)
        {
            if (window == currentWindow)
                UpdateCurrentWindow();
            StringBuilder[] sbs = FixLine(window);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)window].StartX, mWindowSizes[(int)window].StartY + i);
                Console.Write(sbs[i]);
            }
            windowStack.Push(currentWindow);
            currentWindow = window;
        }

        public static void CloseCurrentWindow()
        {
            Console.Clear();
            currentWindow = windowStack.Pop();
            UpdateAllBackWindow();
            UpdateCurrentWindow();
        }

        public static StringBuilder SBWithCustomColor(string cout, int foreColor = 7, int backColor = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\x1b[38;5;" + foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
            sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
            return sb;
        }

        public static StringBuilder SBWithCustomColor(string cout, eColorType foreColor, int backColor = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\x1b[38;5;" + (int)foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
            sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
            return sb;
        }

        public enum eImageType
        {
            Warrior,
            Goblin,
            Dragon
        }
        public static void DrawImage(eWindowType window, eImageType image)
        {
            SBList[(int)window].Append(Images[(int)image]);
        }

        public static string[] Images = { "               ..                   \n            .~:!-                   \n          !=!;~:~:                  \n           *!=:$~=                  \n           ,*~...                   \n   ,,~~~-*;!:*$$=!!~~-~~.           \n   :~==$**===!!;~~=$*!~-,           \n     ;@@@@$*!;!;;-:=#.              \n    ,$#$$$$**!;;;~:***!      .:,    \n    =#$$:=$=**!;;:*#$===;   ***~    \n  .#$==- ~$$==*::! ,!******!==*-    \n  -*=#,  .$$$!:~:;   .,,,,,,,,,     \n .**!.   ,$==!~~;!,       ..,:~,... \n -!*;  ,*###@$**=#$-         !;.    \n *=$*  ;#@$*!$###=;:.        !:.    \n $$;!   #$**;;;$*;:.         !;.    \n -*     ***!;, ~*;:.         !;.    \n       =#**;;, ##!:.         !;.    \n       =#=*!.  .@##-         !;.    \n       =#*;~   .@$:.         !;.    \n     *@@=:     @#;.          !;.    \n     *@@$-     @@*~          !;.    \n     :$**      @@!-          !;.    \n     -**.      -#;,          !;.    \n     ~$-        -#,          !;.    \n     ;!;.       :!~.        .!;.    \n     :::~       ~~~~---      :~.    ",
            " _____         _      _  _        \n|  __ \\       | |    | |(_)       \n| |  \\/  ___  | |__  | | _  _ __  \n| | __  / _ \\ | '_ \\ | || || '_ \\ \n| |_\\ \\| (_) || |_) || || || | | |\n \\____/ \\___/ |_.__/ |_||_||_| |_|",
            "______                                   \n|  _  \\                                  \n| | | | _ __   __ _   __ _   ___   _ __  \n| | | || '__| / _` | / _` | / _ \\ | '_ \\ \n| |/ / | |   | (_| || (_| || (_) || | | |\n|___/  |_|    \\__,_| \\__, | \\___/ |_| |_|\n                      __/ |              \n                     |___/               "
        };
    }
}