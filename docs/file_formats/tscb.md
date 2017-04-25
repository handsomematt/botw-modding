## Synopsis

TSCB is only used once throughout the entire game: `Terrain/A/MainField.tscb`

It is some sort of master file for all files in the MainField folder, containing
a list of every "section" of the map inside.

## Data Structure

### TSCB Header

| Offset | Size | Description                               |
|:------:|:----:|-------------------------------------------|
|  0x00  |  4   | String "TSCB" in ASCII (file identifier). |
|  0x04  |  4   | Unknown (0x0A000000)                      |
|  0x08  |  4   | Unknown (1)                               |
|  0x0C  |  4   | String table offset.                      |
|  0x10  |  4   | Unknown float. (500.0f)                   |
|  0x10  |  4   | Unknown float (800.0f)                    |
|  0x14  |  4   | Material lookup table count (88)          |
|  0x18  |  4   | Tile table count (9033)                   |
|  0x1C  |  4   | Unknown (0)                               |
|  0x20  |  4   | Unknown (0)                               |
|  0x24  |  4   | Unknown float (32.0f)                     |
|  0x28  |  4   | Unknown (8)                               |
|  0x2C  |  4   | Size of Material Lookup Table (2116)      |

### Material Lookup Table

Immediately after the TSCB header are 88 `uint32`s, as described by the count in
the header. Each `uint32` represents an absolute offset to the applicable material
lookup struct below.

| Offset | Size | Description                                    |
|:------:|:----:|------------------------------------------------|
|  0x00  |  4   | `uint32` Index. Goes from 0 to 88 numerically. |
|  0x04  |  4   | `float` Red?                                   |
|  0x08  |  4   | `float` Green?                                 |
|  0x0C  |  4   | `float` Blue?                                  |
|  0x10  |  4   | `float` Alpha?                                 |

This table is possibly used by the terrain MATE format, the red channel could
specify the material index as it tends to obey values between 0-88.

### Tile Table

Immediately after the Material Lookup Table ( Header + Size Of Material Lookup
Table) is the Tile Table. 9033 entries describing tiles of the map each with
additional file descriptors in the MainField folder.

| Offset | Size | Description                                                         |
|:------:|:----:|---------------------------------------------------------------------|
|  0x00  |  4   | `float centerX`                                                     |
|  0x04  |  4   | `float centerY`                                                     |
|  0x08  |  4   | `float edgeLength`                                                  |
|  0x0C  |  4   | `float unk3`                                                        |
|  0x10  |  4   | `float unk4`                                                        |
|  0x14  |  4   | `float unk5`                                                        |
|  0x18  |  4   | `float unk6`                                                        |
|  0x1C  |  4   | `uint32 unk7` (always 0, 1 or 2)                                    |
|  0x20  |  4   | `uint32 stringOffset` offset to the tile "name" in the string table |
|  0x24  |  4   | `uint32 unk9` Always 0.                                             |
|  0x28  |  4   | `uint32 unk10` Always 0.                                            |
|  0x2C  |  4   | `uint32 unk11` ( always seems to be 0 or 4)                         |

Furthermore, **if `unk7` is NOT 0** there will be a further `uint32` at `0x30`
describing a number of further `uint32`s in the structure.
