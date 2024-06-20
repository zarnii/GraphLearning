using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphApp.Model
{
    /// <summary>
    /// Стэк с константной емкостью. 
    /// Если добавляется новый элемент в коллекцию, а стэк уже заполнен, то
    /// элемент, лежащий внизу стэка, удаляется.
    /// </summary>
    /// <typeparam name="TObject">Тип элементов.</typeparam>
    public class LimitedStack<TObject>: IEnumerable<TObject>
    {
        /// <summary>
        /// Стэк.
        /// </summary>
        private List<TObject> _stack;

        /// <summary>
        /// Индекс верхнего элемента стека.
        /// </summary>
        private int _topElementIndex
        {
            get
            {
                return _stack.Count - 1;
            }
        }

        /// <summary>
        /// Лимит элементов.
        /// </summary>
        private readonly int Limit = 0;

        /// <summary>
        /// Количество элементов.
        /// </summary>
        public int Count
        {
            get
            {
                return _stack.Count;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="limit">Лимит элементов.</param>
        /// <exception cref="ArgumentOutOfRangeException">Лимит < 0.</exception>
        public LimitedStack(int limit)
        {
            if (limit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "Лимит элементов не может быть меньше нуля.");
            }

            _stack = new List<TObject>(limit);
            Limit = limit;
        }

        /// <summary>
        /// Получение верхний элемент и удалить его из коллекции.
        /// </summary>
        /// <returns>Верхний элемент стека.</returns>
        public TObject Pop()
        {
            var element = _stack[_topElementIndex];
            _stack.RemoveAt(_topElementIndex);

            return element;
        }

        /// <summary>
        /// Добавление новый элемент на верхушку стэка.
        /// </summary>
        /// <param name="obj">Элемент.</param>
        public void Push(TObject obj)
        {
            if (Count == Limit)
            {
                _stack.RemoveAt(0);
            }

            _stack.Add(obj);
        }

        /// <summary>
        /// Получение верхний элемент стека без его удаления из коллекции.
        /// </summary>
        /// <returns>Верхний элемент стека.</returns>
        public TObject Peek()
        {
            return _stack[_topElementIndex];
        }

        /// <summary>
        /// Очистка стека.
        /// </summary>
        public void Clear()
        {
            _stack.Clear();
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
