using System;
using System.Linq;
using System.Threading;
using LevelDB;

namespace ReversePolishNotation
{
    class Program
    {
        static byte[] GetNumset(ref byte[] source)
        {
            return source.Where((v, i) => i % 2 == 0).ToArray();
        }

        static void Main(string[] args)
        {
            var options = new Options { CreateIfMissing = true };
            var writeOptions = new WriteOptions { };
            var db = new DB(options, @"tempdb");
            var iter = db.GetEnumerator();
            var count = 0;
            var currentNumset = new byte[6];
            var calculator = new RpnCalculator();
            var enumerator = new ExpressionEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    var data = enumerator.Current;
                    if (calculator.Calculate(ref data) == 100)
                    {
                        
                        var set = GetNumset(ref data);
                        if (set.SequenceEqual(currentNumset))
                        {
                            count++;
                        } else
                        {
                            if (count != 0)
                            {
                                db.Put(currentNumset, BitConverter.GetBytes(count));
                                Console.WriteLine($"{string.Join(",", currentNumset.Select(p=>p.ToString()).ToArray())}: {count}");
                            }
                            count = 1;
                            currentNumset = set;
                            
                        }
                        
                    }
                }
                catch
                {
                }
            }
        }
    }
}