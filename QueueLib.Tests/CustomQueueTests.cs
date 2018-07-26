using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QueueLib.Tests
{
    [TestFixture]
    public class CustomQueueTests
    {
        [TestCase(new int[] { 1, 2 }, ExpectedResult = new int[] { 1, 2, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4}, ExpectedResult = new int[] { 1, 2, 3, 4, 5})]
        public int[] Queue_Enqueue_IsCorrect(int[] input)
        {
            CustomQueue<int> queue = new CustomQueue<int>(input);
            queue.Enqueue(5);
            
            return queue.ToArray();
        }

        [TestCase(new char[] { 'v', 'a', 'l' }, ExpectedResult = new char[] { 'v', 'a', 'l', 'e', 'r', 'a' })]
        public char[] Queue_EnqueueRange_IsCorrect(char[] input)
        {
            CustomQueue<char> queue = new CustomQueue<char>(input);
            queue.EnqueueRange(new char[] { 'e', 'r', 'a' });

            return queue.ToArray();
        }

        [TestCase(new double[] { 1, 2, 3, 4}, ExpectedResult = new double[] { 2, 3, 4 })]
        [TestCase(new double[] { 1 }, ExpectedResult = new double[] { })]
        public double[] Queue_Dequeue_IsCorrect(double[] input)
        {
            CustomQueue<double> queue = new CustomQueue<double>(input);
            queue.Dequeue();

            return queue.ToArray();
        }

        [TestCase(new object[] { 12, " sdsd", 'c'}, ExpectedResult = new object[] { })]
        public object[] Queue_Clear_IsCorrect(object[] input)
        {
            CustomQueue<object> queue = new CustomQueue<object>(input);
            queue.Clear();

            return queue.ToArray();
        }

        [TestCase(new int[] { 1, 2, 3, 4 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 4 }, ExpectedResult = false)]
        public bool Queue_Contains_IsCorrect(int[] input)
            => new CustomQueue<int>(input).Contains(3);

        [TestCase(new char[] { 'v', 'a', 'l', 'e', 'r', 'a' }, ExpectedResult = 'v')]
        [TestCase(new char[] { 'r', 'o', 'm', 'a', 's', 'h', 'k', 'a' }, ExpectedResult = 'r')]
        public char Queue_Peek_IsCorrect(char[] input)
            => new CustomQueue<char>(input).Peek();

        [TestCase(ExpectedResult = new int[] { 1, 2, 3 })]
        public int[] Queue_CanForeach()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.EnqueueRange(new int[] { 1, 2, 3 });

            int[] actual = new int[queue.Count];
            int index = 0;
            foreach(int i in queue.ToArray())
            {
                actual[index] = i;
                index++;
            }

            return actual;
        }

        [Test]
        public void Queue_DequeueInEmptyQueue_InvalidOperationException()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void Queue_EnqueueNullElement_ArgumentNullException()
        {
            CustomQueue<object> queue = new CustomQueue<object>();
            Assert.Throws<ArgumentNullException>(() => queue.Enqueue(null));
        }

        [Test]
        public void Queue_EnqueueRangeNullCollection_ArgumentNullException()
        {
            CustomQueue<object> queue = new CustomQueue<object>();
            Assert.Throws<ArgumentNullException>(() => queue.EnqueueRange(null));
        }

        [Test]
        public void Queue_EnqueueRangeNullElementInCollection_ArgumentNullException()
        {
            CustomQueue<object> queue = new CustomQueue<object>();
            List<object> test = new List<object> { "", null };
            Assert.Throws<ArgumentNullException>(() => queue.EnqueueRange(test));
        }

        [Test]
        public void Queue_EnqueueRangeEmptyCollection_ArgumentException()
        {
            CustomQueue<object> queue = new CustomQueue<object>();
            List<object> test = new List<object> {};
            Assert.Throws<ArgumentException>(() => queue.EnqueueRange(test));
        }

        [Test]
        public void Queue_PeekInEmptyQueue_InvalidOperationException()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }
    }
}
