## Synopsis

These files are contained within the archives of `Terrain/A/MainField` and referenced by
the [`MainField.tscb`](tscb.md). Each one contains the height map data for it's relevent
grid tile related to it's LOD.

HGHT maps are accompanied by a series of other terrain-descriptor formats. For each
HGHT map, there is an optional GRASS, WATER, and MATE map.

## Data Structure

Incredibly simple, each height map seems to be `256x256x2` (`131072`) bytes long.
This gives us 16 bits of accuracy per heightmap point, each point is encoded as
an unsigned short.
