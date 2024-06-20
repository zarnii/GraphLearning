using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.Model;

namespace GraphAppTest
{
    [TestClass]
    public class LimitedStackTest
    {
        [TestMethod]
        [DataRow(new int[3] { 1, 2, 3 }, 10)]
        [DataRow(new int[7] { 1, 2, 3, 4, 5, 6, 7 }, 10)]
        [DataRow(new int[1] { 1 }, 10)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 10)]
        public void LimitedStack_PushCount_Success(int[] numbers, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            Assert.AreEqual(numbers.Length, stack.Count);
        }

        [TestMethod]
        [DataRow(new int[3] { 1, 2, 3 }, 10)]
        [DataRow(new int[7] { 1, 2, 3, 4, 5, 6, 7 }, 10)]
        [DataRow(new int[1] { 1 }, 10)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 10)]
        public void LimitedStack_Push_Success(int[] numbers, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            var stackArray = stack.ToArray();

            for (var i = 0; i < numbers.Length; i++)
            {
                Assert.AreEqual(numbers[i], stackArray[i]);
            }
        }

        [TestMethod]
        [DataRow(new int[3] { 1, 2, 3 }, 10)]
        public void LimitedStack_Pop_Success(int[] numbers, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            for (var i = numbers.Length - 1; i >= 0; i--)
            {
                var element = stack.Pop();
                Assert.AreEqual(numbers[i], element);
            }
        }

        [TestMethod]
        [DataRow(new int[3] { 1, 2, 3 }, 2)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, 5)]
        [DataRow(new int[7] { 1, 2, 3, 4, 5, 6, 7 }, 1)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, 10)]
        public void LimitedStack_PushStackOverflowCount_Success(int[] numbers, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            Assert.AreEqual(limit, stack.Count);
        }

        [TestMethod]
        [DataRow(new int[4] { 1, 2, 3, 4 }, new int[2] { 3, 4 }, 2)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, new int[5] { 6, 7, 8, 9, 0 }, 5)]
        [DataRow(new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, new int[1] { 0 }, 1)]
        public void LimitedStack_PushStackOverflow_Success(int[] numbers, int[] expected, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            for (var i = expected.Length - 1; i >= 0; i--)
            {
                Assert.AreEqual(expected[i], stack.Pop());
            }
        }

        [TestMethod]
        [DataRow(new int[5] { 1, 2, 3, 4, 5 }, new int[5] { 1, 2, 3, 4, 5 }, 5)]
        [DataRow(new int[5] { 1, 2, 3, 4, 5 }, new int[2] { 4, 5 }, 2)]
        public void LimitedStack_ToArray_Success(int[] numbers, int[] expected, int limit)
        {
            var stack = new LimitedStack<int>(limit);

            foreach (var number in numbers)
            {
                stack.Push(number);
            }

            var stackArray = stack.ToArray();

            Assert.AreEqual(expected.Length, stackArray.Length);
            
            for (var i = 0; i < stackArray.Length; i++)
            {
                Assert.AreEqual(expected[i], stackArray[i]);
            }
        }
    }
}
