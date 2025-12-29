using System;
using System.Collections.Generic;
using UnityEngine;
//Implements Dijkstras algorithm to find shortest path between 2 hex tiles based on Terrain Movement Cost
public class PathFinder
{
    //Class implements Dijkastra's algorithm to find the Shortest path between 2 hex tiles
    //Adjacency list is the GetNeighbors() function
    protected class SearchNode : IComparable<SearchNode>
    {
        public HexTile Tile;
        public int Cost;
        public SearchNode Pred;

        public SearchNode(HexTile tile)
        {
            this.Tile = tile;
            this.Cost = 0;
            this.Pred = null;
        }

        public SearchNode(SearchNode pred, HexTile nextHexTile)
        {
            this.Tile = nextHexTile;
            this.Cost = pred.Cost + nextHexTile.Terrain.MovementCost;
            this.Pred = pred;
        }

        public int CompareTo(SearchNode other)
        {
            return this.Cost.CompareTo(other.Cost);
        }
    }

    protected SearchNode ComputeShortestPath(HexTile start, HexTile end)
    {
        if (start == null || end == null)
        {
            throw new System.Exception("Nodes cannot be null");
        }
        //TODO: Convert into a min heap
        MinHeap<SearchNode> openList = new MinHeap<SearchNode>();
        HashSet<HexTile> visited = new HashSet<HexTile>();

        openList.Push(new SearchNode(start));
        while (openList.Count > 0)
        {
            SearchNode current = openList.Pop();
            if (visited.Contains(current.Tile)) continue;
            visited.Add(current.Tile);

            if (current.Tile == end) return current;

            foreach (HexTile neighbor in WorldGrid.Instance.GetHexNeighbors(current.Tile))
            {
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    openList.Push(new SearchNode(current, neighbor));
                }
            }
        }
        return null;
    }

    public List<HexTile> ShortestPathData(HexTile start, HexTile end)
    {
        SearchNode result = ComputeShortestPath(start, end);
        if (result == null) return new List<HexTile>();

        List<HexTile> path = new List<HexTile>();
        SearchNode current = result;

        while (current != null)
        {
            path.Add(current.Tile);
            current = current.Pred;
        }

        path.Reverse();
        return path;
    }

    public int ShortestPathCost(HexTile start, HexTile end)
    {
        SearchNode result = ComputeShortestPath(start, end);
        if (result == null) return -1;
        return result.Cost;
    }





}
