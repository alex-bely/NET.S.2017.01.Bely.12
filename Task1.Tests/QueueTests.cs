using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task1;

namespace Task1.Tests
{
    [TestFixture]
    public class QueueTests
    {
        
        private Queue<string> queue;

        
        public void CreateQueue()
        {
            queue = new Queue<string>();
            queue.Enqueue("Mercury");
            queue.Enqueue("Venus");
            queue.Enqueue("Earth");
            queue.Enqueue("Mars");
            queue.Enqueue("Jupiter");
            queue.Enqueue("Saturn");
            queue.Enqueue("Uranus");
            queue.Enqueue("Neptune");
        }

       


        [Test]
        public void GetEnumerator_WhileLoopAndForeach()
        {
            CreateQueue();
            int sum1 = 0;
            int sum2 = 0;
            var enumerator = queue.GetEnumerator();
            while(enumerator.MoveNext())
            {
                sum1 += enumerator.Current.Length;
            }
            
            foreach(var temp in queue)
            {
                sum2 += temp.Length;
            }
            Assert.AreEqual(sum1, sum2);
        }

        [Test]
        public void Dequeue_QueueOfStrings()
        {
            CreateQueue();
            Assert.AreEqual("Mercury", queue.Dequeue());
        }

        [Test]
        public void Contains_SpecifiedStringInQueueofStrings()
        {
            CreateQueue();
            Assert.AreEqual(queue.Contains("Saturn"),true);
            Assert.AreEqual(queue.Contains("Pluto"), false);
        }

        [Test]
        public void Peek_FirstElement()
        {
            CreateQueue();
            Assert.AreEqual(queue.Peek(), "Mercury");
        }

        [Test]
        public void ToArray_QueueOfStrings()
        {
            CreateQueue();
            string[] temp = { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
        
            Assert.AreEqual(queue.ToArray(), temp);
        }


    }
}
