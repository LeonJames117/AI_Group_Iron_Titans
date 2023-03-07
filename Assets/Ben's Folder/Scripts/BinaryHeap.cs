using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BinaryHeap<T> where T : IBinaryHeapNode<T>
{
    T[] nodes;
    public int nodeCount = 0;

    public BinaryHeap(int maxSize) 
    {
        nodes = new T[maxSize];
    }

    public void Add(T node) 
    {
        nodes[nodeCount] = node;
        node.nodeIndex = nodeCount;
        nodeCount++;
        SortUp(node);
    }

    public T Remove() 
    {
        T topNode = nodes[0];
        nodeCount--;

        nodes[0] = nodes[nodeCount];// set the top node to be the last node 
        nodes[0].nodeIndex = 0;

        SortDown(nodes[0]);
        return topNode;
    }

    public void SortUp(T node) 
    {
        T parent;

        bool smaller = true;

        while (smaller) 
        {
            if (node.nodeIndex == 0)//if the current index == the root index 
            {
                smaller = false;
            }
            else 
            {
                parent = nodes[GetParentIndex(node.nodeIndex)];

                if (node.CompareTo(parent) == 1)//if the parent's cost is higher than the current node cost 
                {
                    //SWAP
                    SwapNodes(parent, node);
                }
                else //the parents cost is lower than the current node cost
                {
                    smaller = false;
                }
            }
        }
    }

    void SortDown(T node)
    {
        T leftChild;
        T rightChild;

        bool bigger = true;

        while (bigger) 
        {
            bigger = false;
            //Pick the smaller child to try and swap with

            int leftChildIndex = GetLeftChildIndex(node.nodeIndex);
            int rightChildIndex = GetRightChildIndex(node.nodeIndex);

            if (leftChildIndex < nodeCount) 
            {
                leftChild = nodes[leftChildIndex];
                rightChild = nodes[rightChildIndex];

                if (leftChild.CompareTo(rightChild) == 1)//if the right child's cost is higher
                {
                    //consider left child
                    if (node.CompareTo(leftChild) == -1)//if the left child's cost is lower 
                    {
                        //Swap
                        bigger = true;
                        SwapNodes(node, leftChild);
                    }
                }
                else//if the left child's cost is higher
                {
                    //consider right child
                    if (node.CompareTo(rightChild) == -1)//if the right child's cost is lower 
                    {
                        //Swap
                        bigger = true;
                        SwapNodes(node, rightChild);
                    }
                }
            }
        }
    }

    void SwapNodes(T parent, T child) 
    {
        nodes[parent.nodeIndex] = child;
        nodes[child.nodeIndex] = parent;

        int parentIndex = parent.nodeIndex;//Temp index store

        parent.nodeIndex = child.nodeIndex;
        child.nodeIndex = parentIndex;
    }

    int GetParentIndex(int index) 
    {
        return (index - 1) / 2;
    }

    int GetLeftChildIndex(int index) 
    {
        return (2 * index) + 1;
    }

    int GetRightChildIndex(int index) 
    {
        return (2 * index) + 2;
    }

    public bool Contains(T node) 
    {
        return Equals(nodes[node.nodeIndex], node);
    }
}

public interface IBinaryHeapNode<T> : IComparable<T> 
{
    public int nodeIndex 
    {
        get;
        set;
    }
}
