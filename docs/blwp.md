## Synopsis

These files contain transformation data of static instanced meshes in Breath of the Wild.
Each mesh is identified by a name in the string table and can have one or many
instances at various transformations.


## Data Structure

### Header

| Offset | Size | Description                                           |
|:------:|:----:|-------------------------------------------------------|
|  0x00  |  4   | String "PrOD" in ASCII (file identifier).             |
|  0x04  |  2   | Version 0x0001 in Breath of the Wild.                 |
|  0x08  |  4   | Always 1                                              |
|  0x0C  |  4   | Unknown                                               |
|  0x10  |  4   | File Size.                                            |
|  0x14  |  4   | Number of Meshes                                      |
|  0x18  |  4   | String Table Offset                                   |
|  0x1C  |  4   | Padding                                               |

### Mesh

| Offset | Size | Description                     |
|:------:|:----:|---------------------------------|
|  0x00  |  4   | Size of Instances (count * 32)  |
|  0x04  |  2   | Instances Count                 |
|  0x08  |  4   | Mesh Name (String Table Offset) |
|  0x0C  |  4   | Null Padding                    |

#### Mesh Instance

| Offset | Size | Description                     |
|:------:|:----:|---------------------------------|
|  0x00  |  12  | Position Vector (3 floats)      |
|  0x04  |  12  | Rotation (d) Vector (3 floats)  |
|  0x08  |  4   | Uniform Scale Float             |
|  0x0C  |  4   | Null Padding                    |

### Mesh Name String Table

| Offset | Size | Description                            |
|:------:|:----:|----------------------------------------|
|  0x00  |  4   | Number of strings in the string table. |
|  0x04  |  4   | Size of the string table.              |

Each string is null terminated and then padded up to the next highest 4 byte
alignment.
