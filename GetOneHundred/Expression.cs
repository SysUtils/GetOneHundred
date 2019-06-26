using System;
using System.Collections;
using System.Collections.Generic;

namespace ReversePolishNotation
{
    public class NumsetEnumerator : IEnumerator<byte[]>
    {
        public byte[][][] opcodeMask = 
        {
            new byte[0][], // 1 numbers
            new[] // 2 numbers
            {
                new byte[]
                {
                    1
                }
            },
            new[] // 3 numbers
            {
                new byte[]
                {
                    0, 2
                },
                new byte[]
                {
                    1, 1
                }
            },
            new[] // 5 numbers
            {
                new byte[]
                {
                    0, 0, 0, 4
                },
                new byte[]
                {
                    0, 0, 1, 3
                },
                new byte[]
                {
                    0, 0, 2, 2
                },
                new byte[]
                {
                    0, 0, 3, 1
                },
                new byte[]
                {
                    0, 1, 0, 3
                },
                new byte[]
                {
                    0, 1, 1, 2
                },
                new byte[]
                {
                    0, 1, 2, 1
                },
                new byte[]
                {
                    0, 2, 0, 2
                },
                new byte[]
                {
                    0, 2, 1, 1
                },
                new byte[]
                {
                    1, 0, 0, 3
                },
                new byte[]
                {
                    1, 0, 1, 2
                },
                new byte[]
                {
                    1, 0, 2, 1
                },
                new byte[]
                {
                    1, 1, 0, 2
                },
                new byte[]
                {
                    1, 1, 1, 1
                },
            },
            new[] // 6 numbers
            {
                new byte[]
                {
                    0, 0, 0, 0, 5
                },
                new byte[]
                {
                    0, 0, 0, 1, 4
                },
                new byte[]
                {
                    0, 0, 0, 2, 3
                },
                new byte[]
                {
                    0, 0, 0, 3, 2
                },
                new byte[]
                {
                    0, 0, 0, 4, 1
                },
                new byte[]
                {
                    0, 0, 1, 0, 4
                },
                new byte[]
                {
                    0, 0, 1, 1, 3
                },
                new byte[]
                {
                    0, 0, 1, 2, 2
                },
                new byte[]
                {
                    0, 0, 1, 3, 1
                },
                new byte[]
                {
                    0, 0, 2, 0, 3
                },
                new byte[]
                {
                    0, 0, 2, 0, 3
                },
                new byte[]
                {
                    0, 0, 2, 1, 2
                },
                new byte[]
                {
                    0, 0, 2, 2, 1
                },
                new byte[]
                {
                    0, 0, 3, 0, 2
                },
                new byte[]
                {
                    0, 0, 3, 1, 1
                },
                new byte[]
                {
                    0, 1, 0, 0, 4
                },
                new byte[]
                {
                    0, 1, 0, 1, 3
                },
                new byte[]
                {
                    0, 1, 0, 2, 2
                },
                new byte[]
                {
                    0, 1, 0, 3, 1
                },
                new byte[]
                {
                    0, 1, 1, 0, 3
                },
                new byte[]
                {
                    0, 1, 1, 1, 2
                },
                new byte[]
                {
                    0, 1, 1, 2, 1
                },
                new byte[]
                {
                    0, 1, 2, 0, 2
                },
                new byte[]
                {
                    0, 1, 2, 1, 1
                },
                new byte[]
                {
                    0, 2, 0, 0, 3
                },
                new byte[]
                {
                    0, 2, 0, 0, 3
                },
                new byte[]
                {
                    0, 2, 0, 1, 2
                },
                new byte[]
                {
                    0, 2, 0, 2, 1
                },
                new byte[]
                {
                    0, 2, 1, 0, 2
                },
                new byte[]
                {
                    0, 2, 1, 1, 1
                },
                new byte[]
                {
                    1, 0, 0, 0, 4
                },
                new byte[]
                {
                    1, 0, 0, 1, 3
                },
                new byte[]
                {
                    1, 0, 0, 2, 2
                },
                new byte[]
                {
                    1, 0, 0, 3, 1
                },
                new byte[]
                {
                    1, 0, 1, 0, 3
                },
                new byte[]
                {
                    1, 0, 1, 1, 2
                },
                new byte[]
                {
                    1, 0, 1, 2, 1
                },
                new byte[]
                {
                    1, 0, 2, 0, 2
                },
                new byte[]
                {
                    1, 0, 2, 1, 1
                },
                new byte[]
                {
                    1, 1, 0, 0, 3
                },
                new byte[]
                {
                    1, 1, 0, 1, 2
                },
                new byte[]
                {
                    1, 0, 1, 2, 1
                },
                new byte[]
                {
                    1, 0, 2, 0, 2
                },
                new byte[]
                {
                    1, 0, 2, 1, 1
                },
                new byte[]
                {
                    1, 0, 2, 1, 1
                },
                new byte[]
                {
                    1, 1, 0, 0, 3
                },
                new byte[]
                {
                    1, 1, 0, 1, 2
                },
                new byte[]
                {
                    1, 1, 0, 2, 1
                },
                new byte[]
                {
                    1, 1, 1, 0, 2
                },
                new byte[]
                {
                    1, 1, 1, 1, 1
                }
            }
        };
        
        private int _position = -1;
        private byte[] _data = new byte[6];
        private byte _current;

        public bool MoveNext()
        {
            _position++;
            return _position < 1000000;
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current => Current;


        public byte[] Current
        {
            get
            {
                _data[0] = (byte) (_position % 10);
                _data[1] = (byte) (_position / 10 % 10);
                _data[2] = (byte) (_position / 100 % 10);
                _data[3] = (byte) (_position / 1000 % 10);
                _data[4] = (byte) (_position / 10000 %  10);
                _data[5] = (byte) (_position / 100000 %  10);
                
                return _data;
            }
        }

        public void Dispose()
        {
        }
    }
}