using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using System.Diagnostics;
using Yato.DirectXOverlay;
using System.Windows;
using System.Runtime.InteropServices;
using System.Numerics;

namespace OnScreenProtractor
{
    public partial class Form1 : Form
    {
        public enum RunType
        {
            NotRunning,
            QuickCalc,
            PrecisionCalc
        }

        List<POINT> listOfClicks = new List<POINT>();
        RunType RunStatus = RunType.NotRunning;

        bool mousePressed = false;
        bool drawRunning = false;


        public Form1()
        {
            InitializeComponent();
            ConfrimBtn.Visible = false;
            AbortBtn.Visible = false;
        }

        private void QuickMessureBtn_Click(object sender, EventArgs e)
        {
            MouseHook.Start();
            MouseHook.MouseAction += MouseHook_MouseAction;

            listOfClicks.Clear();
            AngleLabel.Text = "Please click the vertex";
            RunStatus = RunType.QuickCalc;
        }

        private void MouseHook_MouseAction(MouseClickEventArgs e)
        {
            if (RunStatus!= RunType.NotRunning && checkIfNotExists(listOfClicks, e.pt))
            {
                AngleLabel.Text = "Please click a point along the ray";
                listOfClicks.Add(e.pt);
                if (listOfClicks.Count == 3)
                {
                    MouseHook.stop();
                    if (RunStatus == RunType.QuickCalc)
                    {
                        AngleLabel.Visible = true;
                        AngleLabel.Text = calcAngleDegs(listOfClicks).ToString() + "°";

                    } else
                    {
                        PrecisionClicksDone();
                    }

                    RunStatus = RunType.NotRunning;
                }
            }

        }

        private bool checkIfNotExists(List<POINT> points, POINT pointToFind)
        {
            foreach(POINT x in points)
            {
                if (x == pointToFind)
                    return false;
            }
            return true;
        }

        private double calcAngleDegs(List<POINT> points)
        {
            Vector2 AB = new Vector2(points[1].x - points[0].x, points[1].y - points[0].y);
            Vector2 BC = new Vector2(points[2].x - points[0].x, points[2].y - points[0].y);
            AB = Vector2.Normalize(AB);
            BC = Vector2.Normalize(BC);
            float dotProduct = Vector2.Dot(AB, BC);
            //return Math.Acos(dotProduct) * (-180 / Math.PI);
            return Math.Abs((Math.Atan2(BC.Y, BC.X) - Math.Atan2(AB.Y, AB.X)) * (-180 / Math.PI));

                //return AngleFrom3PointsInDegrees(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y);


        }

        private double AngleFrom3PointsInDegrees(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double a = x2 - x1;
            double b = y2 - y1;
            double c = x3 - x2;
            double d = y3 - y2;

            double atanA = Math.Atan2(a, b);
            double atanB = Math.Atan2(c, d);

            return (atanA - atanB) * (180 / Math.PI);
            // if Second line is counterclockwise from 1st line angle is 
            // positive, else negative
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch jew = new Stopwatch();

            var overlay = new OverlayWindow(0, 0, (int)SystemInformation.VirtualScreen.Width, (int)SystemInformation.VirtualScreen.Height);

            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = true,
                Hwnd = overlay.WindowHandle,
                MeasureFps = true,
                VSync = false
            };

            var d2d = new Direct2DRenderer(rendererOptions);

            var whiteSmoke = d2d.CreateBrush(0, 0, 0, 0);

            var blackBrush = d2d.CreateBrush(0, 0, 0, 150);
            var redBrush = d2d.CreateBrush(255, 0, 0, 255);
            jew.Start();
            while (drawRunning)
            {
                d2d.BeginScene();
                d2d.ClearScene(whiteSmoke);
                int temp = findClosestEndPoint(listOfClicks, Form1.MousePosition, 10);
                if (temp != -1 && mousePressed)
                {
                    listOfClicks[temp].x = MousePosition.X;
                    listOfClicks[temp].y = MousePosition.Y;
                }
                d2d.FillCircle(listOfClicks[0].x, listOfClicks[0].y, 7, blackBrush);
                d2d.FillCircle(listOfClicks[1].x, listOfClicks[1].y, 7, blackBrush);
                d2d.FillCircle(listOfClicks[2].x, listOfClicks[2].y, 7, blackBrush);

                d2d.DrawLine(listOfClicks[0].x, listOfClicks[0].y, listOfClicks[1].x, listOfClicks[1].y, 5, redBrush);
                d2d.DrawLine(listOfClicks[0].x, listOfClicks[0].y, listOfClicks[2].x, listOfClicks[2].y, 5, redBrush);

                d2d.EndScene();
            }
            d2d.BeginScene();
            d2d.ClearScene(whiteSmoke);
            d2d.EndScene();
            jew.Stop();
            jew.Reset();
            //running = false;
        }

        private void PrecisionMessureBtn_Click(object sender, EventArgs e)
        {
            MouseHook.Start();
            MouseHook.MouseAction += MouseHook_MouseAction;

            listOfClicks.Clear();
            AngleLabel.Text = "Please click the vertex";
            RunStatus = RunType.PrecisionCalc;
        }

        private void PrecisionClicksDone()
        {
            MouseHook.MouseEvents += MouseHook_MouseEvents;
            MouseHook.Start();
            drawRunning = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerAsync();
            ConfrimBtn.Visible = true;
            AbortBtn.Visible = true;
            QuickMessureBtn.Enabled = false;
        }

        private void MouseHook_MouseEvents(MouseLeftEventChangeArgs _args)
        {
            if(_args.message == MouseMessages.WM_LBUTTONDOWN)
            {
                mousePressed = true;
            } else if (_args.message == MouseMessages.WM_LBUTTONUP)
            {
                mousePressed = false;
            }
        }

        private int findClosestEndPoint(List<POINT> listOfEndPoint, Point mousePos, int MouseRange)
        {
            for(int i = 0; i < listOfEndPoint.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(listOfEndPoint[i].x - mousePos.X, 2) + Math.Pow(listOfEndPoint[i].y - mousePos.Y, 2)) < MouseRange)
                {
                    return i;
                }
            }
            return -1;
        }

        private void ConfrimBtn_Click(object sender, EventArgs e)
        {
            MouseHook.stop();
            drawRunning = false;
            AngleLabel.Visible = true;
            AngleLabel.Text = calcAngleDegs(listOfClicks).ToString() + "°";
            listOfClicks.Clear();
            ConfrimBtn.Visible = false;
            AbortBtn.Visible = false;
            QuickMessureBtn.Enabled = true;
        }

        private void AbortBtn_Click(object sender, EventArgs e)
        {
            MouseHook.stop();
            drawRunning = false;
            listOfClicks.Clear();
            ConfrimBtn.Visible = false;
            AbortBtn.Visible = false;
            QuickMessureBtn.Enabled = true;
        }
    }

    public class MouseClickEventArgs : EventArgs
    {
        public POINT pt { get; private set; }
        public MouseClickEventArgs(POINT pnt)
        {
            pt = pnt;
        }

    }

    public class MouseLeftEventChangeArgs : EventArgs
    {
        public MouseMessages message { get; private set; }
        public MouseLeftEventChangeArgs(MouseMessages msg)
        {
            message = msg;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;

        public static POINT operator -(POINT p1, POINT p2)
        {
            return new POINT() { x = p2.x - p1.x, y = p2.y - p1.y};
        }

        private static bool equals (POINT p1, POINT p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator ==(POINT p1, POINT p2)
        {
            return equals(p1,p2);
        }

        public static bool operator !=(POINT p1, POINT p2)
        {
            return !equals(p1, p2);
        }
    }

    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    public static class MouseHook
    {


        public delegate void MyEventDelegate(MouseClickEventArgs _args);
        public delegate void MyEventDelegate1(MouseLeftEventChangeArgs _args);

        public static event MyEventDelegate MouseAction = delegate { };

        public static event MyEventDelegate1 MouseEvents = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);


        }
        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(new MouseClickEventArgs(hookStruct.pt));
            }

            if (nCode >= 0 && (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam || MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)) {
                MouseEvents(new MouseLeftEventChangeArgs((MouseMessages)wParam));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;





        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }
}
