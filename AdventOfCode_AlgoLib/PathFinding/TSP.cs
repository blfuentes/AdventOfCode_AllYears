using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode_AlgoLib.PathFinding;

internal class TSP
{

    /// <summary>
    /// Travelling Salesman problem. 
    /// This algorithm helps finding the shortest path that visits each node only once.
    /// </summary>
    /// <returns>Tuple containing minimum tour cost and the tour path</returns>
    List<int> Nodes = [];
    int[,] Distances;
    private (int minCost, List<int> tour) HeldKarpRoute()
    {
        int numOfNodes = Nodes.Count;
        int subSetCount = 1 << numOfNodes;
        const int INFINITY = int.MaxValue / 4;

        int[,] dp = new int[subSetCount, numOfNodes];
        int[,] parents = new int[subSetCount, numOfNodes];

        for (int i = 0; i < subSetCount; i++)
        {
            for (int j = 0; j < numOfNodes; j++)
            {
                dp[i, j] = INFINITY;
                parents[i, j] = -1;
            }
        }

        for (int i = 0; i < numOfNodes; i++)
        {
            dp[1 << i, i] = 0;
        }

        for (int mask = 1; mask < subSetCount; mask++)
        {
            for (int j = 0; j < numOfNodes; j++)
            {
                if ((mask & (1 << j)) == 0)
                    continue;

                int previousMask = mask ^ (1 << j);

                if (previousMask == 0)
                    continue;

                for (int k = 0; k < numOfNodes; k++)
                {
                    if ((previousMask & (1 << k)) == 0)
                        continue;

                    int cost = dp[previousMask, k] + Distances[k, j];

                    if (cost < dp[mask, j])
                    {
                        dp[mask, j] = cost;
                        parents[mask, j] = k;
                    }
                }
            }
        }

        int fullMask = subSetCount - 1;
        int minCost = INFINITY;
        int lastCity = 0;
        int firstCity = 0;

        for (int j = 0; j < numOfNodes; j++)
        {
            int cost = dp[fullMask, j];
            if (cost < minCost)
            {
                minCost = cost;
                lastCity = j;
            }
        }

        List<int> tour = [];
        int currentMask = fullMask;
        int currentCity = lastCity;

        while (currentMask != 0)
        {
            tour.Add(currentCity);
            int prevCity = parents[currentMask, currentCity];

            if (prevCity == -1)
            {
                firstCity = currentCity;
                break;
            }

            currentMask ^= (1 << currentCity);
            currentCity = prevCity;
        }

        tour.Reverse();

        return (minCost, tour);
    }

    /// <summary>
    /// Held-Karp algorithm starting from a specific node.
    /// Finds the shortest Hamiltonian path starting from initNode.
    /// </summary>
    /// <param name="initNode">The node index to start the path from</param>
    /// <returns>Tuple containing minimum tour cost and the tour path</returns>
    private (int minCost, List<int> tour) HeldKarpRoute(int initNode)
    {
        int numOfNodes = Nodes.Count;
        int subSetCount = 1 << numOfNodes;
        const int INFINITY = int.MaxValue / 4;

        int[,] dp = new int[subSetCount, numOfNodes];
        int[,] parents = new int[subSetCount, numOfNodes];

        for (int i = 0; i < subSetCount; i++)
        {
            for (int j = 0; j < numOfNodes; j++)
            {
                dp[i, j] = INFINITY;
                parents[i, j] = -1;
            }
        }

        dp[1 << initNode, initNode] = 0;

        for (int mask = 1; mask < subSetCount; mask++)
        {
            if ((mask & (1 << initNode)) == 0)
                continue;

            for (int j = 0; j < numOfNodes; j++)
            {
                if ((mask & (1 << j)) == 0)
                    continue;

                int previousMask = mask ^ (1 << j);

                if (previousMask == 0)
                    continue;

                for (int k = 0; k < numOfNodes; k++)
                {
                    if ((previousMask & (1 << k)) == 0)
                        continue;

                    int cost = dp[previousMask, k] + Distances[k, j];

                    if (cost < dp[mask, j])
                    {
                        dp[mask, j] = cost;
                        parents[mask, j] = k;
                    }
                }
            }
        }

        int fullMask = subSetCount - 1;
        int minCost = INFINITY;
        int lastCity = initNode;

        for (int j = 0; j < numOfNodes; j++)
        {
            if (j == initNode) continue; // Don't end where we started (for path, not cycle)
            
            int cost = dp[fullMask, j];
            if (cost < minCost)
            {
                minCost = cost;
                lastCity = j;
            }
        }

        List<int> tour = [];
        int currentMask = fullMask;
        int currentCity = lastCity;

        while (currentMask != 0)
        {
            tour.Add(currentCity);
            int prevCity = parents[currentMask, currentCity];

            if (prevCity == -1)
            {
                break;
            }

            currentMask ^= (1 << currentCity);
            currentCity = prevCity;
        }

        tour.Reverse();

        return (minCost, tour);
    }
}
