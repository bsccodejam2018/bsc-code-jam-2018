# Algorithms

| Technique | Explanation |
|------|------------|
| **Recursive Functions** | Recursive functions are a class of functions that calls itself during execution to iteratively solve a problem. This works well with problems that can be broken down into smaller components that build on top one another to form a solution of a more complex problem. |
| **Greedy Algorithm** | A greedy algorithm is an algorithmic paradigm that follows the problem solving heuristic of making the locally optimal choice at each stage with the intent of finding a global optimum.  Greedy algorithms are optimal for problems that exhibit optimal substructure. In many problems, a greedy strategy does not usually produce an optimal solution, but nonetheless a greedy heuristic may yield locally optimal solutions that approximate a globally optimal solution in a reasonable amount of time. |
| **Dijkstra Algorithm** | Dijkstra's algorithm and its variants represent a family of algorithms applied to graphs to find the shortest path between two nodes (vertices) of interest. Dijkstra's algorithm is in fact a solution to a dynamic programming problem defined on a graph where the goal is to find the *shortest* (minimum distance travelled) path between nodes. |
| **Memoization** | Memoization refers to the caching of input parameter(s) and the output value from a function call, allowing for its use later during the program's execution.  When the same input parameters are passed in again, the cached output value is returned directly, rather than having to execute the function body in order to calculate the output.  This can have substantial performance implications when used in recursive functions or functions that are otherwise expensive to execute.  This is simply a very specific form of caching done on a function invocation level.  However, this is contingent upon the function being pure - i.e., given the same input, the function will always produce the same output. |
| **Dynamic Programming** | Dynamic programming is a cunning technique to use when straight recursion takes too long as in the Pony Express and Freeform Factory problem. If there is an overlap of subproblems, then the previously computed answers can be reused. Memoization can be used, while the recursive algorithm is executing, to cache the subproblem answers. |

# Graph Theory

| Concept | Explanation |
|------|------------|
| **Graph Theory** | Graph theory is the study of mathematical objects known as graphs, which consist of vertices (or nodes) connected by edges.  Any scenario in which one wishes to examine the structure of a network of connected objects is potentially a problem for graph theory.  |
| **Complete Graphs** | A complete graph is a graph where every vertex has an edge to every other vertex. In the Freeform Factory problem we had to complete the subgraphs to solve it. |
| **Bipartite Graphs** | A bipartite graph is a graph where the vertices are divided into two groups and there are no edges between any vertices in the same group. We used a bipartite graph in the Freeform Factory problem where one group was the machines and one group was the workers. An edge can only be between a machine and a worker.|
| **Cyclic Graph** | A graph is said to contain a cycle if at least one vertex is reachable from itself.  I.e., following the edges from a specific vertex, it is possible to arrive back at that vertex.  A graph that contains at least one cycle is referred to as a cyclic graph. |
| **Hamilton Cycles** | In graph theory, A Hamiltonian path is a path that visits every node in a graph once and once only with no route between these nodes being taken more than once. A Hamiltonian Cycle is such a path that starts and ends at the same node. |
