## Synopsis

BYML is a binary YAML file and is used as a generic data container throughout
Breath of the Wild. Unlike other Nintendo games Breath of the Wild identifies
it's version as `0x02` being the first to do so.

BYML is mostly used in the following files: `Actor/ActorInfo.product.sbyml`,
`Event/EventInfo.sbyml` and extensively throughout `Bootup.pack` storing data
about tips, shop information, status effects, area data and more..

## Data Structure

The file is broken up into a node structure, with possible interlinking between
the nodes. Each node has a one byte format identifier. The File begins with a
header which points to up to four special nodes; the node name table node; the
string value table node; the path value node and finally the root node.

### Header

| Offset | Size | Description                                                                                                                                                    |
|:------:|:----:|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |  2   | String "BY" in ASCII (file identifier).                                                                                                                        |
|  0x02  |  2   | Version 0x0002 in Breath of the Wild.                                                                                                                          |
|  0x04  |  4   | Node name table node Offset to the node name table node, relative to start (usually 0x014). Unknown if this can ever be 0. Must be a string value node (0xc2). |
|  0x08  |  4   | String value table node Offset to the string value table node, relative to start. May be 0 if string values unused. Must be a string value node (0xc2).        |
|  0x0c  |  4   | Root node Offset to the root node, relative to start. Must be either an array node (0xc0) or a dictionary node (0xc1).                                         |

### Nodes

Every node format has a unique one byte identifier as follows. Some nodes are
considered value nodes as indicated below. The full nodes have a longer encoding
in the file, which must be four byte aligned. The order of encoding full nodes
within the file does not seem to matter.

| Identifier |          Type          | Description                                                                                          |
|:----------:|:----------------------:|------------------------------------------------------------------------------------------------------|
|    0xA0    |      Value (Index)     | String. Value is an index into the string value table.                                               |
|    0xC0    |          Full          | Array. Node is an array of nodes, typically, though not necessarily, all of the same format.         |
|    0xC1    |          Full          | Dictionary. Node is a mapping from strings in the node name table to other nodes.                    |
|    0xC2    | Full (Special Purpose) | String table. Special purpose node type only seen in the node name table and the string value table. |
|    0xD0    |          Value         | Boolean. Node is 1 or 0 representing true or false respectively.                                     |
|    0xD1    |          Value         | Integer. Node is a signed integer value.                                                             |
|    0xD2    |          Value         | Float. Node is a 32 bit floating point value.                                                        |
|    0xD3    |          Value         | Hash. Node is a 32 bit CRC hash.                                                        |

#### Value Nodes
Value nodes can only be encoded as children of other nodes. Each value node has
a direct four byte encoding. For string and path values, this encoding is simply
a four byte index into the string or path tables respectively.

#### 0xC0 - Array Node

| Offset | Size | Description                                                                 |
|:------:|:----:|-----------------------------------------------------------------------------|
|  0x00  |  1   | 0xC0 node type.                                                             |
|  0x01  |  3   | Number of entries in array.                                                 |
|  0x04  |  4   | Variable length array of node types; the type of each element in the array. |

**There then follows padding bytes up to the nearest 4 bytes.** After the
padding is the variable length array of N node values. For value nodes this is
the four byte node value. For full nodes this is an offset to the node relative
to the start of the file.

#### 0xC1 - Dictionary Node

Dictionary nodes are used to encode name value collections. The order of entries
in the dictionary does not seem to matter.

| Offset | Size | Description                      |
|:------:|:----:|----------------------------------|
|  0x00  |  1   | 0xC1 node type.                  |
|  0x01  |  3   | Number of entries in dictionary. |

What then follows is a variable length array of dictionary entries. Each entry
has the following structure.

| Offset | Size | Description                                                                                                                              |
|:------:|:----:|------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |  3   | 0xC1 node type.                                                                                                                          |
|  0x03  |  1   | The node type.                                                                                                                           |
|  0x04  |  4   | Value. For value nodes this is the four byte node value. For full nodes this is an offset to the node relative to the start of the file. |

#### 0xC2 - String Table Node

| Offset | Size | Description                                                                                                                                                    |
|:------:|:----:|-----------------------------------------------------------------------------------------------------------------------------------------------|
|  0x00  |  1   | 0xC2 node type.                                                                                                                               |
|  0x01  |  3   | Number of entries in string value table.                                                                                                      |
|  0x04  |  4   | Variable length array of offsets to each string relative to the start of the node. The last entry is an offset to the end of the last string. |

Immediately following the node header are the string values at the positions
indicated by the offsets. Despite the length being clear from the table, the
string values are null terminated, perhaps for ease of decoding. They are always
stored in alphabetical order.
