using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Computes "frontline edges": borders between neighboring tiles owned by different countries.
/// Stores each border exactly once using EdgeKey in a HashSet.
/// </summary>
public sealed class FrontlineManager : MonoBehaviour
{
    public static FrontlineManager Instance { get; private set; }

    /// <summary>
    /// Unique identifier for an edge between two neighboring tiles, order-independent.
    /// Uses coordinates so (A,B) == (B,A).
    /// </summary>
    public readonly struct EdgeKey : IEquatable<EdgeKey>
    {
        public readonly int ax, ay, bx, by;

        public EdgeKey(HexTile a, HexTile b)
        {
            // Canonical ordering so the same border is represented once
            bool aFirst = a.X < b.X || (a.X == b.X && a.Y <= b.Y);

            HexTile first = aFirst ? a : b;
            HexTile second = aFirst ? b : a;

            ax = first.X; ay = first.Y;
            bx = second.X; by = second.Y;
        }

        public bool Equals(EdgeKey other) =>
            ax == other.ax && ay == other.ay && bx == other.bx && by == other.by;

        public override bool Equals(object obj) => obj is EdgeKey other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int h = 17;
                h = h * 31 + ax;
                h = h * 31 + ay;
                h = h * 31 + bx;
                h = h * 31 + by;
                return h;
            }
        }

        public override string ToString() => $"({ax},{ay})-({bx},{by})";
    }

    // All borders where two different countries touch
    private readonly HashSet<EdgeKey> edges = new();

    // Optional: group edges by country pair for fast querying (A vs B)
    private readonly Dictionary<(int low, int high), List<EdgeKey>> edgesByPair = new();

    public IReadOnlyCollection<EdgeKey> AllEdges => edges;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>
    /// Recompute all frontlines by iterating ONLY claimed tiles (not the whole grid).
    /// Assumes Country.ClaimedTiles is kept accurate on every claim/unclaim/spawn.
    /// </summary>
    public void RecomputeFromClaimedTiles()
    {
        edges.Clear();
        edgesByPair.Clear();

        // CountryManager.AllCountries() should return runtime countries
        foreach (Country country in CountryManager.Instance.AllCountries())
        {
            foreach (HexTile tile in country.ClaimedTiles)
            {
                if (tile == null) continue;
                if (!tile.OwnerCountryId.HasValue) continue;

                int ownerA = tile.OwnerCountryId.Value;

                HexTile[] neighbors = WorldGrid.Instance.GetHexNeighbors(tile);
                foreach (HexTile n in neighbors)
                {
                    if (n == null) continue;
                    if (!n.OwnerCountryId.HasValue) continue;

                    int ownerB = n.OwnerCountryId.Value;
                    if (ownerA == ownerB) continue;

                    var key = new EdgeKey(tile, n);

                    if (edges.Add(key))
                    {
                        int low = Mathf.Min(ownerA, ownerB);
                        int high = Mathf.Max(ownerA, ownerB);
                        var pair = (low, high);

                        if (!edgesByPair.TryGetValue(pair, out var list))
                        {
                            list = new List<EdgeKey>();
                            edgesByPair[pair] = list;
                        }

                        list.Add(key);
                    }
                }
            }
        }

        Debug.Log($"Frontlines recomputed: {edges.Count} edges across {edgesByPair.Count} country-pairs.");
    }

    /// <summary>
    /// Get edges between two countries (order-independent).
    /// </summary>
    public IReadOnlyList<EdgeKey> GetEdgesBetween(int countryA, int countryB)
    {
        int low = Mathf.Min(countryA, countryB);
        int high = Mathf.Max(countryA, countryB);
        return edgesByPair.TryGetValue((low, high), out var list) ? list : Array.Empty<EdgeKey>();
    }

    // Optional debug rendering: draws red lines between neighboring tiles that form frontlines.
    private void OnDrawGizmos()
    {

        if (!Application.isPlaying) return;
        if (HexGridRenderer.Instance == null) return;

        Gizmos.color = Color.yellow;

        foreach (var e in edges)
        {
            // Convert EdgeKey coords back to tiles for drawing
            HexTile a = WorldGrid.Instance.Get(e.ax, e.ay);
            HexTile b = WorldGrid.Instance.Get(e.bx, e.by);
            if (a == null || b == null) continue;

            Vector3 pa = HexGridRenderer.Instance.GetWorldPosition(a) + Vector3.up * 0.2f;
            Vector3 pb = HexGridRenderer.Instance.GetWorldPosition(b) + Vector3.up * 0.2f;
            Gizmos.DrawLine(pa, pb);
        }
    }

    
}
