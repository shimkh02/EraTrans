﻿using Fillter2.Parsing;
using System;
using System.Collections.Generic;

namespace 에라번역
{
    [Serializable]
    public class ChangeLog
    {
        public enum 행동
        {
            번역, 일괄번역
        }
        public string ErbName { get; }
        public int LineNum { get; }
        public string Str1 { get; }
        public string Str2 { get; }
        public 행동 했던일 { get; }
        /// <summary>
        /// 일반 번역 로그
        /// </summary>
        /// <param name="erbName"></param>
        /// <param name="lineNum"></param>
        /// <param name="원본"></param>
        /// <param name="번역본"></param>
        public ChangeLog(string erbName, int lineNum, string 원본, string 번역본) : this(erbName, lineNum, 원본, 번역본, 행동.번역)
        {

        }
        /// <summary>
        /// 일괄 번역 로그
        /// </summary>
        /// <param name="erbName"></param>
        /// <param name="원본"></param>
        /// <param name="일괄번역본"></param>
        public ChangeLog(string erbName, string 원본, string 일괄번역본) : this(erbName, -1, 원본, 일괄번역본, 행동.일괄번역)
        {

        }
        private ChangeLog(string erbName, int lineNum, string str1, string str2, 행동 했던일)
        {
            ErbName = erbName;
            LineNum = lineNum;
            Str1 = str1;
            Str2 = str2;
            this.했던일 = 했던일;
        }
        public static bool Equals(ChangeLog log1, ChangeLog log2)
        {
            if (log1.ErbName != log2.ErbName)
                return false;
            if (log1.Str1 == log2.Str1 && log1.Str2 == log2.Str2 && log1.LineNum == log2.LineNum && log1.했던일 == log2.했던일)
            {
                return true;
            }
            return false;
        }
        public static ChangeLog Redo(ChangeLog log, Dictionary<string, ErbParser> parsers)
        {
            switch (log.했던일)
            {
                case (행동.번역):
                    {
                        return Back(new ChangeLog(log.ErbName, log.LineNum, log.Str2, log.Str1), parsers);
                    }
                case (행동.일괄번역):
                    {
                        return Back(new ChangeLog(log.ErbName, log.Str2, log.Str1), parsers);
                    }
            }
            return null;
        }
        public static ChangeLog Back(ChangeLog log, Dictionary<string, ErbParser> parsers)
        {
            ChangeLog cl;
            switch (log.했던일)
            {
                case (행동.번역):
                    {
                        parsers[log.ErbName].PrintLines[log.LineNum].PrintStr = log.Str1;
                        cl = new ChangeLog(log.ErbName, log.LineNum, log.Str2, log.Str1);
                        break;
                    }
                case (행동.일괄번역):
                    {
                        var diclog = new List<Tuple<int, string>>();
                        foreach (var temp in parsers[log.ErbName].PrintLines)
                        {
                            diclog.Add(new Tuple<int, string>(temp.Key, temp.Value.PrintStr.Replace(log.Str2, log.Str1)));
                        }
                        foreach (var temp in diclog)
                        {
                            parsers[log.ErbName].PrintLines[temp.Item1] = new LineInfo(temp.Item2);
                        }
                        cl = new ChangeLog(log.ErbName, log.Str2, log.Str1);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("행동을 알수없습니다.");
                    }
            }
            return cl;
        }
    }
}
