using System;
using System.IO;

namespace JsonT;

public sealed class JsonTextWriter : JsonWriter, IDisposable
{
    private readonly TextWriter writer;
    private int indent;
    private int arrayIndent;
    private int arrayDepth = 0;
    private bool arrayBegin;
    private bool wasValue;
    private bool wasBracket;

    private JsonTextWriter(Stream fs) 
    {
        writer = new StreamWriter(fs);
    }

    public static void WriteToFile(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    public static void WriteToFile(Stream fs, JsonValue value) 
    {
        using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    private void Newline() 
    {
        writer.Write("\n");
        for (int i = 0; i < indent; i++)
            writer.Write("\t");
    }

    private void NextValue() 
    {
        if (wasValue)
            writer.Write(", ");
        if ((wasValue || wasBracket) && !arrayBegin) 
        {
            Newline();
        }
        else if (arrayBegin) 
        {
            if (arrayIndent < arrayDepth)
                arrayIndent++;
            else 
            {
                arrayIndent = 0;
                Newline();
            }
        }
        

        wasValue = true;
        wasBracket = false;
    }

    private void NextKey() 
    {
        if (wasValue)
            writer.Write(",");
        Newline();
        wasValue = false;
        wasBracket = false;
    }
    
    private void NextBracket() 
    {
        if (wasValue)
            writer.Write(",");
        if (wasBracket)
            writer.Write(" ");
        wasBracket = true;
        wasValue = false;
    }

    public override void BeginArray()
    {
        NextBracket();
        writer.Write("[");
        indent++;
        arrayBegin = true;
    }

    public override void BeginObject()
    {
        NextBracket();
        writer.Write("{");
        indent++;
    }

    public void Dispose()
    {
        writer.Dispose();
    }

    public override void EndArray()
    {
        indent--;
        if (wasBracket)
            writer.Write(" ");
        else   
            Newline();

        writer.Write("]");
        wasValue = true;
        wasBracket = false;
        arrayBegin = false;
    }

    public override void EndObject()
    {
        indent--;
        if (wasBracket)
            writer.Write(" ");
        else   
            Newline();

        writer.Write("}");
        wasValue = true;
        wasBracket = false;
    }

    public override void WriteKey(string name)
    {
        NextKey();
        writer.Write($"\"{name}\"");
        writer.Write(": ");
    }

    public override void WriteNull()
    {
        NextValue();
        writer.Write("null");
    }

    public override void WriteValue(byte value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(short value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(int value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(long value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(sbyte value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(ushort value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(uint value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(ulong value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(float value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(double value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(decimal value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(bool value)
    {
        NextValue();
        writer.Write(value ? "true" : "false");
    }

    public override void WriteValue(char value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(string value)
    {
        NextValue();
        writer.Write("\"" + value + "\"");
    }

    public override void WriteValue(nint value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(nuint value)
    {
        NextValue();
        writer.Write(value);
    }
}