using System;
using System.Collections;
using System.Collections.Generic;

namespace ReversePolishNotation
{
    public class ExpressionEnumerator : IEnumerator<byte[]>
    {
        private int _position = -1;
        private int _opPosition = 3124;
        private byte[] _data = new byte[11];
        private byte _current;

        public bool MoveNext()
        {
            _opPosition++;
            if (_opPosition == 3125)
            {
                _position++;
                _opPosition = 0;
                _data[10] = (byte) (_position % 10);
                _data[8] = (byte) (_position  / 10 % 10);
                _data[6] = (byte) (_position  / 100 % 10);
                _data[4] = (byte) (_position  / 1000 % 10);
                _data[2] = (byte) (_position  / 10000 % 10);
                _data[0] = (byte) (_position  / 100000 % 10);
            }
            
            return _position < 1000000;
        }

        public void Reset()
        {
            _position = -1;
            _opPosition = 3124;
        }

        object IEnumerator.Current => Current;


        public byte[] Current
        {
            get
            {
                _data[1] = (byte) (_opPosition % 5 + 10);
                _data[3] = (byte) (_opPosition / 5 % 5 + 10);
                _data[5] = (byte) (_opPosition / 25 % 5 + 10);
                _data[7] = (byte) (_opPosition / 125 % 5 + 10);
                _data[9] = (byte) (_opPosition / 625 % 5 + 10);
                
                return _data;
            }
        }

        public void Dispose()
        {
        }
    }
    
    public class Expression: IEnumerable
    {
        private byte[] _data;
        private char[] _charData;

        public Expression()
        {
            _data = new byte[6];
            _charData = new char[11];
        }
        
        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}