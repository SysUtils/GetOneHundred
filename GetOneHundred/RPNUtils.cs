using System.Collections.Generic;

namespace ReversePolishNotation
{
    public class RpnUtils
    {
        private static readonly string[] OpStrings =
        {
            "*",
            "/",
            "+",
            "-"
        };

        public static double Calculate(int[] numset, byte[] mask, byte[] opcodes)
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
                    switch ((OpCode) opcodes[codeId])
                    {
                        case OpCode.Mul:
                            numStack.Push(num1 * num2);
                            break;
                        case OpCode.Div:
                            if (num2 == 0) return -0; // /0 invalid operation returns != 100
                            numStack.Push(num1 / num2);
                            break;
                        case OpCode.Add:
                            numStack.Push(num1 + num2);
                            break;
                        case OpCode.Sub:
                            if (num2 == 0) return -0; // +-0 equal ops, returns != 100
                            numStack.Push(num1 - num2);
                            break;
                    }

                    codeId++;
                }
            }

            var res = numStack.Pop();
            return res;
        }

        public static string FromPolish(int[] numset, byte[] mask, byte[] opcodes)
        {
            var stack = new Stack<string>();
            var codeId = 0;
            for (var i = 0; i < numset.Length; i++)
            {
                stack.Push(numset[i].ToString());
                if (i - 1 < 0)
                    continue;
                for (var t = 0; t < mask[i - 1]; t++)
                {
                    var num2 = stack.Pop();
                    var num1 = stack.Pop();
                    stack.Push($"({num1}{OpStrings[opcodes[codeId]]}{num2})");
                    codeId++;
                }
            }

            return stack.Pop();
        }

        public static bool IsPrime(byte[] mask, byte[] opcodes)
        {
            var codeId = 0;
            var lastPriority = -1;
            for (var i = 0; i < mask.Length; i++)
                if (mask[i] > 0)
                {
                    if (lastPriority == opcodes[codeId] / 2)
                        return false;
                    lastPriority = opcodes[codeId + mask[i] - 1] / 2;
                    codeId += mask[i];
                }

            return true;
        }

        private enum OpCode
        {
            Mul = 0,
            Div = 1,
            Add = 2,
            Sub = 3
        }
    }
}