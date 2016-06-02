﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fillter
{
    public class Trans
    {
        [DllImport("Translate\\EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public extern static int Init(string eztransPath);
        [DllImport("Translate\\EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        extern static void Terminate();
        [DllImport("Translate\\EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        extern static string Translate(string str);
        public static void Destory() => Terminate();
        public static string GetString(string str) => Translate(str);
    }
}
