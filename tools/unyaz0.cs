// unyaz0
// compile using: `csc unyaz0.cs`

using System;
using System.IO;

namespace unyaz0
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("usage: {0} <in> <out>", Environment.GetCommandLineArgs()[0]);
                Environment.Exit(1);
            }

            var input = new FileStream(args[0], FileMode.Open, FileAccess.Read);
            var output = new FileStream(args[1], FileMode.Create, FileAccess.ReadWrite);

            Decompress(input, output);
        }

        static UInt16 Reverse(UInt16 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        static UInt32 Reverse(UInt32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        static void Decompress(FileStream input, FileStream output)
        {
            using (var reader = new BinaryReader(input))
            using (var writer = new BinaryWriter(output))
            {
                if (Reverse(reader.ReadUInt32()) != 0x59617A30)
                    throw new Exception("Invalid Yaz0 header.");

                var decompressedSize = Reverse(reader.ReadUInt32());
                reader.BaseStream.Seek(8, SeekOrigin.Current);

                int decompressedBytes = 0;
                while (decompressedBytes < decompressedSize)
                {
                    var groupConfig = reader.ReadByte();
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((groupConfig & (1 << i)) == (1 << i))
                        {
                            writer.Write(reader.ReadByte());
                            decompressedBytes++;
                        }
                        else if (decompressedBytes < decompressedSize)
                        {
                            var dataBackSeekOffset = Reverse(reader.ReadUInt16());
                            int dataSize;

                            byte nibble = (byte)(dataBackSeekOffset >> 12);
                            if (nibble == 0)
                                dataSize = reader.ReadByte() + 0x12;
                            else
                            {
                                dataSize = nibble + 0x02;
                                dataBackSeekOffset &= 0x0FFF;
                            }

                            for (int j = 0; j < dataSize; j++)
                            {
                                writer.BaseStream.Seek(-dataBackSeekOffset - 1, SeekOrigin.Current);
                                byte readByte = (byte)writer.BaseStream.ReadByte();
                                writer.Seek(0, SeekOrigin.End);
                                writer.Write(readByte);
                                decompressedBytes++;
                            }
                        }
                    }
                }
            }
        }
    }
}
