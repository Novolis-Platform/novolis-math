using System.Collections.Concurrent;

namespace Novolis.Math.Geometry;

/// <summary>
/// Deterministic work and resource counters exposed by map controls for
/// diagnostics and feature-level tests.
/// </summary>
public sealed class MapPerformanceCounters
{
    readonly ConcurrentDictionary<MapTileKey, byte> _activeTileRequests = new();
    long _visibleTileCalculations;
    long _tileRequestsStarted;
    long _duplicateTileRequests;
    long _tileRequestsCompleted;
    long _decodedTilesCreated;
    long _decodedTilesDisposed;
    long _cacheEvictions;
    long _redrawRequests;
    long _refreshCancellations;
    long _currentDecodedTiles;
    long _peakDecodedTiles;

    /// <summary>Number of visible-key calculations performed by the view.</summary>
    public long VisibleTileCalculations => Volatile.Read(ref _visibleTileCalculations);

    /// <summary>Number of tile-source calls started by the view.</summary>
    public long TileRequestsStarted => Volatile.Read(ref _tileRequestsStarted);

    /// <summary>Number of requests started while the same key was active.</summary>
    public long DuplicateTileRequests => Volatile.Read(ref _duplicateTileRequests);

    /// <summary>Number of tile-source calls that completed.</summary>
    public long TileRequestsCompleted => Volatile.Read(ref _tileRequestsCompleted);

    /// <summary>Number of decoded images delivered to the view.</summary>
    public long DecodedTilesCreated => Volatile.Read(ref _decodedTilesCreated);

    /// <summary>Number of decoded images disposed by the view.</summary>
    public long DecodedTilesDisposed => Volatile.Read(ref _decodedTilesDisposed);

    /// <summary>Number of decoded images removed from the bounded view cache.</summary>
    public long CacheEvictions => Volatile.Read(ref _cacheEvictions);

    /// <summary>Number of redraw requests issued by the view.</summary>
    public long RedrawRequests => Volatile.Read(ref _redrawRequests);

    /// <summary>Number of refreshes canceled or superseded.</summary>
    public long RefreshCancellations => Volatile.Read(ref _refreshCancellations);

    /// <summary>Decoded images currently retained by the view.</summary>
    public long CurrentDecodedTiles => Volatile.Read(ref _currentDecodedTiles);

    /// <summary>Highest decoded-image count observed by the view.</summary>
    public long PeakDecodedTiles => Volatile.Read(ref _peakDecodedTiles);

    internal void RecordVisibleTileCalculation() =>
        Interlocked.Increment(ref _visibleTileCalculations);

    internal void RecordTileRequestStarted(MapTileKey key)
    {
        if (!_activeTileRequests.TryAdd(key, 0))
            Interlocked.Increment(ref _duplicateTileRequests);
        Interlocked.Increment(ref _tileRequestsStarted);
    }

    internal void RecordTileRequestCompleted(MapTileKey key)
    {
        _activeTileRequests.TryRemove(key, out _);
        Interlocked.Increment(ref _tileRequestsCompleted);
    }

    internal void RecordDecodedTileCreated() =>
        Interlocked.Increment(ref _decodedTilesCreated);

    internal void RecordDecodedTileStored()
    {
        var current = Interlocked.Increment(ref _currentDecodedTiles);
        while (true)
        {
            var peak = Volatile.Read(ref _peakDecodedTiles);
            if (current <= peak
                || Interlocked.CompareExchange(ref _peakDecodedTiles, current, peak) == peak)
            {
                return;
            }
        }
    }

    internal void RecordDecodedTileRemoved() =>
        Interlocked.Decrement(ref _currentDecodedTiles);

    internal void RecordDecodedTileDisposed() =>
        Interlocked.Increment(ref _decodedTilesDisposed);

    internal void RecordCacheEviction() =>
        Interlocked.Increment(ref _cacheEvictions);

    internal void RecordRedrawRequest() =>
        Interlocked.Increment(ref _redrawRequests);

    internal void RecordRefreshCancellation() =>
        Interlocked.Increment(ref _refreshCancellations);
}
