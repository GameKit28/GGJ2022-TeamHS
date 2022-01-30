using System;

public static class MiscExtensions 
{
    public static Int32 WrapShift(this Int32 input, int shift){
        uint uInput = (uint)input;
        return (Int32)(uInput << shift | uInput >> (32 - shift));
    }
}