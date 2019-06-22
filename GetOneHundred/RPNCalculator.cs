using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ReversePolishNotation
{
    public class RpnCalculator
    {
        private enum OpCode
        {
            Mul = 11,
            Div = 12,
            Add = 13,
            Sub = 14,
            Invalid = 10
        }
        
        private Stack<int> numStack = new Stack<int>();
        private Stack<OpCode> opStack = new Stack<OpCode>();
        
        private OpCode ToOperation(byte s)
        {
            if (s < 10)
            {
                return OpCode.Invalid;
            }

            return (OpCode) s;
        }

        private byte Priority(OpCode op)
        {
            switch (op)
            {
               /* case OpCode.OpenBracket:
                case OpCode.CloseBracket:
                    return 0;*/

                case OpCode.Add:
                case OpCode.Sub:
                    return 1;

                case OpCode.Mul:
                case OpCode.Div:
                    return 2;
            }

            throw new InvalidOperationException();
        }

        private void ProcessOp(OpCode opCode)
        {
            var num2 = numStack.Pop();
            var num1 = numStack.Pop();
            switch (opCode)
            {
                case OpCode.Mul:
                    numStack.Push(num1 * num2);
                    break;
                case OpCode.Div:
                    numStack.Push(num1 / num2);
                    break;
                case OpCode.Add:
                    numStack.Push(num1 + num2);
                    break;
                case OpCode.Sub:
                    numStack.Push(num1 - num2);
                    break;
            }
        }

        public int Calculate(ref byte[] expression)
        {
            var accum = 0;
            var flag = false;
            foreach (var symbol in expression)
            {
                var op = ToOperation(symbol);

                if (op != OpCode.Invalid)
                {
                    if (flag)
                    {
                        numStack.Push(accum);
                        accum = 0;
                        flag = false;
                    }
/*
                    if (op == OpCode.OpenBracket)
                    {
                        opStack.Push(OpCode.OpenBracket);
                        continue;
                    }

                    if (op == OpCode.CloseBracket)
                    {
                        while (opStack.Peek() != OpCode.OpenBracket)
                            ProcessOp(ref numStack, opStack.Pop());
                        opStack.Pop();
                        continue;
                    }*/


                    var priority = Priority(op);
                    while (opStack.Any() && Priority(opStack.Peek()) > priority)
                        ProcessOp(opStack.Pop());
                    opStack.Push(op);
                }
                else if (symbol < 10)
                {
                    accum *= 10;
                    accum += symbol;
                    flag = true;
                }
            }

            if (flag)
            {
                numStack.Push(accum);
            }

            while (opStack.Any())
            {
                ProcessOp(opStack.Pop());
            }

            if (numStack.Count != 1)
                throw new InvalidDataException();
            return numStack.Pop();
        }
        
        public void ToString(IEnumerable<byte> expression)
        {
            var result = new StringBuilder();
            var accum = 0;
            var flag = false;
            foreach (var symbol in expression)
            {
                var op = ToOperation(symbol);

                if (op != OpCode.Invalid)
                {
                    switch (op)
                    {
                        case OpCode.Add:
                            result.Append("+");
                            break;
                        case OpCode.Mul:
                            result.Append("*");
                            break;
                        case OpCode.Div:
                            result.Append("/");
                            break;
                        case OpCode.Sub:
                            result.Append("-");
                            break;
                        /*case OpCode.OpenBracket:
                            result.Append("(");
                            break;
                        case OpCode.CloseBracket:
                            result.Append("(");
                            break;*/
                        case OpCode.Invalid:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (symbol < 10)
                {
                    result.Append(accum.ToString());
                    accum *= 10;
                    accum += symbol;
                    flag = true;
                }
            }
        }
    }
}