
using System;
using System.Collections;
using System.Collections.Generic;
public class SimpleList<T>
{
    private T[] myArray;
    int currentSize;
    int capacity;
    public int Length => currentSize;
    public int Capacity => capacity;
    public SimpleList(int maxSize)
    {
        myArray = new T[maxSize];
        capacity = maxSize;
    }
    public SimpleList(int maxSize, int currentSize) : this(maxSize)
    {
        this.currentSize = currentSize;
    }
    public SimpleList(int maxSize, int currentSize, T initValue) : this(maxSize, currentSize)
    {
        for (int i = 0; i < maxSize; i++)
        {
            myArray[i] = initValue;
        }
    }
    public SimpleList(T[] array)
    {
        myArray = array;
        capacity = array.Length;
    }
    public SimpleList(int currentSize, T[] array) : this(array)
    {
        this.currentSize = currentSize;
    }
    public T this[int key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }
    T GetValue(int key)
    {
        return myArray[key];
    }
    void SetValue(int key, T value)
    {
        myArray[key] = value;
    }
    public void SetLength(int length)
    {
        currentSize = Math.Min(length, capacity);
    }
    public void Clear()
    {
        currentSize = 0;
    }
    public void Add(T item)
    {
        if (capacity > currentSize)
        {
            currentSize++;
            myArray[currentSize - 1] = item;
        }
    }
    public void RemoveAt(int id)
    {
        if (id < currentSize && id >= 0)
        {
            for (int i = id + 1; i < currentSize; i++)
            {
                myArray[i - 1] = myArray[i];
            }
            currentSize--;
        }
    }

    // public IEnumerator<T> GetEnumerator()
    // {
    //     return ((IEnumerable<T>)myArray).GetEnumerator();
    // }

    // IEnumerator IEnumerable.GetEnumerator()
    // {
    //     return myArray.GetEnumerator();
    // }
}