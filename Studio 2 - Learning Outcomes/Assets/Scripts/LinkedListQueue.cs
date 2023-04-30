using UnityEngine;

public class LinkedListQueue<T>
{
    class Node
    {
        public Node(T newData) => data = newData;
        public T data;
        public Node next;
    }

    Node first;
    Node last;
    public int queueSize;

    public LinkedListQueue() { }

    public LinkedListQueue(T[] dataSet)
    {
        for (int i = 0; i < dataSet.Length; i++)
        {
            Enqueue(dataSet[i]);
        }
    }

    public void Enqueue(T data)
    {
        Node newNode = new(data);
        if (first == null)
        {
            first = last = newNode;
        }
        else
        {
            last.next = newNode;
            last = newNode;
        }
        queueSize++;
    }

    public T Dequeue()
    {
        if (first == null)
        {
            Debug.Log("Empty queue!");
            return default(T);
        }
        Node placeholder = first;
        first = first.next;
        queueSize--;
        return placeholder.data;
    }
}
