using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> where T : IComparable<T>
{
    // If a node is stored at index i:
    // Its left child is at index 2*i + 1.
    // Its right child is at index 2*i + 2.
    // The parent of a node at index i can be found at index [(i-1)/2].
    private readonly List<T> _elements = new List<T>();


    public int Count => _elements.Count;

    public void Push(T item)
    {
        _elements.Add(item);
        BubbleUp(_elements.Count - 1);
    }

    private void BubbleUp(int index)
    {
        // While not the root
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            // If the item is smaller than its parent, swap!
            if (_elements[index].CompareTo(_elements[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                index = parentIndex; // Move pointer up
            }
            else
            {
                break; // Order is correct
            }
        }
    }
    public T Peek()
    {
        return _elements[0];
    }

    public T Pop()
    {
        if (_elements.Count == 0) throw new InvalidOperationException("Heap is empty");

        // The smallest item is at the root
        T min = _elements[0];

        // Move the last item to the root
        int lastIndex = _elements.Count - 1;
        _elements[0] = _elements[lastIndex];
        _elements.RemoveAt(lastIndex);

        if (_elements.Count > 0)
        {
            SinkDown(0);
        }

        return min;
    }

    private void SinkDown(int index)
    {
        int lastIndex = _elements.Count - 1;

        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            // Compare with Left Child
            if (leftChild <= lastIndex &&
                _elements[leftChild].CompareTo(_elements[smallest]) < 0)
            {
                smallest = leftChild;
            }

            // Compare with Right Child
            if (rightChild <= lastIndex &&
                _elements[rightChild].CompareTo(_elements[smallest]) < 0)
            {
                smallest = rightChild;
            }

            // If the smallest is still the current index, we are done
            if (smallest == index) break;

            Swap(index, smallest);
            index = smallest; // Move pointer down
        }
    }

    private void Swap(int indexA, int indexB)
    {
        T temp = _elements[indexA];
        _elements[indexA] = _elements[indexB];
        _elements[indexB] = temp;
    }


}
