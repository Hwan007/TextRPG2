using System.Text;

public partial class FourWeekHomework
{
    /// <summary>
    /// 화면을 나누어 글을 뿌린다.
    /// </summary>
    public class Display
    {
        static WindowType currentWindow = WindowType.Full;
        static Stack<WindowType> windowStack = new Stack<WindowType>();
        static StringBuilder[] SBList = new StringBuilder[(int)WindowType.End];
        /// <summary>
        /// 해당 윈도우 StringBuilder에 더한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        /// <param name="cout">출력할 문자열</param>
        public static void AddSBToWindow(WindowType window, StringBuilder cout)
        {
            SBList[(int)window].Append(cout);
        }
        /// <summary>
        /// 해당 윈도우 StringBuilder에 배열을 더한다.
        /// </summary>
        /// <param name="window">윈도우 종류</param>
        /// <param name="cout">출력할 문자열들</param>
        public static void AddSBToWindow(WindowType window, StringBuilder[] cout)
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
        public static void ClearSBWindow(WindowType window)
        {
            SBList[(int)window].Clear();
        }

        public static void UpdateWindow(WindowType window)
        {
            StringBuilder[] sbs = FixLine(window);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)window].StartX, mWindowSizes[(int)window].StartY + i);
                Console.Write(sbs[i]);
            }
        }

        public static void UpdateAllBackWindow()
        {
            Stack<WindowType> tempWindowStack = new Stack<WindowType>();

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

        public static void UpdateCurrentWindow()
        {
            StringBuilder[] sbs = FixLine(currentWindow);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)currentWindow].StartX, mWindowSizes[(int)currentWindow].StartY + i);
                Console.Write(sbs[i]);
            }
        }

        public enum WindowType
        {
            Full,
            Top,
            Bottom,
            Left,
            Right,
            Center,
            End
        }

        struct WindowSize
        {
            public int Width;
            public int Height;
            public int StartX;
            public int StartY;
        }

        private static WindowSize[] mWindowSizes = new WindowSize[(int)WindowType.End] {
        new WindowSize() { Width=80-4, Height=20-2, StartX=+2, StartY=0+1 },
        new WindowSize() { Width=80-4, Height=10-2, StartX=+2, StartY=0+1 },
        new WindowSize() { Width=80-4, Height=10-2, StartX=+2, StartY=10+1 },
        new WindowSize() { Width=40-4, Height=20-2, StartX=+2, StartY=0+1 },
        new WindowSize() { Width=40-4, Height=20-2, StartX=40+2, StartY=0+1 },
        new WindowSize() { Width=40-4, Height=10-2, StartX=20+2, StartY=5+1 },
        };

        /// <summary>
        /// Width를 넘는지 계산해서 1개의 StringBuilder마다 1줄로 리턴한다. Height를 넘은 줄은 리턴하지 않는다.
        /// </summary>
        /// <param name="window">줄을 고칠 윈도우</param>
        /// <returns></returns>
        public static StringBuilder[] FixLine(WindowType window)
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

        public static void OpenWindow(WindowType window)
        {
            StringBuilder[] sbs = FixLine(window);
            for (int i = 0; i < sbs.Length; ++i)
            {
                Console.SetCursorPosition(mWindowSizes[(int)window].StartX, mWindowSizes[(int)window].StartY+i);
                Console.Write(sbs[i]);
            }
            windowStack.Push(currentWindow);
            currentWindow = window;
        }

        public static void CloseWindow(WindowType window)
        {
            currentWindow = windowStack.Pop();
            UpdateAllBackWindow();
            UpdateCurrentWindow();
        }

        public enum ColorType
        {
            Gold = 178,
            Red = 160,
            Gray = 7,
            White = 15,
            Black = 16,
            Orange = 208,
            PurpleBlue = 75
        }

        public static StringBuilder SBWithCustomColor(string cout, int foreColor = 7, int backColor = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\x1b[38;5;" + foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
            sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
            return sb;
        }

        public static StringBuilder SBWithCustomColor(string cout, ColorType foreColor, int backColor = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\x1b[38;5;" + (int)foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
            sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
            return sb;
        }
    }
}