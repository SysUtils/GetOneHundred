﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LevelDB;

namespace ReversePolishNotation
{
    class Program
    {
        static byte[][][] GenerateOpCodes()
        {
            var result = new byte[6][][];
            for (var opCount = 0; opCount < 6; opCount++)
            {
                var chunk = new byte[(int)Math.Pow(4, opCount)][];
                for (var i = 0; i < Math.Pow(4, opCount);i++)
                {
                    var buf = new byte[opCount];
                    for (var pos = 0; pos < opCount; pos++)
                    {
                        buf[pos] = (byte)(i / Math.Pow(4, pos) % 4);
                    }

                    chunk[i] = buf;
                }

                result[opCount] = chunk;
            }

            return result;
        }
        
        static int[] BreakNumset(byte[] numset, byte[] breaks)
        {
            var result = new List<int>(numset.Length);
            var buf = 0;
            var breakId = 0;
            var pos = 0;
            var cnt = 0;
            while (breakId<breaks.Length && pos < numset.Length)
            {
                if (breaks[breakId] == pos)
                {
                    result.Add(buf);
                    buf = 0;
                    breakId++;
                } 
                buf = buf * 10 + numset[pos];
                pos++;
            }
            if (pos <= numset.Length)
            {
                for (;pos < numset.Length; pos++)
                    buf = buf * 10 + numset[pos];
                result.Add(buf);
            }

            return result.ToArray();
        }

        static int[][] GetNumsets(byte[] numset)
        {
            var result = new List<int[]>();
            var breaks = new byte[] {6,6,6,6,6};
            var breakId = 0;
            var cnt = 0;
            while (breakId < breaks.Length)
            {
                if (breakId == 0 || (breaks[breakId] - 1 > breaks[breakId - 1]))
                {
                    breaks[breakId]--;
                    for (var i = breakId + 1; i < breaks.Length; i++)
                        breaks[i] = 6;
                    breakId++;
                    cnt++;
                    result.Add(BreakNumset(numset, breaks));
                }
                else
                {
                    breakId--;
                }
            }

            return result.ToArray();
        }

        static void TestNumber(byte[] x)
        {
            var opCodes = GenerateOpCodes();
            var enumerator = new NumsetEnumerator();
            var calc = new RpnCalculator();
            var count = 0;
            var numsets = GetNumsets(x);
            for (var i = 0; i < numsets.Length; i++)
            {
                var l = numsets[i].Length - 1;
                for (int mask = 0; mask < enumerator.opcodeMask[l].Length; mask++)
                {
                    for (int codeId = 0; codeId < opCodes[l].Length; codeId++)
                    {
                        try
                        {
                            if (Math.Abs(calc.Calculate(numsets[i], enumerator.opcodeMask[l][mask], opCodes[l][codeId]) - 100) < 0.0000000000001)
                            {
                                count++;
                                Console.WriteLine(string.Join("", enumerator.opcodeMask[l][mask]));
                                Console.WriteLine(string.Join("", opCodes[l][codeId]));
                                Console.WriteLine(string.Join(",", numsets[i]));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {

         //   TestNumber(new byte[] {9,9,8,1,0,5});
         //   return;
            var opCodes = GenerateOpCodes();

            var options = new Options {CreateIfMissing = true};
            var writeOptions = new WriteOptions { };
            var db = new DB(options, @"result");
            var enumerator = new NumsetEnumerator();
            var calc = new RpnCalculator();
            var data = new List<byte[]>(1000000);
       /*     var e = db.GetEnumerator();
            while (e.MoveNext())
            {
                Console.WriteLine($"{string.Join("",e.Current.Key)}: {BitConverter.ToInt32(e.Current.Value)}");
            }*/


            while (enumerator.MoveNext())
             {
                 data.Add((byte[])enumerator.Current.Clone());
             }
    
             var total = 0;
    
             data.AsParallel().ForAll(x =>
             {
                 var count = 0;
                 var numsets = GetNumsets(x);
                 for (var i = 0; i < numsets.Length; i++)
                 {
                     var l = numsets[i].Length - 1;
                     for (int mask = 0; mask < enumerator.opcodeMask[l].Length; mask++)
                     {
                         for (int codeId = 0; codeId < opCodes[l].Length; codeId++)
                         {
                             try
                             {
                                 if (Math.Abs(calc.Calculate(numsets[i], enumerator.opcodeMask[l][mask], opCodes[l][codeId]) - 100) < 0.0000000000001)
                                 {
                                     count++;
                                 }
                             }
                             catch
                             {
                             }
                         }
                     }
                 }
    
                 
                 Console.WriteLine(Interlocked.Increment(ref total));
                 if (count > 0)
                 {
                     db.Put(x, BitConverter.GetBytes(count));
                 }
             });
        }
    }
}