using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

internal static class StreamParser
{
    public static bool TrySkipLine(FileStream stream)
    {
        int b;
        while ((b = stream.ReadByte()) is not '\n' and not -1) ;
        return b is not -1;
    }

    public static bool TrySkipWhiteSpace(FileStream stream)
    {
        int b;
        while (char.IsWhiteSpace((char)(b = stream.ReadByte())));
        if (b is -1)
        {
            return false;
        }
        stream.Position--;
        return true;
    }

    public static bool TrySkipAlphanumericString(FileStream stream, out int skippedBytes)
    {
        int b;
        for (skippedBytes = 0; (b = stream.ReadByte()) is (not -1) and >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_' or >= '0' and <= '9'; skippedBytes++) ;
        if (b is -1)
        {
            return false;
        }
        stream.Position--;
        return true;
    }

    public static bool TrySkipString(FileStream stream, out int skippedBytes)
    {
        int b;
        for (skippedBytes = 0; (b = stream.ReadByte()) is (not -1) and >= 'a' and <= 'z' or >= 'A' and <= 'Z'; skippedBytes++);
        if (b is -1)
        {
            return false;
        }
        stream.Position--;
        return true;
    }

    public static bool TryParseAlphanumericString(FileStream stream, out string? s)
    {
        if (!TrySkipAlphanumericString(stream, out int skippedBytes))
        {
            s = null;
            return false;
        }
        stream.Position -= skippedBytes;
        byte[] buffer = new byte[skippedBytes];
        stream.Read(buffer, 0, skippedBytes);
        s = Encoding.ASCII.GetString(buffer);
        return true;
    }

    public static bool TryParseString(FileStream stream, out string? s)
    {
        if (!TrySkipString(stream, out int skippedBytes))
        {
            s = null;
            return false;
        }
        stream.Position -= skippedBytes;
        byte[] buffer = new byte[skippedBytes];
        stream.Read(buffer, 0, skippedBytes);
        s = Encoding.ASCII.GetString(buffer);
        return true;
    }

    public static bool TrySkipNumber(FileStream stream, out int skippedBytes)
    {
        int b;
        if ((b = stream.ReadByte()) is '-' || char.IsDigit((char)b))
        {
            skippedBytes = 1;
            if (b is '-')
            {
                if (!char.IsDigit((char)stream.ReadByte()))
                {
                    throw new InvalidDataException("encountered element that was not a number!");
                }
                skippedBytes = 2;
            }
            if (!char.IsWhiteSpace((char)stream.ReadByte()))
            {
                throw new InvalidDataException("encountered element that was not a number! There was no whitespace following it.");
            }
            stream.Position--;
            return true;
        }
        skippedBytes = 0;
        return false;
    }
}
