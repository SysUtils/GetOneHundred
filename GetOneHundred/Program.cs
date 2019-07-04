using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LevelDB;

namespace ReversePolishNotation
{
    internal class Program
    {
        private static byte[][][] GenerateOpCodes()
        {
            var result = new byte[6][][];
            for (var opCount = 0; opCount < 6; opCount++)
            {
                var chunk = new byte[(int) Math.Pow(4, opCount)][];
                for (var i = 0; i < Math.Pow(4, opCount); i++)
                {
                    var buf = new byte[opCount];
                    for (var pos = 0; pos < opCount; pos++) buf[pos] = (byte) (i / Math.Pow(4, pos) % 4);

                    chunk[i] = buf;
                }

                result[opCount] = chunk;
            }

            return result;
        }

        private static int[][] GetNumsets(byte[] numset)
        {
            return new[] {numset.Select(x => (int) x).ToArray()};
        }

        private static void TestNumber(byte[] x)
        {
            var opCodes = GenerateOpCodes();
            var enumerator = new NumsetEnumerator();
            var count = 0;
            var numsets = GetNumsets(x);
            for (var i = 0; i < numsets.Length; i++)
            {
                var l = numsets[i].Length - 1;
                for (var mask = 0; mask < enumerator.opcodeMask[l].Length; mask++)
                for (var codeId = 0; codeId < opCodes[l].Length; codeId++)
                    try
                    {
                        if (RpnUtils.IsPrime(enumerator.opcodeMask[l][mask], opCodes[l][codeId]))
                            if (Math.Abs(RpnUtils.Calculate(numsets[i], enumerator.opcodeMask[l][mask],
                                             opCodes[l][codeId]) - 100) < 0.0000000000001)
                            {
                                count++;
                                Console.WriteLine(RpnUtils.FromPolish(numsets[i], enumerator.opcodeMask[l][mask],
                                    opCodes[l][codeId]));
                            }
                    }
                    catch
                    {
                    }
            }
        }

        private static void Main(string[] args)
        {
            var opCodes = GenerateOpCodes();

            var options = new Options {CreateIfMissing = true};
            var db = new DB(options, @"result");
            var enumerator = new NumsetEnumerator();
            var data = new List<byte[]>(1000000);

            while (enumerator.MoveNext()) data.Add((byte[]) enumerator.Current.Clone());

            var total = 0;

            data.AsParallel().ForAll(x =>
            {
                var count = 0;
                var numsets = GetNumsets(x);
                for (var i = 0; i < numsets.Length; i++)
                {
                    var l = numsets[i].Length - 1;
                    for (var mask = 0; mask < enumerator.opcodeMask[l].Length; mask++)
                    for (var codeId = 0; codeId < opCodes[l].Length; codeId++)
                        try
                        {
                            if (RpnUtils.IsPrime(enumerator.opcodeMask[l][mask], opCodes[l][codeId]))
                                if (Math.Abs(RpnUtils.Calculate(numsets[i], enumerator.opcodeMask[l][mask],
                                                 opCodes[l][codeId]) - 100) < 0.0000000000001)
                                    count++;
                        }
                        catch
                        {
                            // ignored
                        }
                }

                var num = Interlocked.Increment(ref total);
                if (num % 1000 == 0)
                    Console.WriteLine(num);

                if (count > 0) db.Put(x, BitConverter.GetBytes(count));
            });
        }
    }
}