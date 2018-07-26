using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QueueLib
{
    /// <summary>
    /// Class that provides queue implementation
    /// </summary>
    /// <typeparam name="T">Queue values type</typeparam>
    public class CustomQueue <T> : IEnumerable<T>, IEnumerable
    {
        #region Constants
        private const int DEFAULT_QUEUE_CAPACITY = 4;
        #endregion

        #region Private fields
        private int _head;
        private int _tail;
        private int _capacity;

        private T[] _queueBase;
        #endregion

        #region Properties
        /// <summary>
        /// Property that represent current amount of values in queue
        /// </summary>
        public int Count { get; private set; }
        #endregion

        #region .ctors
        /// <summary>
        /// Default <see cref="CustomQueue{T}"/> ctor
        /// </summary>
        public CustomQueue()
        {
            _capacity = DEFAULT_QUEUE_CAPACITY;
            _queueBase = new T[_capacity];
            Count = 0;
        }

        /// <summary>
        /// <see cref="CustomQueue{T}"/> ctor based on <paramref name="capacity"/> value
        /// </summary>
        /// <param name="capacity">Needed capacity of queue</param>
        /// <exception cref="ArgumentException">Throw when <paramref name="capacity"/> is less than 1</exception>
        public CustomQueue(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException($"{nameof(capacity)} can't be less than 1");
            }

            _capacity = capacity;
            _queueBase = new T[_capacity];
            Count = 0;
        }

        /// <summary>
        /// <see cref="CustomQueue{T}"/> ctor based on <paramref name="collection"/> value
        /// </summary>
        /// <param name="collection">Initializer collection</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="collection"/> is empty</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="collection"/> is equal to null</exception>
        public CustomQueue(IEnumerable<T> collection)
        {
            _capacity = collection.Count();
            _queueBase = new T[_capacity];
            EnqueueRange(collection);
        }
        #endregion

        #region Public API
        /// <summary>
        /// Clear the queue values
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            Count = _head = _tail = 0;
            Array.Clear(_queueBase, 0, _queueBase.Length);
        }

        /// <summary>
        /// Check if queue contains <paramref name="item"/>
        /// </summary>
        /// <param name="item">Needed queue item</param>
        /// <returns>bool result</returns>
        public bool Contains(T item)
        {
            if (Count == 0)
            {
                return false;
            }

            return ContainsInner(item);
        }

        /// <summary>
        /// Popped first element from queue
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when <see cref="Count"/> is equal to 0</exception>
        /// <returns>popped value</returns>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException($"Can't do this operation because {Count} is equal to 0");
            }

            T value = _queueBase[_head];
            _queueBase[_head] = default;
            _head = (_head + 1) % Count;
            Count--;

            return value;
        }

        /// <summary>
        /// Insert <paramref name="value"/> into the enqueue
        /// </summary>
        /// <param name="value">inserted value</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="value"/> is equal to null</exception>
        public void Enqueue(T value) => EnqueueInner(value);

        /// <summary>
        /// Insert <paramref name="collection"/> into the enqueue
        /// </summary>
        /// <param name="collection">inserted collection</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="collection"/> is empty</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="collection"/> or one of this elements is equal to null</exception>
        public void EnqueueRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"{nameof(collection)} can't be equal to null!");
            }

            if (collection.Count() == 0)
            {
                throw new ArgumentException($"{nameof(collection)} can't be empty!");
            }

            foreach(var i in collection)
            {
                EnqueueInner(i);
            }
        }

        /// <summary>
        /// Get the first element of queue without removing it
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when <see cref="Count"/> is equal to 0</exception>
        /// <returns>first element of queue</returns>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException($"Can't do this operation when {nameof(Count)} is equal to 0!");
            }

            return this[0];
        }

        /// <summary>
        /// Return <see cref="List{T}"/> representation of queue
        /// </summary>
        /// <returns><see cref="List{T}"/> representation of queue</returns>
        public List<T> ToList()
        {
            List<T> result = new List<T>(Count);
            for (int i = 0; i < Count; i++)
            {
                result.Add(this[i]);
            }

            return result;
        }

        /// <summary>
        /// Return <see cref="T[]"/> representation of queue
        /// </summary>
        /// <returns><see cref="T[]"/> representation of queue</returns>
        public T[] ToArray()
        {
            T[] result = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = this[i];
            }

            return result;
        }
        #endregion

        #region Private methods
        private T this[int i]
        {
            get => _queueBase[(_head + i) % _queueBase.Length];
            set
            {
                _queueBase[(_head + i) % _queueBase.Length] = value;
            }
        }

        private void EnqueueInner(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(value)} can't be equal to null!");
            }

            if (Count == _capacity)
            {
                IncreaseCapacity();
            }

            _queueBase[_tail] = value;
            _tail = (_tail + 1) % _queueBase.Length;
            Count++;
        }

        private void IncreaseCapacity()
        {
            int newCapacity = _capacity * 2;
            T[] temporary = new T[newCapacity];

            for(int i = 0; i < Count; i++)
            {
                temporary[i] = this[i];
            }

            _queueBase = temporary;
            _capacity = newCapacity;
            _head = 0;
            _tail = Count == _capacity ? 0 : Count;
        }

        private bool ContainsInner(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < Count; i++)
            {
                if (comparer.Equals(this[i], item))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region IEnumerable and Enumerator
        public IEnumerator GetEnumerator()
            => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => new Enumerator(this);

        /// <summary>
        /// Structure representing enumerator of <see cref="CustomQueue{T}"/>
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            #region Fields
            private readonly CustomQueue<T> _queue;
            private int _index;
            private T _currentElement;
            #endregion

            /// <summary>
            /// ctor for enumerator of <see cref="CustomQueue{T}"/>
            /// </summary>
            /// <param name="queue">needed queue</param>
            internal Enumerator(CustomQueue<T> queue)
            {
                _queue = queue;
                _index = -1;
                _currentElement = default;
            }

            /// <summary>
            /// Property representing current element of queue
            /// </summary>
            /// <exception cref="InvalidOperationException">Throws when enumerator is finished or hasn't started yet</exception>
            public T Current
            {
                get
                {
                    switch (_index)
                    {
                        case -1:
                            {
                                throw new InvalidOperationException($"Enumerator hasn't started yet!");
                            }
                        case -2:
                            {
                                throw new InvalidOperationException($"Enumerator is finished!");
                            }
                        default:
                            return _currentElement;
                    }
                }
            }

            /// <summary>
            /// Property representing current element of queue
            /// </summary>
            /// <exception cref="InvalidOperationException">Throws when enumerator is finished or hasn't started yet</exception>
            object IEnumerator.Current
            {
                get
                {
                    switch (_index)
                    {
                        case -1:
                            {
                                throw new InvalidOperationException($"Enumerator hasn't started yet!");
                            }
                        case -2:
                            {
                                throw new InvalidOperationException($"Enumerator is finished!");
                            }
                        default:
                            return _currentElement;
                    }
                }
            }

            /// <summary>
            /// Dispose enumerator
            /// </summary>
            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

            /// <summary>
            /// Element for get current element in foreach cycle
            /// </summary>
            /// <returns>is possible to move next</returns>
            public bool MoveNext()
            {
                if (_index == -2)
                {
                    return false;
                }

                _index++;

                if (_index == _queue.Count)
                {
                    _index = -2;
                    _currentElement = default;

                    return false;
                }

                _currentElement = _queue[_index];

                return true;
            }

            /// <summary>
            /// Reset index of enumerator
            /// </summary>
            public void Reset() => _index = -1;
        }
        #endregion
    }
}