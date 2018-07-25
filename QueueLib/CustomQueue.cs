using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QueueLib
{
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
        public int Count { get; private set; }
        #endregion

        #region .ctors
        public CustomQueue()
        {
            _capacity = DEFAULT_QUEUE_CAPACITY;
            _queueBase = new T[_capacity];
            Count = 0;
        }

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
        #endregion

        #region Public API
        public void Enqueue(T value) => EnqueueInner(value);

        public void EnqueueRange(IEnumerable<T> assert)
        {
            if (assert == null)
            {
                throw new ArgumentNullException($"{nameof(assert)} can't be equal to null!");
            }

            if (assert.Count() == 0)
            {
                throw new ArgumentException($"{nameof(assert)} can't be empty!");
            }

            foreach(var i in assert)
            {
                EnqueueInner(i);
            }
        }

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

        public void Clear()
        {
            Count = _head = _tail = 0;
            Array.Clear(_queueBase, 0, _queueBase.Length);
        }

        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for(int i = 0; i < Count; i++)
            {
                if (comparer.Equals(this[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public List<T> ToList()
        {
            List<T> result = new List<T>(Count);
            for (int i = 0; i < Count; i++)
            {
                result.Add(_queueBase[i]);
            }

            return result;
        }

        public T[] ToArray()
        {
            T[] result = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = _queueBase[i];
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
        #endregion

        #region IEnumerable and Enumerator
        public IEnumerator GetEnumerator()
            => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            #region Fields
            private readonly CustomQueue<T> _queue;
            private int _index;
            private T _currentElement;
            #endregion

            internal Enumerator(CustomQueue<T> queue)
            {
                _queue = queue;
                _index = -1;
                _currentElement = default;
            }

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

            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

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

            public void Reset() => _index = -1;
        }
        #endregion
    }
}