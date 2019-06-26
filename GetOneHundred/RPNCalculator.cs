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
            Mul = 0,
            Div = 1,
            Add = 2,
            Sub = 3,
        }

        public double Calculate(int[] numset, byte[] mask, byte[] opcodes)
        {
            var numStack = new Stack<double>();
            var accum = 0;
            var flag = false;
            var codeId = 0;
            for (var i = 0; i < numset.Length; i++)
            {
               numStack.Push(numset[i]);
               if (i - 1 < 0)
                   continue;
               for (var t = 0; t < mask[i - 1]; t++)
               {
                   var num2 = numStack.Pop();
                   var num1 = numStack.Pop();
                   switch ((OpCode)opcodes[codeId])
                   {
                       case OpCode.Mul:
                           numStack.Push(num1 * num2);
                           break;
                       case OpCode.Div:
                           if (num2 == 0)
                           {
                               return -0;
                           }
                           numStack.Push(num1 / num2);
                           break;
                       case OpCode.Add:
                           numStack.Push(num1 + num2);
                           break;
                       case OpCode.Sub:
                           numStack.Push(num1 - num2);
                           break;
                   }
                   codeId++;
               }
            }

            var res = numStack.Pop();
            return res;
        }
    }
}