using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace PoeAcolyte.Service
{
    public class WIN32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT : IEquatable<RECT>
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public override string ToString()
            {
                return "Left: " + Left.ToString() + " Top: " + Top.ToString() +
                       " Right: " + Right.ToString() + " Bottom: " + Bottom.ToString() +
                       " Width: " + Width.ToString() + " Height: " + Height.ToString();
            }


            public int Width => Right - Left;
            public int Height => Bottom - Top;


            public bool Equals(RECT other)
            {
                return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
            }

            public override bool Equals(object obj)
            {
                return obj is RECT other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Left;
                    hashCode = (hashCode * 397) ^ Top;
                    hashCode = (hashCode * 397) ^ Right;
                    hashCode = (hashCode * 397) ^ Bottom;
                    return hashCode;
                }
            }

            public static bool operator ==(RECT left, RECT right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(RECT left, RECT right)
            {
                return !left.Equals(right);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler) :
                this() // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32) (Marshal.SizeOf(typeof(WINDOWINFO)));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hWnd, ref WINDOWINFO pwi);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        
        // [DllImport("USER32.DLL")]
        // [return:MarshalAs(UnmanagedType.Bool)]
        // private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        //
        // [DllImport("USER32.DLL")]
        // [return:MarshalAs(UnmanagedType.Bool)]
        // private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    }
}