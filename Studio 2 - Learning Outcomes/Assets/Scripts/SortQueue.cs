public class SortQueue
{
    public static int[] SortAscending(int[] values)
    {
        bool added;
        int[] result = new int[values.Length];
        if (result.Length == 0 ) { return result; }
        LinkedListQueue<int> linkedListQueue = new(values);
        result[0] = linkedListQueue.Dequeue();
        added = false;
        for(int currentPositionInArray = 0; currentPositionInArray < result.Length; currentPositionInArray++)
        {
            // Stop the loop when the queue is empty
            if (linkedListQueue.queueSize == 0)
            {
                break;
            }
            // Get next element
            int newEntry = linkedListQueue.Dequeue();
            // loop through the new array to find the position where the element should be placed
            for (int positionInAddedElements = 0; positionInAddedElements <= currentPositionInArray; positionInAddedElements++)
            {
                if (result[positionInAddedElements] < newEntry)
                {
                    continue;
                }
                else
                {
                    // insert the element where it needs to be added by moving the elements up by one
                    for (int k = currentPositionInArray; k >= positionInAddedElements; k--)
                    {
                        result[k + 1] = result[k];
                    }
                    result[positionInAddedElements] = newEntry;
                    added = true;
                    break;
                }
            }
            // if no element was added to the array, add it at the end
            if (!added)
            {
                result[currentPositionInArray + 1] = newEntry;
            }
            added = false;
        }
        return result;
    }

    public static int[] SortDescending(int[] values)
    {
        bool added;
        int[] result = new int[values.Length];
        if (result.Length == 0) { return result; }
        LinkedListQueue<int> linkedListQueue = new(values);
        result[result.Length - 1] = linkedListQueue.Dequeue();
        added = false;
        for (int currentPositionInArray = result.Length - 1; currentPositionInArray >= 0; currentPositionInArray--)
        {
            if (linkedListQueue.queueSize == 0)
            {
                break;
            }
            int num = linkedListQueue.Dequeue();
            for (int positionInAddedElements = result.Length - 1; positionInAddedElements >= currentPositionInArray; positionInAddedElements--)
            {
                if (result[positionInAddedElements] < num)
                {
                    continue;
                }
                else
                {
                    for (int k = currentPositionInArray; k <= positionInAddedElements; k++)
                    {
                        result[k - 1] = result[k];
                    }
                    result[positionInAddedElements] = num;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                result[currentPositionInArray - 1] = num;
            }
            added = false;
        }
        return result;
    }
}


