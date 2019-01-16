using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarNar
{
    public class TokenReader
    {
        public string[] Words;
        public int _pos;

        public TokenReader(string source)
        {
            var wl = new List<string>();
            bool ifn = false;
            char selected = '\0';

            var b = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                if(i < source.Length && source[i] == '/' && source[i+1] == '/')
                {
                    int x = i;
                    
                    for(; ; )
                    {


                        if(x == source.Length)
                        {
                            break;
                        }

                        if(source[x] == '\r' || source[x] == '\n')
                        {
                            break;
                        }

                        x++;
                    }
                    i = x;
                    continue;
                }

                if (((source[i] == '`' || source[i] == '\'') && !ifn) || (ifn && selected == source[i]))
                {
                    if (ifn)
                    {
                        b.Append(source[i]);

                        wl.Add(b.ToString());
                        b = new StringBuilder();
                        selected = '\0';
                    }
                    else
                    {
                        selected = source[i];
                        if (b.Length > 0)
                        {
                            wl.Add(b.ToString());
                            b = new StringBuilder();
                        }
                        b.Append(source[i]);
                    }
                    ifn = !ifn;
                }
                else
                {
                    if (ifn)
                    {
                        b.Append(source[i]);
                    }
                    else
                    {
                        if (char.IsWhiteSpace(source[i]))
                        {
                            if (b.Length > 0)
                            {
                                wl.Add(b.ToString());
                                b = new StringBuilder();
                            }
                            wl.Add(source[i].ToString());

                            continue;
                        }
                        else if (!char.IsLetterOrDigit(source[i]))
                        {
                            if (b.Length > 0)
                            {
                                wl.Add(b.ToString());
                                b = new StringBuilder();
                            }
                            wl.Add(source[i].ToString());
                        }
                        else
                        {
                            b.Append(source[i]);
                        }
                    }
                }
            }

            if (b.Length > 0)
                wl.Add(b.ToString());

            Words = wl.ToArray();
        }

        public bool MoveNext(int count = 1)
        {
            return (_pos = _pos + count) < Words.Length;
        }

        public string Current { get { return Words[_pos]; } }
        public bool CanMoveNext { get { return _pos < Words.Length; } }
        public string Next { get { return Words[_pos + 1]; } }

        public string NextNonWhite
        {
            get
            {
                for (int x = _pos + 1; x < Words.Length; x++)
                {
                    if (!string.IsNullOrWhiteSpace(Words[x]))
                        return Words[x];
                }

                return string.Empty;
            }
        }

        public decimal GetValue()
        {
            return decimal.Parse(Words[_pos]);
        }

        public bool IsNumberLiteral()
        {
            string x = Words[_pos];

            if (x.Length == 0)
                return false;
            int TotalDots = 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (!char.IsNumber(x[i]))
                {
                    if (x[i] == '.')
                    {
                        TotalDots++;
                        if (TotalDots == 1)
                            continue;
                    }
                    return false;
                }
            }

            return true;
        }

        public bool EqualTo(params string[] words)
        {
            int Total = 0;
            int index = 0;
            for (int i = _pos; i < _pos + words.Length && i < Words.Length; i++)
            {
                if (words[index].ToLower() == Words[i].ToLower())
                    Total++;
                else
                    return false;
                index++;
            }
            return Total == words.Length;
        }
    }
}
