using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace LeakBlocker
{
    internal static class Program
    {
        private static readonly Assembly currentAssembly = Assembly.GetExecutingAssembly();
        private static readonly IList<string> resources = currentAssembly.GetManifestResourceNames();

        internal static int Start(string[] parameters, Thread splashScreenThread = null)
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            if (splashScreenThread != null)
                splashScreenThread.Start();

            MethodInfo entryPoint = null;

            foreach (string resource in resources)
            {
                if (resource.EndsWith(".resources"))
                    continue;
                Assembly assembly = LoadAssemblyFromResource(resource);
                if (assembly.EntryPoint != null)
                    entryPoint = assembly.EntryPoint;
            }

            if (entryPoint == null)
                throw new FileNotFoundException();

            ParameterInfo[] parameterDescriptions = entryPoint.GetParameters();
            object[] parameterValues = (parameterDescriptions.Length == 0) ? new object[0] : new object[] { parameters };

            object returnValue = entryPoint.Invoke(null, parameterValues);

            return (returnValue is int) ? (int)returnValue : 0;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            foreach (string currentResource in resources)
            {
                if (currentResource.EndsWith(".resources"))
                    continue;
                if (currentResource.Contains(assemblyName.Name))
                    return LoadAssemblyFromResource(currentResource);
            }

            return null;
        }

        private static Assembly LoadAssemblyFromResource(string resource)
        {
            using (Stream stream = currentAssembly.GetManifestResourceStream(resource))
            {
                int firstByte = stream.ReadByte();
                int secondByte = stream.ReadByte();
                stream.Position = 0;

                bool compressed = ((firstByte != 0x4D) || (secondByte != 0x5A));

                byte[] assemblyData = compressed ? DecompressResource(stream) : GetAssemblyData(stream);

                return Assembly.Load(assemblyData);
            }
        }
                
        private static byte[] GetAssemblyData(Stream inStream)
        {
            var assemblyData = new byte[inStream.Length];
            inStream.Position = 0;
            inStream.Read(assemblyData, 0, (int)inStream.Length);
            return assemblyData;
        }
        
        private static byte[] DecompressResource(Stream inStream)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                uint[] mIsMatchDecoders = new uint[192];
                uint[] mIsRepDecoders = new uint[12];
                uint[] mIsRepG0Decoders = new uint[12];
                uint[] mIsRepG1Decoders = new uint[12];
                uint[] mIsRepG2Decoders = new uint[12];
                uint[] mIsRep0LongDecoders = new uint[192];

                uint[] mPosDecoders = new uint[114];

                KeyValuePair<uint[], int> mPosAlignDecoder = new KeyValuePair<uint[], int>(new uint[16], 4);

                KeyValuePair<uint[], int>[] mLenDecoderLowCoder = new KeyValuePair<uint[], int>[16];
                KeyValuePair<uint[], int>[] mLenDecoderMidCoder = new KeyValuePair<uint[], int>[16];
                KeyValuePair<uint[], int> mLenDecoderHighCoder = new KeyValuePair<uint[], int>(new uint[256], 8);
                uint mLenDecoderNumPosStates = 0;

                KeyValuePair<uint[], int>[] mRepLenDecoderLowCoder = new KeyValuePair<uint[], int>[16];
                KeyValuePair<uint[], int>[] mRepLenDecoderMidCoder = new KeyValuePair<uint[], int>[16];
                KeyValuePair<uint[], int> mRepLenDecoderHighCoder = new KeyValuePair<uint[], int>(new uint[256], 8);
                uint mRepLenDecoderNumPosStates = 0;

                uint mDictionarySize = 0xFFFFFFFF;
                uint mDictionarySizeCheck = 0;

                byte[] buffer = new byte[0];
                uint pos = 0;
                uint windowSize = 0;
                uint streamPos = 0;

                KeyValuePair<uint[], int>[] mPosSlotDecoder = new KeyValuePair<uint[], int>[4];

                byte[] properties = new byte[5];

                mPosSlotDecoder[0] = new KeyValuePair<uint[], int>(new uint[64], 6);
                mPosSlotDecoder[1] = new KeyValuePair<uint[], int>(new uint[64], 6);
                mPosSlotDecoder[2] = new KeyValuePair<uint[], int>(new uint[64], 6);
                mPosSlotDecoder[3] = new KeyValuePair<uint[], int>(new uint[64], 6);

                if (inStream.Read(properties, 0, 5) != 5)
                    throw new InvalidOperationException();

                long outSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = inStream.ReadByte();
                    if (v < 0)
                        throw (new Exception("Can't Read 1"));
                    outSize |= ((long)(byte)v) << (8 * i);
                }

                if (properties.Length < 5)
                    throw new ArgumentException();
                int lc = properties[0] % 9;
                int remainder = properties[0] / 9;
                int lp = remainder % 5;
                int pb = remainder / 5;
                if (pb > 4)
                    throw new ArgumentException();
                UInt32 dictionarySize = 0;
                for (int i = 0; i < 4; i++)
                    dictionarySize += ((UInt32)(properties[1 + i])) << (i * 8);

                if (mDictionarySize != dictionarySize)
                {
                    mDictionarySize = dictionarySize;
                    mDictionarySizeCheck = Math.Max(mDictionarySize, 1);
                    uint blockSize3 = Math.Max(mDictionarySizeCheck, 4096);

                    if (windowSize != blockSize3)
                        buffer = new byte[blockSize3];
                    windowSize = blockSize3;
                    pos = 0;
                    streamPos = 0;
                }

                if (lp > 8)
                    throw new ArgumentException();
                if (lc > 8)
                    throw new ArgumentException();

                int mLiteralDecoderNumPosBits = lp;
                uint mLiteralDecoderPosMask = ((uint)1 << lp) - 1;
                int mLiteralDecoderNumPrevBits = lc;
                uint numStates5 = (uint)1 << (mLiteralDecoderNumPrevBits + mLiteralDecoderNumPosBits);
                uint[][] mLiteralDecoderCoders = new uint[numStates5][];

                for (int i = 0; i < numStates5; i++)
                    mLiteralDecoderCoders[i] = new uint[0x300];

                if (pb > 4)
                    throw new ArgumentException();
                uint numPosStates1 = (uint)1 << pb;

                for (uint posState = mLenDecoderNumPosStates; posState < numPosStates1; posState++)
                {
                    mLenDecoderLowCoder[posState] = new KeyValuePair<uint[], int>(new uint[8], 3);
                    mLenDecoderMidCoder[posState] = new KeyValuePair<uint[], int>(new uint[8], 3);
                }
                mLenDecoderNumPosStates = numPosStates1;

                for (uint posState = mRepLenDecoderNumPosStates; posState < numPosStates1; posState++)
                {
                    mRepLenDecoderLowCoder[posState] = new KeyValuePair<uint[], int>(new uint[8], 3);
                    mRepLenDecoderMidCoder[posState] = new KeyValuePair<uint[], int>(new uint[8], 3);
                }
                mRepLenDecoderNumPosStates = numPosStates1;

                uint mPosStateMask = numPosStates1 - 1;

                Stream rangeDecoderStream = inStream;

                uint rangeDecoderCode = 0;
                uint rangeDecoderRange = 0xFFFFFFFF;
                for (int j = 0; j < 5; j++)
                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();

                if ((pos - streamPos) != 0)
                {
                    outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));
                }

                streamPos = 0;
                pos = 0;

                uint i4;
                for (i4 = 0; i4 < 12; i4++)
                {
                    for (uint j = 0; j <= mPosStateMask; j++)
                    {
                        uint index = (i4 << 4) + j;
                        mIsMatchDecoders[index] = 1024;
                        mIsRep0LongDecoders[index] = 1024;
                    }
                    mIsRepDecoders[i4] = 1024;
                    mIsRepG0Decoders[i4] = 1024;
                    mIsRepG1Decoders[i4] = 1024;
                    mIsRepG2Decoders[i4] = 1024;
                }

                uint numStates1 = (uint)1 << (mLiteralDecoderNumPrevBits + mLiteralDecoderNumPosBits);
                for (uint i1 = 0; i1 < numStates1; i1++)
                {
                    for (int j = 0; j < 0x300; j++)
                        mLiteralDecoderCoders[i1][j] = 1024;
                }

                for (i4 = 0; i4 < 4; i4++)
                {
                    for (uint i7 = 1; i7 < (1 << mPosSlotDecoder[i4].Value); i7++)
                        mPosSlotDecoder[i4].Key[i7] = 1024;
                }

                for (i4 = 0; i4 < 114; i4++)
                    mPosDecoders[i4] = 1024;

                uint mLenDecoderChoice = 1024;
                for (uint posState = 0; posState < mLenDecoderNumPosStates; posState++)
                {
                    for (uint i7 = 1; i7 < (1 << mLenDecoderLowCoder[posState].Value); i7++)
                        mLenDecoderLowCoder[posState].Key[i7] = 1024;

                    for (uint i7 = 1; i7 < (1 << mLenDecoderMidCoder[posState].Value); i7++)
                        mLenDecoderMidCoder[posState].Key[i7] = 1024;
                }
                uint mLenDecoderChoice2 = 1024;

                for (uint i7 = 1; i7 < (1 << mLenDecoderHighCoder.Value); i7++)
                    mLenDecoderHighCoder.Key[i7] = 1024;

                uint mRepLenDecoderChoice = 1024;
                for (uint posState = 0; posState < mRepLenDecoderNumPosStates; posState++)
                {
                    for (uint i7 = 1; i7 < (1 << mRepLenDecoderLowCoder[posState].Value); i7++)
                        mRepLenDecoderLowCoder[posState].Key[i7] = 1024;

                    for (uint i7 = 1; i7 < (1 << mRepLenDecoderMidCoder[posState].Value); i7++)
                        mRepLenDecoderMidCoder[posState].Key[i7] = 1024;
                }
                uint mRepLenDecoderChoice2 = 1024;

                for (uint i7 = 1; i7 < (1 << mRepLenDecoderHighCoder.Value); i7++)
                    mRepLenDecoderHighCoder.Key[i7] = 1024;

                for (uint i7 = 1; i7 < (1 << mPosAlignDecoder.Value); i7++)
                    mPosAlignDecoder.Key[i7] = 1024;

                uint state = 0;
                uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;

                UInt64 nowPos64 = 0;
                UInt64 outSize64 = (UInt64)outSize;
                if (nowPos64 < outSize64)
                {
                    uint decodeResult;
                    if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsMatchDecoders[state << 4]))
                    {
                        rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsMatchDecoders[state << 4]);
                        mIsMatchDecoders[state << 4] += (2048 - mIsMatchDecoders[state << 4]) >> 5;
                        if (rangeDecoderRange < 16777216)
                        {
                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                            rangeDecoderRange <<= 8;
                        }
                        decodeResult = 0;
                    }
                    else
                    {
                        rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsMatchDecoders[state << 4]);
                        rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsMatchDecoders[state << 4]);
                        mIsMatchDecoders[state << 4] -= (mIsMatchDecoders[state << 4]) >> 5;
                        if (rangeDecoderRange < 16777216)
                        {
                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                            rangeDecoderRange <<= 8;
                        }
                        decodeResult = 1;
                    }

                    if (decodeResult != 0)
                        throw new InvalidOperationException();

                    if (state < 4) state = 0;
                    else if (state < 10) state -= 3;
                    else state -= 6;

                    uint symbol = 1;
                    do
                    {
                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]))
                        {
                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                            mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol] += (2048 - mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]) >> 5;
                            if (rangeDecoderRange < 16777216)
                            {
                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                rangeDecoderRange <<= 8;
                            }
                            decodeResult = 0;
                        }
                        else
                        {
                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                            mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol] -= (mLiteralDecoderCoders[((0 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(0 >> (8 - mLiteralDecoderNumPrevBits))][symbol]) >> 5;
                            if (rangeDecoderRange < 16777216)
                            {
                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                rangeDecoderRange <<= 8;
                            }
                            decodeResult = 1;
                        }

                        symbol = (symbol << 1) | decodeResult;
                    }
                    while (symbol < 0x100);
                    byte b = (byte)symbol;

                    buffer[pos++] = b;
                    if (pos >= windowSize)
                    {
                        if ((pos - streamPos) != 0)
                        {
                            outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));
                            if (pos >= windowSize)
                                pos = 0;
                            streamPos = pos;
                        }
                    }

                    nowPos64++;
                }
                while (nowPos64 < outSize64)
                {
                    uint posState = (uint)nowPos64 & mPosStateMask;

                    uint decodeResult;
                    if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsMatchDecoders[(state << 4) + posState]))
                    {
                        rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsMatchDecoders[(state << 4) + posState]);
                        mIsMatchDecoders[(state << 4) + posState] += (2048 - mIsMatchDecoders[(state << 4) + posState]) >> 5;
                        if (rangeDecoderRange < 16777216)
                        {
                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                            rangeDecoderRange <<= 8;
                        }
                        decodeResult = 0;
                    }
                    else
                    {
                        rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsMatchDecoders[(state << 4) + posState]);
                        rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsMatchDecoders[(state << 4) + posState]);
                        mIsMatchDecoders[(state << 4) + posState] -= (mIsMatchDecoders[(state << 4) + posState]) >> 5;
                        if (rangeDecoderRange < 16777216)
                        {
                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                            rangeDecoderRange <<= 8;
                        }
                        decodeResult = 1;
                    }

                    if (decodeResult == 0)
                    {
                        byte b;
                        byte prevByte = buffer[(pos - 1) + (((pos - 1) >= windowSize) ? windowSize : 0)];
                        if (!(state < 7))
                        {
                            uint[] decoder2 = mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))];
                            byte matchByte = buffer[(pos - rep0 - 1) + (((pos - rep0 - 1) >= windowSize) ? windowSize : 0)];

                            uint symbol = 1;
                            do
                            {
                                uint matchBit = (uint)(matchByte >> 7) & 1;
                                matchByte <<= 1;

                                uint bit;
                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * decoder2[((1 + matchBit) << 8) + symbol]))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * decoder2[((1 + matchBit) << 8) + symbol]);
                                    decoder2[((1 + matchBit) << 8) + symbol] += (2048 - decoder2[((1 + matchBit) << 8) + symbol]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    bit = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * decoder2[((1 + matchBit) << 8) + symbol]);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * decoder2[((1 + matchBit) << 8) + symbol]);
                                    decoder2[((1 + matchBit) << 8) + symbol] -= (decoder2[((1 + matchBit) << 8) + symbol]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    bit = 1;
                                }

                                symbol = (symbol << 1) | bit;
                                if (matchBit != bit)
                                {
                                    while (symbol < 0x100)
                                    {
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * decoder2[symbol]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * decoder2[symbol]);
                                            decoder2[symbol] += (2048 - decoder2[symbol]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * decoder2[symbol]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * decoder2[symbol]);
                                            decoder2[symbol] -= (decoder2[symbol]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 1;
                                        }

                                        symbol = (symbol << 1) | decodeResult;
                                    }
                                    break;
                                }
                            }
                            while (symbol < 0x100);
                            b = (byte)symbol;
                        }
                        else
                        {
                            uint symbol = 1;
                            do
                            {
                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                                    mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol] += (2048 - mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]);
                                    mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol] -= (mLiteralDecoderCoders[(((uint)nowPos64 & mLiteralDecoderPosMask) << mLiteralDecoderNumPrevBits) + (uint)(prevByte >> (8 - mLiteralDecoderNumPrevBits))][symbol]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                symbol = (symbol << 1) | decodeResult;
                            }
                            while (symbol < 0x100);
                            b = (byte)symbol;
                        }

                        buffer[pos++] = b;
                        if (pos >= windowSize)
                        {
                            if ((pos - streamPos) != 0)
                            {
                                outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));
                                if (pos >= windowSize)
                                    pos = 0;
                                streamPos = pos;
                            }
                        }

                        if (state < 4) state = 0;
                        else if (state < 10) state -= 3;
                        else state -= 6;

                        nowPos64++;
                    }
                    else
                    {
                        uint len;

                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsRepDecoders[state]))
                        {
                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsRepDecoders[state]);
                            mIsRepDecoders[state] += (2048 - mIsRepDecoders[state]) >> 5;
                            if (rangeDecoderRange < 16777216)
                            {
                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                rangeDecoderRange <<= 8;
                            }
                            decodeResult = 0;
                        }
                        else
                        {
                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsRepDecoders[state]);
                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsRepDecoders[state]);
                            mIsRepDecoders[state] -= (mIsRepDecoders[state]) >> 5;
                            if (rangeDecoderRange < 16777216)
                            {
                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                rangeDecoderRange <<= 8;
                            }
                            decodeResult = 1;
                        }

                        if (decodeResult == 1)
                        {
                            if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsRepG0Decoders[state]))
                            {
                                rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsRepG0Decoders[state]);
                                mIsRepG0Decoders[state] += (2048 - mIsRepG0Decoders[state]) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 0;
                            }
                            else
                            {
                                rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsRepG0Decoders[state]);
                                rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsRepG0Decoders[state]);
                                mIsRepG0Decoders[state] -= (mIsRepG0Decoders[state]) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 1;
                            }

                            if (decodeResult == 0)
                            {
                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsRep0LongDecoders[(state << 4) + posState]))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsRep0LongDecoders[(state << 4) + posState]);
                                    mIsRep0LongDecoders[(state << 4) + posState] += (2048 - mIsRep0LongDecoders[(state << 4) + posState]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsRep0LongDecoders[(state << 4) + posState]);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsRep0LongDecoders[(state << 4) + posState]);
                                    mIsRep0LongDecoders[(state << 4) + posState] -= (mIsRep0LongDecoders[(state << 4) + posState]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                if (decodeResult == 0)
                                {
                                    state = (uint)(state < 7 ? 9 : 11);

                                    byte b = buffer[(pos - rep0 - 1) + (((pos - rep0 - 1) >= windowSize) ? windowSize : 0)];

                                    buffer[pos++] = b;
                                    if (pos >= windowSize)
                                    {
                                        if ((pos - streamPos) != 0)
                                        {
                                            outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));
                                            if (pos >= windowSize)
                                                pos = 0;
                                            streamPos = pos;
                                        }
                                    }

                                    nowPos64++;
                                    continue;
                                }
                            }
                            else
                            {
                                UInt32 distance;

                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsRepG1Decoders[state]))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsRepG1Decoders[state]);
                                    mIsRepG1Decoders[state] += (2048 - mIsRepG1Decoders[state]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsRepG1Decoders[state]);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsRepG1Decoders[state]);
                                    mIsRepG1Decoders[state] -= (mIsRepG1Decoders[state]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                if (decodeResult == 0)
                                {
                                    distance = rep1;
                                }
                                else
                                {
                                    if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mIsRepG2Decoders[state]))
                                    {
                                        rangeDecoderRange = ((rangeDecoderRange >> 11) * mIsRepG2Decoders[state]);
                                        mIsRepG2Decoders[state] += (2048 - mIsRepG2Decoders[state]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 0;
                                    }
                                    else
                                    {
                                        rangeDecoderCode -= ((rangeDecoderRange >> 11) * mIsRepG2Decoders[state]);
                                        rangeDecoderRange -= ((rangeDecoderRange >> 11) * mIsRepG2Decoders[state]);
                                        mIsRepG2Decoders[state] -= (mIsRepG2Decoders[state]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 1;
                                    }

                                    if (decodeResult == 0)
                                        distance = rep2;
                                    else
                                    {
                                        distance = rep3;
                                        rep3 = rep2;
                                    }
                                    rep2 = rep1;
                                }
                                rep1 = rep0;
                                rep0 = distance;
                            }

                            if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mRepLenDecoderChoice))
                            {
                                rangeDecoderRange = ((rangeDecoderRange >> 11) * mRepLenDecoderChoice);
                                mRepLenDecoderChoice += (2048 - mRepLenDecoderChoice) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 0;
                            }
                            else
                            {
                                rangeDecoderCode -= ((rangeDecoderRange >> 11) * mRepLenDecoderChoice);
                                rangeDecoderRange -= ((rangeDecoderRange >> 11) * mRepLenDecoderChoice);
                                mRepLenDecoderChoice -= (mRepLenDecoderChoice) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 1;
                            }

                            if (decodeResult == 0)
                            {
                                len = 1;
                                for (int bitIndex = mRepLenDecoderLowCoder[posState].Value; bitIndex > 0; bitIndex--)
                                {
                                    if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mRepLenDecoderLowCoder[posState].Key[len]))
                                    {
                                        rangeDecoderRange = ((rangeDecoderRange >> 11) * mRepLenDecoderLowCoder[posState].Key[len]);
                                        mRepLenDecoderLowCoder[posState].Key[len] += (2048 - mRepLenDecoderLowCoder[posState].Key[len]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 0;
                                    }
                                    else
                                    {
                                        rangeDecoderCode -= ((rangeDecoderRange >> 11) * mRepLenDecoderLowCoder[posState].Key[len]);
                                        rangeDecoderRange -= ((rangeDecoderRange >> 11) * mRepLenDecoderLowCoder[posState].Key[len]);
                                        mRepLenDecoderLowCoder[posState].Key[len] -= (mRepLenDecoderLowCoder[posState].Key[len]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 1;
                                    }

                                    len = (len << 1) + decodeResult;
                                }
                                len -= ((uint)1 << mRepLenDecoderLowCoder[posState].Value);
                            }
                            else
                            {
                                uint symbol7 = 8;

                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mRepLenDecoderChoice2))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mRepLenDecoderChoice2);
                                    mRepLenDecoderChoice2 += (2048 - mRepLenDecoderChoice2) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mRepLenDecoderChoice2);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mRepLenDecoderChoice2);
                                    mRepLenDecoderChoice2 -= (mRepLenDecoderChoice2) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                if (decodeResult == 0)
                                {
                                    uint m8 = 1;
                                    for (int bitIndex = mRepLenDecoderMidCoder[posState].Value; bitIndex > 0; bitIndex--)
                                    {
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mRepLenDecoderMidCoder[posState].Key[m8]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mRepLenDecoderMidCoder[posState].Key[m8]);
                                            mRepLenDecoderMidCoder[posState].Key[m8] += (2048 - mRepLenDecoderMidCoder[posState].Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mRepLenDecoderMidCoder[posState].Key[m8]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mRepLenDecoderMidCoder[posState].Key[m8]);
                                            mRepLenDecoderMidCoder[posState].Key[m8] -= (mRepLenDecoderMidCoder[posState].Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 1;
                                        }

                                        m8 = (m8 << 1) + decodeResult;
                                    }
                                    m8 -= ((uint)1 << mRepLenDecoderMidCoder[posState].Value);

                                    symbol7 += m8;
                                }
                                else
                                {
                                    symbol7 += 8;

                                    uint m8 = 1;
                                    for (int bitIndex = mRepLenDecoderHighCoder.Value; bitIndex > 0; bitIndex--)
                                    {
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mRepLenDecoderHighCoder.Key[m8]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mRepLenDecoderHighCoder.Key[m8]);
                                            mRepLenDecoderHighCoder.Key[m8] += (2048 - mRepLenDecoderHighCoder.Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mRepLenDecoderHighCoder.Key[m8]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mRepLenDecoderHighCoder.Key[m8]);
                                            mRepLenDecoderHighCoder.Key[m8] -= (mRepLenDecoderHighCoder.Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 1;
                                        }

                                        m8 = (m8 << 1) + decodeResult;
                                    }
                                    m8 -= ((uint)1 << mRepLenDecoderHighCoder.Value);

                                    symbol7 += m8;
                                }
                                len = symbol7;
                            }
                            len += 2;

                            state = (uint)(state < 7 ? 8 : 11);
                        }
                        else
                        {
                            rep3 = rep2;
                            rep2 = rep1;
                            rep1 = rep0;

                            if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLenDecoderChoice))
                            {
                                rangeDecoderRange = ((rangeDecoderRange >> 11) * mLenDecoderChoice);
                                mLenDecoderChoice += (2048 - mLenDecoderChoice) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 0;
                            }
                            else
                            {
                                rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLenDecoderChoice);
                                rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLenDecoderChoice);
                                mLenDecoderChoice -= (mLenDecoderChoice) >> 5;
                                if (rangeDecoderRange < 16777216)
                                {
                                    rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                    rangeDecoderRange <<= 8;
                                }
                                decodeResult = 1;
                            }

                            if (decodeResult == 0)
                            {
                                len = 1;
                                for (int bitIndex = mLenDecoderLowCoder[posState].Value; bitIndex > 0; bitIndex--)
                                {
                                    if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLenDecoderLowCoder[posState].Key[len]))
                                    {
                                        rangeDecoderRange = ((rangeDecoderRange >> 11) * mLenDecoderLowCoder[posState].Key[len]);
                                        mLenDecoderLowCoder[posState].Key[len] += (2048 - mLenDecoderLowCoder[posState].Key[len]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 0;
                                    }
                                    else
                                    {
                                        rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLenDecoderLowCoder[posState].Key[len]);
                                        rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLenDecoderLowCoder[posState].Key[len]);
                                        mLenDecoderLowCoder[posState].Key[len] -= (mLenDecoderLowCoder[posState].Key[len]) >> 5;
                                        if (rangeDecoderRange < 16777216)
                                        {
                                            rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                            rangeDecoderRange <<= 8;
                                        }
                                        decodeResult = 1;
                                    }

                                    len = (len << 1) + decodeResult;
                                }
                                len -= ((uint)1 << mLenDecoderLowCoder[posState].Value);
                            }
                            else
                            {
                                uint symbol7 = 8;

                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLenDecoderChoice2))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mLenDecoderChoice2);
                                    mLenDecoderChoice2 += (2048 - mLenDecoderChoice2) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLenDecoderChoice2);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLenDecoderChoice2);
                                    mLenDecoderChoice2 -= (mLenDecoderChoice2) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                if (decodeResult == 0)
                                {
                                    uint m8 = 1;
                                    for (int bitIndex = mLenDecoderMidCoder[posState].Value; bitIndex > 0; bitIndex--)
                                    {
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLenDecoderMidCoder[posState].Key[m8]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mLenDecoderMidCoder[posState].Key[m8]);
                                            mLenDecoderMidCoder[posState].Key[m8] += (2048 - mLenDecoderMidCoder[posState].Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLenDecoderMidCoder[posState].Key[m8]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLenDecoderMidCoder[posState].Key[m8]);
                                            mLenDecoderMidCoder[posState].Key[m8] -= (mLenDecoderMidCoder[posState].Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 1;
                                        }

                                        m8 = (m8 << 1) + decodeResult;
                                    }
                                    m8 -= ((uint)1 << mLenDecoderMidCoder[posState].Value);

                                    symbol7 += m8;
                                }
                                else
                                {
                                    symbol7 += 8;

                                    uint m8 = 1;
                                    for (int bitIndex = mLenDecoderHighCoder.Value; bitIndex > 0; bitIndex--)
                                    {
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mLenDecoderHighCoder.Key[m8]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mLenDecoderHighCoder.Key[m8]);
                                            mLenDecoderHighCoder.Key[m8] += (2048 - mLenDecoderHighCoder.Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mLenDecoderHighCoder.Key[m8]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mLenDecoderHighCoder.Key[m8]);
                                            mLenDecoderHighCoder.Key[m8] -= (mLenDecoderHighCoder.Key[m8]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            decodeResult = 1;
                                        }

                                        m8 = (m8 << 1) + decodeResult;
                                    }

                                    m8 -= ((uint)1 << mLenDecoderHighCoder.Value);

                                    symbol7 += m8;
                                }
                                len = symbol7;
                            }
                            len += 2;

                            state = (uint)(state < 7 ? 7 : 10);

                            uint len2 = len - 2;
                            if (len2 >= 4)
                                len2 = 3;

                            uint posSlot = 1;
                            for (int bitIndex = mPosSlotDecoder[len2].Value; bitIndex > 0; bitIndex--)
                            {
                                if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mPosSlotDecoder[len2].Key[posSlot]))
                                {
                                    rangeDecoderRange = ((rangeDecoderRange >> 11) * mPosSlotDecoder[len2].Key[posSlot]);
                                    mPosSlotDecoder[len2].Key[posSlot] += (2048 - mPosSlotDecoder[len2].Key[posSlot]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 0;
                                }
                                else
                                {
                                    rangeDecoderCode -= ((rangeDecoderRange >> 11) * mPosSlotDecoder[len2].Key[posSlot]);
                                    rangeDecoderRange -= ((rangeDecoderRange >> 11) * mPosSlotDecoder[len2].Key[posSlot]);
                                    mPosSlotDecoder[len2].Key[posSlot] -= (mPosSlotDecoder[len2].Key[posSlot]) >> 5;
                                    if (rangeDecoderRange < 16777216)
                                    {
                                        rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                        rangeDecoderRange <<= 8;
                                    }
                                    decodeResult = 1;
                                }

                                posSlot = (posSlot << 1) + decodeResult;
                            }
                            posSlot -= ((uint)1 << mPosSlotDecoder[len2].Value);

                            if (posSlot >= 4)
                            {
                                int numDirectBits = (int)((posSlot >> 1) - 1);
                                rep0 = ((2 | (posSlot & 1)) << numDirectBits);
                                if (posSlot < 14)
                                {
                                    uint m = 1;
                                    uint symbol = 0;
                                    uint startIndex = rep0 - posSlot - 1;
                                    for (int bitIndex = 0; bitIndex < numDirectBits; bitIndex++)
                                    {
                                        uint bit;
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mPosDecoders[startIndex + m]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mPosDecoders[startIndex + m]);
                                            mPosDecoders[startIndex + m] += (2048 - mPosDecoders[startIndex + m]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            bit = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mPosDecoders[startIndex + m]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mPosDecoders[startIndex + m]);
                                            mPosDecoders[startIndex + m] -= (mPosDecoders[startIndex + m]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            bit = 1;
                                        }

                                        m <<= 1;
                                        m += bit;
                                        symbol |= (bit << bitIndex);
                                    }

                                    rep0 += symbol;
                                }
                                else
                                {
                                    uint range1 = rangeDecoderRange;
                                    uint code1 = rangeDecoderCode;
                                    uint result1 = 0;
                                    for (int i3 = numDirectBits - 4; i3 > 0; i3--)
                                    {
                                        range1 >>= 1;

                                        uint t = (code1 - range1) >> 31;
                                        code1 -= range1 & (t - 1);
                                        result1 = (result1 << 1) | (1 - t);

                                        if (range1 < 16777216)
                                        {
                                            code1 = (code1 << 8) | (byte)rangeDecoderStream.ReadByte();
                                            range1 <<= 8;
                                        }
                                    }
                                    rangeDecoderRange = range1;
                                    rangeDecoderCode = code1;

                                    rep0 += (result1 << 4);

                                    uint m = 1;
                                    uint symbol = 0;
                                    for (int bitIndex = 0; bitIndex < mPosAlignDecoder.Value; bitIndex++)
                                    {
                                        uint bit;
                                        if (rangeDecoderCode < ((rangeDecoderRange >> 11) * mPosAlignDecoder.Key[m]))
                                        {
                                            rangeDecoderRange = ((rangeDecoderRange >> 11) * mPosAlignDecoder.Key[m]);
                                            mPosAlignDecoder.Key[m] += (2048 - mPosAlignDecoder.Key[m]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            bit = 0;
                                        }
                                        else
                                        {
                                            rangeDecoderCode -= ((rangeDecoderRange >> 11) * mPosAlignDecoder.Key[m]);
                                            rangeDecoderRange -= ((rangeDecoderRange >> 11) * mPosAlignDecoder.Key[m]);
                                            mPosAlignDecoder.Key[m] -= (mPosAlignDecoder.Key[m]) >> 5;
                                            if (rangeDecoderRange < 16777216)
                                            {
                                                rangeDecoderCode = (rangeDecoderCode << 8) | (byte)rangeDecoderStream.ReadByte();
                                                rangeDecoderRange <<= 8;
                                            }
                                            bit = 1;
                                        }

                                        m <<= 1;
                                        m += bit;
                                        symbol |= (bit << bitIndex);
                                    }

                                    rep0 += symbol;
                                }
                            }
                            else
                                rep0 = posSlot;
                        }
                        if (rep0 >= nowPos64 || rep0 >= mDictionarySizeCheck)
                        {
                            if (rep0 == 0xFFFFFFFF)
                                break;
                            throw new InvalidOperationException();
                        }

                        uint len1 = len;
                        uint distance2 = rep0;
                        uint pos2 = pos - distance2 - 1;
                        if (pos2 >= windowSize)
                            pos2 += windowSize;
                        for (; len1 > 0; len1--)
                        {
                            if (pos2 >= windowSize)
                                pos2 = 0;
                            buffer[pos++] = buffer[pos2++];
                            if (pos >= windowSize)
                            {
                                if ((pos - streamPos) != 0)
                                {
                                    outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));
                                    if (pos >= windowSize)
                                        pos = 0;
                                    streamPos = pos;
                                }
                            }
                        }

                        nowPos64 += len;
                    }
                }

                if ((pos - streamPos) != 0)
                    outStream.Write(buffer, (int)streamPos, (int)(pos - streamPos));

                byte[] sourceData = new byte[outStream.Length];
                outStream.Position = 0;
                outStream.Read(sourceData, 0, (int)outStream.Length);
                return sourceData;
            }
        }
    }
}
















