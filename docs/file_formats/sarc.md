## Synopsis

SARC is a proprietary archive format used throughout Breath of the Wild, common
file extensions that use this format are: `.arc`, `.sarc`, `.pack` (when the
archive contains mostly other archive files), `.bars` (audio data) and
`.bgenv` (shader files). They are also sometimes compressed using [yaz0](yaz0.md)
into `.szs` files

SARC header followed by a file table, file name table, finally content is stored.

## Data Structure

### SARC Header

| Offset | Size | Description                                                                        |
|:------:|:----:|------------------------------------------------------------------------------------|
|  0x00  |  4   | String "SARC" in ASCII (file identifier).                                          |
|  0x04  |  2   | Header Length: length of this header (**0x14**)                                    |
|  0x06  |  2   | Byte order mark (BOM): 0xFE, 0xFF for big endian and 0xFF, 0xFE for little endian. |
|  0x08  |  4   | File size of the entire archive in bytes.                                          |
|  0x0C  |  4   | Beginning of data offset.                                                          |
|  0x10  |  4   | Unknown. Always 0x01000000?                                                        |

### File Table

#### SFAT Header

The SARC Header is immediately followed by the following structure.

| Offset | Size | Description                                                                        |
|:------:|:----:|------------------------------------------------------------------------------------|
|  0x00  |  4   | String "SFAT" in ASCII (identifier).                                               |
|  0x04  |  2   | Header Length: length of this header (**0x0C**)                                    |
|  0x06  |  2   | Node count.                                                                        |
|  0x08  |  4   | Hash Multiplier. Always 0x00000065.                                                |

#### Node

The SFAT Header is followed by an array of SFAT Node structures.

| Offset | Size | Description                                                                                         |
|:------:|:----:|-----------------------------------------------------------------------------------------------------|
|  0x00  |  4   | File name hash.                                                                                     |
|  0x04  |  1   | 0x00 for archives without any file name stored, 0x01 for archives with file names stored.           |
|  0x05  |  3   | File name table entry offset, relative to the end of the file name table header, divided by 4.      |
|  0x08  |  4   | Beginning of node file data, relative to the Beginning of data offset specified in the SARC header. |
|  0x0C  |  4   | End of node file data, relative to the Beginning of data offset specified in the SARC header.       |

The file name hash is calculated like so:

```c
uint GetHash(char* name, int length, uint multiplier)
{
	uint result = 0;
	for(int i = 0; i < length; i++)
		result = name[i] + result * multiplier;
	return result;
}
```

### File Name Table

The File Name Table is a list of null-terminated strings which represent the
filenames of the packed files. It is referenced by SFAT Nodes.

#### SFNT Header
The SFAT Node array is immediately followed by an 0x8 byte SFNT Header structure.

| Offset | Size | Description                                                                                         |
|:------:|:----:|----------------------------------------------------------------|
|  0x00  |  4   | String "SFNT" in ASCII (identifier).                           |
|  0x04  |  2   | Header Length: the length of this header in bytes. Always 0x8. |
|  0x06  |  2   | Unknown, always zero?                                          |

#### Strings
The SFAT Header is immediately followed by 4-byte aligned null-terminated
strings that represent the filenames of the packed files.
