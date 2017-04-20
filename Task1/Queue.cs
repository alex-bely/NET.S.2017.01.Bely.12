using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    public class Queue<T> : IEnumerable<T>, IEnumerable
    {
        #region Fields
        private T[] array;
        private int head;
        private int tail;
        private int size;

        private const int defaultCapacity = 4;
        #endregion

        #region Property
        /// <summary>
        /// Gets the number of elements contained in the Queue<T>
        /// </summary>
        public int Count => this.size;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Queue<T> class that is empty
        /// </summary>
        public Queue()
        {
            array = new T[0];
        }

        /// <summary>
        /// Initializes a new instance of the Queue<T> class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Queue<T> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException">Capasity value is invalid</exception>
        public Queue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Invalid capacity");
            array = new T[capacity];
            head = 0;
            tail = 0;
            size = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Queue<T> class 
        /// that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new Queue<T>.</param>
        public Queue(IEnumerable<T> collection)
        {
            if (ReferenceEquals(collection, null))
                throw new ArgumentNullException("Collection is null referenced");

            array = new T[defaultCapacity];
            size = 0;

            using (IEnumerator<T> en = collection.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    Enqueue(en.Current);
                }
            }

        }
        #endregion

        #region Public methods
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public void Clear()
        {
            if (head < tail)
                Array.Clear(array, head, size);
            else
            {
                Array.Clear(array, head, array.Length - head);
                Array.Clear(array, 0, head);
            }
            head = 0;
            tail = 0;
            size = 0;
        }

        /// <summary>
        /// Determines whether an element is in the Queue<T>.
        /// </summary>
        /// <param name="item">he object to locate in the Queue<T>. 
        /// The value can be null for reference types.</param>
        /// <returns>True if item is found in the Queue<T>; otherwise, false.</returns>
        public bool Contains(T item)
        {
            int index = this.head;
            int num2 = this.size;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            while (num2-- > 0)
            {
                if (item == null)
                {
                    if (this.array[index] == null)
                    {
                        return true;
                    }
                }
                else if ((this.array[index] != null) && comparer.Equals(this.array[index], item))
                {
                    return true;
                }
                index = (index + 1) % this.array.Length;
            }

            return false;
        }

        /// <summary>
        /// Copies the Queue<T> elements to an existing one-dimensional Array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from Queue<T>.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (ReferenceEquals(array, null))
                throw new ArgumentNullException("Array is null referenced");
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException("Invalid index");

            int arrayLength = array.Length;
            if (arrayLength - arrayIndex < size)
                throw new ArgumentException("Target array is too small");

            int numToCopy = (arrayLength - arrayIndex < size) ? (arrayLength - arrayIndex) : size;
            if (numToCopy == 0) return;

            int firstPart = (array.Length - head < numToCopy) ? array.Length - head : numToCopy;
            Array.Copy(array, head, array, arrayIndex, firstPart);
            numToCopy -= firstPart;
            if (numToCopy > 0)
            {
                Array.Copy(array, 0, array, arrayIndex + array.Length - head, numToCopy);
            }

        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the Queue<T>.</returns>
        public T Dequeue()
        {
            if (size == 0)
                throw new InvalidOperationException("Queue is empty");

            T removed = array[head];
            array[head] = default(T);
            head = (head + 1) % array.Length;
            size--;
            return removed;
        }

        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        /// <param name="item">The object to add to the Queue<T>. </param>
        public void Enqueue(T item)
        {
            if (this.size == this.array.Length)
            {
                int capacity = (int)((this.array.Length * 200L) / 100L);
                if (capacity < (this.array.Length + 4))
                {
                    capacity = this.array.Length + 4;
                }
                this.SetCapacity(capacity);
            }
            this.array[this.tail] = item;
            this.tail = (this.tail + 1) % this.array.Length;
            this.size++;
        }

        /// <summary>
        /// Returns the object at the beginning of the Queue<T> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the Queue<T>.</returns>
        public T Peek()
        {
            if (size == 0)
                throw new InvalidOperationException("Queue is empty");

            return array[head];
        }

        /// <summary>
        /// Copies the Queue<T> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the Queue<T>.</returns>
        public T[] ToArray()
        {
            T[] destinationArray = new T[this.size];
            if (this.size != 0)
            {
                if (this.head < this.tail)
                {
                    Array.Copy(this.array, this.head, destinationArray, 0, this.size);
                    return destinationArray;
                }
                Array.Copy(this.array, this.head, destinationArray, 0, this.array.Length - this.head);
                Array.Copy(this.array, 0, destinationArray, this.array.Length - this.head, this.tail);
            }
            return destinationArray;
        }

        /// <summary>
        ///Sets the capacity to the actual number of elements in the Queue<T>,
        ///if that number is less than 90 percent of current capacity
        /// </summary>
        public void TrimExcess()
        {
            int num = (int)(this.array.Length * 0.9);
            if (this.size < num)
            {
                this.SetCapacity(this.size);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Queue<T>.
        /// </summary>
        /// <returns>An Queue<T>.Enumerator for the Queue<T>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion


        #region Private members
        /// <summary>
        /// Returns an enumerator that iterates through the Queue.
        /// </summary>
        /// <returns>An Queue<T>.Enumerator for the Queue<T>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Sets specified capacity for internal array
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Queue<T> can contain.</param>
        private void SetCapacity(int capacity)
        {
            T[] destinationArray = new T[capacity];
            if (this.size > 0)
            {
                if (this.head < this.tail)
                {
                    Array.Copy(this.array, this.head, destinationArray, 0, this.size);
                }
                else
                {
                    Array.Copy(this.array, this.head, destinationArray, 0, this.array.Length - this.head);
                    Array.Copy(this.array, 0, destinationArray, this.array.Length - this.head, this.tail);
                }
            }
            this.array = destinationArray;
            this.head = 0;
            this.tail = (this.size == capacity) ? 0 : this.size;

        }

        /// <summary>
        /// Gets the element according to specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element corresponded to specified index</returns>
        private T GetElement(int index)
        {
            return array[(head + index) % array.Length];
        }

        /// <summary>
        /// Enumerates the elements of a Queue<T>.
        /// </summary>
        private struct Enumerator : IEnumerator<T>
        {
            private Queue<T> q;
            private int index;
            private T currentElement;

            /// <summary>
            /// Initializes a new instance of the Enumerator
            /// </summary>
            /// <param name="q">Collection of elemenys for iterating</param>
            internal Enumerator(Queue<T> q)
            {
                this.q = q;
                index = -1;
                currentElement = default(T);
            }

            /// <summary>
            /// Releases all resources used by the Queue<T>.Enumerator.
            /// </summary>
            public void Dispose()
            {
                index = -2;
                currentElement = default(T);
            }

            /// <summary>
            /// Advances the enumerator to the next element of the Queue<T>.
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                if (index == -2)
                    return false;

                index++;

                if (index == q.size)
                {
                    index = -2;
                    currentElement = default(T);
                    return false;
                }

                currentElement = q.GetElement(index);
                return true;
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get
                {
                    if (index < 0)
                    {
                        if (index == -1)
                            throw new InvalidOperationException(nameof(index));
                        else
                            throw new InvalidOperationException(nameof(index));
                    }
                    return currentElement;
                }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, 
            /// which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                index = -1;
                currentElement = default(T);
            }

        }

        #endregion
        
    }
}
