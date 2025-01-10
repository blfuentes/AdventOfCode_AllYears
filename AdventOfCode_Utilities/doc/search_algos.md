# Search Algos

## Properties


|   Feature |   DFS |   BFS |A* |
|-----------|-------|-------|---|
|Exploration    |   Depth-first |Level-by-level |Cost-guided|
|Shortest Path  |   No  |Yes (unweighted)   |Yes (weighted)|
|Memory Usage   |   Low |High   |High|
|Completeness	|   No (if infinite)  |Yes  |Yes|
|Optimality |   No  |Yes (unweighted)   |Yes (with good heuristic)|

## How to Choose
### Use DFS if:

- You’re exploring paths or solving puzzles where the path length doesn’t matter.
- You have limited memory and the graph is large.

### Use BFS if:

- You need the shortest path in an unweighted graph.
- Memory usage isn’t a major constraint.

### Use A if:*

- The graph is weighted, and you want the shortest path efficiently.
- You can design or utilize a good heuristic.