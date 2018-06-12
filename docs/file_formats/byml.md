## Synopsis

BYML is a binary YAML file and is used as a generic data container throughout
Breath of the Wild. Unlike other Nintendo games Breath of the Wild identifies
its version as `0x02` being the first to do so.

BYML is mostly used in the following files: `Actor/ActorInfo.product.sbyml`,
`Event/EventInfo.sbyml` and extensively throughout `Bootup.pack` storing data
about tips, shop information, status effects, area data and more.

## Data Structure

The file is broken up into a node structure, with possible interlinking between
the nodes. Each node has a one byte format identifier. The File begins with a
header which points to three special nodes; the hash key table;
the string table node and the root node.

### Header

| Offset | Size | Description                                                                                                                                                    |
|:------:|:----:|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |  2   | String "BY" (big endian) or "YB" (little endian) in ASCII (file identifier). |
|  0x02  |  2   | Version 0x0002 in Breath of the Wild. Values of 1 and 3 are accepted as well. |
|  0x04  |  4   | Offset to the hash key table, relative to start (usually 0x010). May be 0 if no hash nodes are used. Must be a string value node (0xc2). |
|  0x08  |  4   | Offset to the string table, relative to start. May be 0 if no strings are used. Must be a string value node (0xc2). |
|  0x0c  |  4   | Offset to the root node, relative to start. Must be either an array node (0xc0) or a hash node (0xc1). |

### Nodes

Every node format has a unique one byte identifier as follows. Some nodes are
considered value nodes as indicated below. The container nodes have a longer encoding
in the file, which must be four byte aligned. The order of encoding full nodes
within the file does not seem to matter.

| Identifier |          Type          | Description                                                                                          |
|:----------:|:----------------------:|------------------------------------------------------------------------------------------------------|
|    0xA0    |      Value (Index)     | String. Value is an index into the string table. |
|    0xC0    |        Container       | Array. Node is an array of nodes, typically, though not necessarily, all of the same format. |
|    0xC1    |        Container       | Hash. Node is a mapping from strings in the hash key table to other nodes. |
|    0xC2    |   Container (Special)  | String table. Special purpose node type only seen in the hash key table and the string table. |
|    0xD0    |          Value         | Bool. Node is 1 or 0 representing true or false respectively. |
|    0xD1    |          Value         | Int. Node is a signed 32 bit integer value. |
|    0xD2    |          Value         | Float. Node is a binary32 floating-point number. |
|    0xD3    |          Value         | UInt. Node is an unsigned 32 bit integer value. The game uses this for some CRC32 hashes and for masks. |
|    0xD4    |     Value (Special)    | Int64. Node is a 64 bit integer value. Not seen in Breath of the Wild. |
|    0xD5    |     Value (Special)    | UInt64. Node is an unsigned 64 bit integer value. Not seen in Breath of the Wild. |
|    0xD6    |     Value (Special)    | Double. Node is a binary64 floating-point number. Not seen in Breath of the Wild. |
|    0xFF    |          Value         | Null. Value is always 0. Not seen in Breath of the Wild. |


#### Value Nodes
Value nodes can only be encoded as children of container nodes. Each value node has
a direct four byte encoding.

For string nodes, this encoding is simply a four byte index into the string table respectively.

For special value nodes, the value is an offset to a 64 bit integer / floating-point number relative
to the start of the file.

#### 0xC0 - Array Node

| Offset     | Size | Description                                                                 |
|:----------:|:----:|-----------------------------------------------------------------------------|
|  0x00      |  1   | 0xC0 node type.                                                             |
|  0x01      |  3   | Number of entries in array.                                                 |
|  0x04      |  N   | Array of N node types; the type of each element in the array.               |
|  0x04 + N' | 4\*N | Array of N node values. For regular value nodes, this is a 4 byte node value. For other nodes, this is a 4 byte offset to the node relative to the start of the file. |

N' is N rounded up to the nearest multiple of 4.

#### 0xC1 - Hash Node

Hash / dictionary nodes are used to encode name value collections. The order of entries
in the dictionary does not seem to matter.

| Offset | Size | Description                      |
|:------:|:----:|----------------------------------|
|  0x00  |  1   | 0xC1 node type.                  |
|  0x01  |  3   | Number of entries in dictionary. |

What then follows is a variable length array of dictionary entries. Each entry
has the following structure.

| Offset | Size | Description                                                                                                                              |
|:------:|:----:|------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |  3   | Name. Value is an index in the hash key table.                                                                                                         |
|  0x03  |  1   | The node type.                                                                                                                                         |
|  0x04  |  4   | Value. For regular value nodes, this is the 4 byte node value. For other nodes, this is a 4 byte offset to the node relative to the start of the file. |

#### 0xC2 - String Table Node

| Offset | Size | Description                                                                                                                                                    |
|:------:|:----------:|-----------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |      1     | 0xC2 node type.                                                                                                                               |
|  0x01  |      3     | Number of entries in the string table.                                                                                                       |
|  0x04  | Y=4\*(N+1) | Array of N+1 offsets to each string relative to the start of the node. The last entry is an offset to the end of the last string. |
| 0x04+Y'|  variable  | Array of N null-terminated strings stored in alphabetical order. |

Y' is Y rounded up to the nearest multiple of 4.
