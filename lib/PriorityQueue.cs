using System;
using System.Collections.Generic;
using System.Linq;

namespace workspace.lib
{
    public class PriorityQueue
    {
        private int[] _heap;
        public int _currSize;

        public PriorityQueue(int capacity)
        {
            _heap = new int[capacity];
            _currSize = 0;
            Enumerable.Range(0, capacity).ToList().ForEach(n => _heap[n] = -1);
        }

        public (int, int) GetParent(int currIdx)
        {
            if (currIdx == 0)
            {
                return (-1, -1);
            }
            if (currIdx < 3)
            {
                return (0, _heap[0]);
            }
            return ((currIdx - 1) / 2, _heap[(currIdx - 1) / 2]);
        }

        public (int, int) GetLeftChild(int currIdx)
        {
            if (currIdx >= _heap.Length - 1 || 2 * currIdx + 1 >= _heap.Length) return (-1, -1);
            if (currIdx == 0)
            {
                return (1, _heap[1]);
            }
            return (2 * currIdx + 1, _heap[2 * currIdx + 1]);
        }

        public (int, int) GetRightChild(int currIdx)
        {
            if (currIdx >= _heap.Length - 2 || 2 * currIdx + 2 >= _heap.Length) return (-1, -1);
            if (currIdx == 0)
            {
                return (2, _heap[2]);
            }
            return (2 * currIdx + 2, _heap[2 * currIdx + 2]);
        }

        public void Swap(int idx1, int idx2)
        {
            var tmp = _heap[idx1];
            _heap[idx1] = _heap[idx2];
            _heap[idx2] = tmp;
        }

        public void AddNew(int value)
        {
            _heap[_currSize] = value;
            _currSize++;
            BubbleUp();
        }

        public void BubbleUp()
        {
            var currIdx = _currSize - 1;
            var currVal = _heap[currIdx];
            var parent = GetParent(currIdx);
            while (parent.Item1 != -1 && currVal != -1 && currVal < parent.Item2)
            {
                Swap(currIdx, parent.Item1);
                currIdx = parent.Item1;
                parent = GetParent(currIdx);
            }    
        }

        public int GetMin()
        {
            if (_heap.Length == 0) return -1;
            var min = _heap[0];
            _currSize--;
            BubbleDown();
            return min;
        }

        public void BubbleDown()
        {
            if (_heap.Length == 0) return;
            var currIdx = 0;
            _heap[currIdx] = _heap[_currSize];
            _heap[_currSize] = -1;
            var currVal = _heap[currIdx];
            var left = GetLeftChild(currIdx);
            var right = GetRightChild(currIdx);
            while ((left.Item2 != -1 && left.Item2 < currVal) || (right.Item2 != -1 && right.Item2 < currVal))
            {
                var indexOfMin = 
                    left.Item2 != -1 && left.Item2 < right.Item2
                    ? left.Item1
                    : (right.Item2 != -1
                    ? right.Item1
                    : left.Item1);
                Swap(currIdx, indexOfMin);
                currIdx = indexOfMin;
                left = GetLeftChild(currIdx);
                right = GetRightChild(currIdx);
            }
        }

        public void DisplayElements()
        {
            Console.WriteLine(string.Join(", ", _heap));
        }
    }
}