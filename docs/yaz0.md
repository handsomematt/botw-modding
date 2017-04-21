## Synopsis

Yaz0 is a run length encoding method used throughout Breath of the Wild. The
format is thoroughly understood and has a variety of tools to decompress it.

A simple tool to decompress Yaz0 files can be found in [`tools/unyaz0.cs`](tools/unyaz0.cs). (Compile using `csc unyaz0.cs`)

## Data Structure

### Header

| Offset |   Type   | Description                   |
|:------:|:--------:|-------------------------------|
|  0x00  |  char[4] | "Yaz0"                        |
|  0x04  | uint32_t | size of the uncompressed data |
|  0x08  |  char[8] | null padding                  |

### Data Groups

The complete compressed data is organized in data groups. Each data group
consists of 1 group header byte and 8 chunks:

| N |    Size   | Description                   |
|:-:|:---------:|-------------------------------|
| 1 |   1 byte  | the group header byte         |
| 8 | 1-3 bytes | 8 chunks                      |

Each bit of the group header correspondents to one chunk:
* The MSB (most significant bit, 0x80) correspondents to chunk 1
* The LSB (lowest significant bit, 0x01) correspondents to chunk 8

A set bit (=1) in the group header means, that the chunk is exact 1 byte long. This byte must be copied to the output stream 1:1. A cleared bit (=0) defines, that the chunk is 2 or 3 bytes long interpreted as a back reference to already decompressed data that must be copied.

| Size    | Data Bytes | Size Calculation                        |
|---------|------------|-----------------------------------------|
| 2 bytes | NR RR      | N = 1..f   SIZE = N+2 (=3..0x11)        |
| 3 bytes | 0R RR NN   | N= 00..ff  SIZE = N+0x12 (=0x12..0x111) |

* RRR is a value between 0x000 and 0xfff. Go back RRR+1 bytes in the output stream to find the start of the data to be copied.
* SIZE is calculated from N (see above) and declares the number of bytes to be copied.
* It is important to know, that a chunk may reference itself. For example if RRR=1 (go back 1+1=2) and SIZE=10 the previous 2 bytes are copied 10/2=5 times.

Decoding data groups and chunks is done until the end of the destination data is reached.
