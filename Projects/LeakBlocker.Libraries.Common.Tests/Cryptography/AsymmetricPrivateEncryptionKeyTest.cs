﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.Libraries.Common.Tests.Cryptography
{
    partial class AsymmetricPrivateEncryptionKeyTest
    {
        private static IEnumerable<AsymmetricPrivateEncryptionKey> GetInstances()
        {
            yield return new AsymmetricPrivateEncryptionKey(new byte[] { 0x07, 0x02, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x52, 0x53, 0x41, 0x32, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x35, 0xC1, 0x46, 0x21, 0x52, 0x6E, 0x41, 0x31, 0x1A, 0xF6, 0x64, 0x85, 0x01, 0xF9, 0x45, 0xA2, 0xAE, 0x2D, 0xE4, 0x5C, 0x1A, 0x2F, 0xFA, 0x47, 0x62, 0xF4, 0x55, 0x98, 0x5D, 0xBC, 0x8F, 0xE1, 0x89, 0x86, 0x0A, 0xAB, 0x83, 0xCF, 0xFE, 0xD9, 0x33, 0x63, 0x48, 0x37, 0x0F, 0x2F, 0xD9, 0x6E, 0x01, 0x67, 0x6D, 0x28, 0xC8, 0x85, 0x97, 0x0C, 0x57, 0xDE, 0x94, 0x4F, 0x6B, 0x95, 0xC1, 0xB5, 0x04, 0x87, 0xD3, 0xF8, 0x9D, 0x01, 0x30, 0xC0, 0x77, 0x2B, 0xEF, 0x39, 0xF4, 0xB7, 0xDF, 0x76, 0x4B, 0xB0, 0xF6, 0x31, 0x66, 0x9F, 0x24, 0x7A, 0x9D, 0x73, 0xBA, 0x2D, 0xCD, 0xAC, 0x14, 0x1F, 0x23, 0xB4, 0x6E, 0x7A, 0xFE, 0xE9, 0x45, 0xDB, 0x75, 0x2B, 0xAF, 0xF5, 0x3B, 0x64, 0xE7, 0xFE, 0xEB, 0xDE, 0x06, 0x28, 0x3A, 0xC4, 0xFC, 0x1F, 0xF0, 0x0F, 0xCC, 0xEC, 0x75, 0x3F, 0xF6, 0x1F, 0x93, 0xD0, 0x6D, 0x65, 0x4F, 0x2E, 0x1A, 0xF4, 0xB4, 0x81, 0xA4, 0xC6, 0xBC, 0xE8, 0x97, 0xA7, 0x74, 0x1C, 0xCE, 0xF0, 0x93, 0xEA, 0x37, 0x82, 0x1D, 0xC2, 0xF7, 0x56, 0x5E, 0x1F, 0x69, 0xEE, 0xF5, 0x8C, 0x5C, 0xAB, 0x00, 0x0E, 0xD2, 0xFF, 0x81, 0xBE, 0x2F, 0xFA, 0xB3, 0x30, 0x4F, 0xF6, 0x82, 0xED, 0x81, 0x85, 0x21, 0xA0, 0xDA, 0x79, 0xF3, 0x16, 0x2E, 0x25, 0xC7, 0x6A, 0xCE, 0xFF, 0xD2, 0xE0, 0x84, 0xEC, 0xA4, 0x26, 0x50, 0xAD, 0xEF, 0x7D, 0xF5, 0xDF, 0x69, 0xC0, 0x69, 0x3F, 0x21, 0x47, 0xF4, 0xC8, 0xFA, 0x1A, 0x19, 0x12, 0x38, 0x78, 0x3C, 0xFD, 0x30, 0xC1, 0xA6, 0x20, 0x6E, 0x53, 0xEF, 0xA9, 0xB0, 0x79, 0x3F, 0xCC, 0xBA, 0xB7, 0xC3, 0xF8, 0x99, 0xD6, 0x88, 0x5A, 0xE9, 0xDB, 0x69, 0x51, 0x55, 0x17, 0x61, 0x17, 0x65, 0x41, 0x88, 0x6F, 0xF6, 0x60, 0xBA, 0x9E, 0x3F, 0xC5, 0x77, 0xDF, 0xCB, 0xC4, 0xC9, 0x0D, 0xAB, 0xBA, 0xDF, 0x38, 0x6D, 0x70, 0xB5, 0x27, 0xB3, 0x91, 0x58, 0xD4, 0x56, 0xA3, 0x32, 0xE8, 0xD3, 0x14, 0x6A, 0xB1, 0xBB, 0x0C, 0x62, 0xD6, 0x6C, 0xF9, 0x47, 0x36, 0xCB, 0x41, 0xE5, 0xB9, 0x6B, 0xEC, 0x59, 0x0A, 0x8D, 0xE7, 0x81, 0x25, 0xE0, 0x0B, 0x5E, 0xA6, 0x99, 0x59, 0x04, 0x23, 0xB6, 0x09, 0xCF, 0x58, 0x73, 0x91, 0x97, 0x41, 0x0E, 0xBD, 0xC6, 0xCF, 0xD1, 0x97, 0x78, 0x08, 0x39, 0x58, 0xCA, 0xAA, 0x66, 0xC1, 0x46, 0x1B, 0xE9, 0x19, 0x77, 0xC5, 0xC8, 0xAD, 0x09, 0x1C, 0x5F, 0x42, 0x60, 0x73, 0x0D, 0xB3, 0x8E, 0xE6, 0x20, 0xA2, 0x65, 0x5D, 0xB5, 0x5F, 0xB6, 0x05, 0x37, 0x0D, 0xD4, 0xDC, 0xC0, 0x16, 0xA2, 0xEE, 0x3B, 0xBF, 0x20, 0xD4, 0xFA, 0x0D, 0x47, 0xF3, 0xC4, 0xFF, 0x0F, 0x0E, 0x12, 0x47, 0xBB, 0xDF, 0x8B, 0x58, 0x4F, 0xFF, 0xE1, 0x61, 0xF6, 0x4A, 0x46, 0xF0, 0x19, 0xC5, 0x66, 0xBF, 0xE6, 0xC3, 0x1F, 0x33, 0x7C, 0x8F, 0x4F, 0xCB, 0x0B, 0x24, 0x0A, 0x9E, 0xD1, 0xDB, 0xC7, 0xA3, 0x2E, 0x50, 0x58, 0xFD, 0x2D, 0xF9, 0xD7, 0xA9, 0xD2, 0x35, 0x5D, 0x0B, 0x06, 0x70, 0x69, 0xD7, 0x71, 0xD2, 0x48, 0xE3, 0x97, 0x87, 0xF9, 0x70, 0x1A, 0xF8, 0x10, 0x89, 0xA3, 0x35, 0x7F, 0xF1, 0xD9, 0x16, 0x24, 0x94, 0xF3, 0x4F, 0x35, 0xEE, 0x3C, 0x3C, 0xD8, 0x8C, 0xC7, 0xED, 0x3E, 0x79, 0xE8, 0x65, 0x89, 0x17, 0x7D, 0x4D, 0xE9, 0x2C, 0x03, 0x92, 0x5C, 0x6E, 0x95, 0xEB, 0x6D, 0xB9, 0xF0, 0x54, 0x75, 0x9F, 0x3D, 0x47, 0xF2, 0x22, 0xB4, 0x3B, 0xE8, 0xFE, 0x6F, 0xAD, 0x6B, 0x78, 0x05, 0xA0, 0xAC, 0x15, 0x46, 0xD8, 0x47, 0xC1, 0x47, 0x34, 0x0E, 0x91, 0x60, 0x04, 0xBE, 0x02, 0x9F, 0xB5, 0x1F, 0x7B, 0x0B, 0x59, 0x85, 0x26, 0x02, 0xCF, 0xE1, 0xFD, 0x3E, 0xDC, 0x72, 0xA8, 0x3D, 0x4A, 0xF2, 0x46, 0xAF, 0x15, 0x11, 0xB4, 0x48, 0x03, 0x6B, 0x46, 0x73, 0xFB, 0x95, 0x52, 0x43, 0xEA, 0x52, 0xA1, 0x3F, 0x30, 0x36, 0x7D, 0x17, 0xF2, 0x30, 0x88, 0x2F, 0xAA, 0x14, 0x9B, 0x0E, 0xBC, 0xB6, 0x34, 0xB6, 0x1B, 0x09, 0x0D, 0xB4, 0x12, 0xD2, 0x02, 0xB6, 0xE5, 0x40, 0xF0, 0x83, 0x3E, 0x79, 0xED, 0x6E, 0x5B, 0xA5, 0x3E, 0x3C, 0x75, 0x1B, 0xDF, 0x86, 0x9E, 0xA5, 0x12, 0x8F, 0xBA, 0xED, 0xC9, 0xBC, 0x30, 0xD9, 0x35, 0xB3, 0x6D, 0xC3, 0x70, 0xD0, 0xE4, 0x42, 0x77, 0x16, 0x0B, 0x6D, 0xC3, 0x16, 0x19, 0x1A, 0x8E, 0x94, 0x4E, 0x1F, 0xAC, 0x62, 0xEC, 0xF6, 0xFE, 0xF9, 0x9B, 0xD0, 0x17, 0x78, 0x01, 0x22, 0xF9, 0x26, 0x3A, 0x97, 0xEA, 0x6D, 0xF4, 0x59, 0x14, 0x22, 0x29, 0x0B, 0x2F, 0x99, 0xE4, 0x63, 0xD9, 0xFD, 0xB2, 0x9B, 0x65, 0x02, 0x71, 0x7E, 0xC4, 0x40, 0x92, 0x89, 0x86, 0x27, 0xBD, 0xE0, 0x41, 0x11, 0x9F, 0x10, 0x4A, 0x4A, 0x12, 0xD2, 0x7E, 0x6D, 0x76, 0xB9, 0x3A, 0x70, 0x44, 0x82, 0x07, 0xD8, 0xC7, 0x84, 0x43, 0x7F, 0xA0, 0x29, 0x33, 0x98, 0x28, 0xEA, 0x14, 0x46, 0xF8, 0x69, 0x06, 0xBB, 0x0C, 0x13, 0x6E, 0x9E, 0x74, 0x45, 0x69, 0xCB, 0xAF, 0x66, 0x82, 0xEE, 0xB4, 0x33, 0x2E, 0x1F, 0xDD, 0x22, 0x38, 0xDB, 0xF5, 0xBA, 0xB6, 0x71, 0xBE, 0x8F, 0x97, 0x9F, 0x6C, 0xCE, 0x08, 0x9D, 0xD9, 0x29, 0xF7, 0xB7, 0x12, 0x8C, 0xB2, 0x53, 0x28, 0xE8, 0x3E, 0xAE, 0xFC, 0x76, 0x0F, 0x34, 0xD5, 0xCF, 0x55, 0x00, 0x93, 0x40, 0x69, 0xD0, 0x56, 0x47, 0x9D, 0xE0, 0x44, 0xB5, 0xC7, 0xBD, 0x6A, 0xAE, 0x83, 0x90, 0xAB, 0x2D, 0x65, 0xC2, 0x67, 0x1C, 0x94, 0x03, 0xB8, 0xAC, 0xEB, 0x9A, 0x65, 0xC5, 0xAE, 0x6E, 0xD0, 0x8A, 0xD2, 0x3F, 0x87, 0x14, 0x82, 0x4B, 0x95, 0x72, 0x64, 0xEA, 0xF6, 0x1B, 0x3F, 0x3B, 0xC5, 0xA6, 0x0B, 0x61, 0x81, 0x07, 0x38, 0xFF, 0x2E, 0x8B, 0xED, 0x60, 0x86, 0x78, 0xC8, 0x36, 0x08, 0x3C, 0xB8, 0x5A, 0x92, 0x97, 0x4C, 0x4C, 0x08, 0x11, 0x1F, 0x59, 0x1A, 0xA8, 0x00, 0xC8, 0x10, 0xC0, 0xD5, 0x7D, 0xA6, 0x86, 0x6C, 0xD6, 0x66, 0x42, 0xEF, 0x98, 0x5B, 0x5F, 0x25, 0x77, 0x91, 0xB3, 0x09, 0xFF, 0xCD, 0x2B, 0x26, 0x0B, 0x76, 0x43, 0xC6, 0x79, 0x76, 0xF4, 0x00, 0xF6, 0x54, 0xF4, 0xE6, 0x0C, 0xA7, 0x7D, 0x94, 0x59, 0xF6, 0x2B, 0x5F, 0x5A, 0xA7, 0x68, 0xEC, 0xA4, 0xD8, 0xA9, 0x25, 0xE0, 0x61, 0x23, 0xE0, 0x20, 0x76, 0xB3, 0x72, 0x74, 0x16, 0xC9, 0xA6, 0xAC, 0x63, 0x26, 0xFB, 0x2A, 0x16, 0x05, 0x33, 0xF6, 0x51, 0xC0, 0xE3, 0x39, 0x84, 0x2D, 0x4F, 0xC8, 0x7C, 0x40, 0xD7, 0x2F, 0xA5, 0x75, 0xFB, 0x4F, 0x13, 0x1C, 0x70, 0xA8, 0xAB, 0x92, 0x62, 0x72, 0xFD, 0xD4, 0x77, 0x09, 0x79, 0x64, 0xFE, 0x9E, 0xDD, 0xC2, 0x94, 0x17, 0x58, 0x5F, 0x33, 0x2E, 0x7F, 0x5C, 0x2D, 0x9E, 0xF4, 0x04, 0xCB, 0x5A, 0xA1, 0x89, 0x14, 0x59, 0xC1, 0xAA, 0x6B, 0x8F, 0x27, 0x0C, 0xA4, 0x5F, 0x89, 0x11, 0x83, 0x01, 0xAC, 0xD5, 0x3C, 0xFB, 0xA4, 0x4C, 0x9E, 0x6F, 0xAB, 0x76, 0x38, 0x26, 0x85, 0xB8, 0x79, 0xAF, 0x78, 0xEA, 0xCC, 0xA1, 0x41, 0x4D, 0xD8, 0xBF, 0xCF, 0xB9, 0x82, 0x5F, 0x62, 0x8A, 0xC8, 0xA2, 0xF6, 0xEF, 0x0F, 0x47, 0x8F, 0xC4, 0x03, 0x11, 0xA0, 0xE1, 0xC3, 0x25, 0xDD, 0x20, 0x75, 0x58, 0xAC, 0x43, 0x19, 0xD5, 0xCE, 0x57, 0x6C, 0x4A, 0xC7, 0xBD, 0x13, 0x15, 0xCB, 0xD6, 0x7C, 0x8A, 0xF1, 0x67, 0xEF, 0xCA, 0x32, 0xAD, 0xFB, 0x10, 0x44, 0x1E, 0xA7, 0x7A, 0x02, 0x1A, 0xA5, 0x41, 0xD4, 0xBF, 0x7B, 0x91, 0xD8, 0xE3, 0x1A, 0x18, 0x11, 0xD3, 0x0F, 0xE0, 0xE1, 0x4D, 0x75, 0x43, 0x3D, 0x70, 0x1B, 0x94, 0xCA, 0xFC, 0x03, 0x7F, 0x77, 0x31, 0xC1, 0x89, 0xDF, 0x3B, 0x5D, 0xEC, 0xAD, 0x57, 0x97, 0x61, 0x4C, 0x1E, 0x28, 0xD6, 0x41, 0x82, 0x12, 0xD2, 0x27, 0x40, 0x60, 0xCD, 0x2A, 0xFF, 0x65, 0xB0, 0xC0, 0x25, 0x1A, 0x60, 0xB2, 0xC6, 0x2D, 0xE3, 0x90, 0x2F, 0x79, 0x97, 0x8F, 0x41, 0x8C, 0x7D, 0x51, 0xFA, 0x56, 0xB0, 0xB5, 0x2D, 0x9D, 0xDE, 0x58, 0x4A, 0xE4, 0x43, 0x11, 0xD8, 0x8C, 0x38, 0xBE, 0x63, 0x21, 0xF7, 0x29, 0x24, 0x6F, 0x83, 0x17, 0xDC, 0xD5, 0x6E, 0x4B, 0xC4, 0x18, 0xA6, 0xB3, 0x15, 0x0D, 0x0B, 0x44, 0x70, 0x0A, 0xE6, 0x48 }.ToReadOnlyList());
            yield return new AsymmetricPrivateEncryptionKey(new byte[] { 0x07, 0x02, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x52, 0x53, 0x41, 0x32, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x47, 0xDD, 0xBE, 0xCB, 0x9A, 0x97, 0xB8, 0x03, 0xD7, 0x18, 0x9D, 0xAF, 0x9B, 0x58, 0x90, 0xE0, 0x7B, 0xC1, 0x48, 0x77, 0x3D, 0xFF, 0x04, 0xB2, 0x32, 0x71, 0xB1, 0xC8, 0x73, 0x28, 0xBC, 0x9D, 0xC6, 0xCB, 0x7A, 0x60, 0xAE, 0x81, 0xB3, 0xBE, 0xED, 0xC0, 0x71, 0x36, 0x4D, 0xFE, 0x50, 0xAA, 0xAD, 0x39, 0x8D, 0x55, 0x32, 0x32, 0x35, 0x66, 0xD6, 0x48, 0xA2, 0x23, 0x3B, 0x72, 0x73, 0x7F, 0x16, 0x58, 0x21, 0x4E, 0xBB, 0xBA, 0xA2, 0x9D, 0xB8, 0x55, 0x52, 0xCD, 0x26, 0x60, 0xFA, 0xCB, 0xB7, 0xB5, 0x74, 0xEC, 0xEF, 0x41, 0x51, 0x55, 0xA6, 0xC4, 0x64, 0x22, 0x8A, 0x71, 0x5A, 0x7F, 0x78, 0x1B, 0x55, 0x42, 0x40, 0xE2, 0xCB, 0xE2, 0xAA, 0xEE, 0xB0, 0xEA, 0xC0, 0x7B, 0x24, 0x48, 0xB6, 0xAD, 0x71, 0x45, 0xA5, 0x78, 0x06, 0x37, 0xDE, 0x3F, 0x7B, 0xB7, 0xAD, 0x5F, 0xA1, 0xCB, 0xED, 0x9C, 0x0E, 0x5A, 0x80, 0x82, 0x2E, 0x28, 0x7A, 0xF1, 0xE8, 0xF4, 0x28, 0x46, 0xFD, 0xF8, 0xEA, 0x09, 0xB3, 0xAD, 0x2E, 0x3F, 0x34, 0x06, 0xA3, 0x88, 0xEA, 0xB6, 0x72, 0xFA, 0x80, 0xE8, 0xD9, 0xE4, 0xEC, 0xFF, 0xE6, 0xFF, 0x56, 0x3E, 0x0B, 0x37, 0x5C, 0xB4, 0xDC, 0x15, 0x67, 0x12, 0xAF, 0x52, 0x54, 0x76, 0xE9, 0xD1, 0xFF, 0x09, 0xD8, 0x88, 0x81, 0xD2, 0x96, 0xFF, 0xD5, 0x85, 0xA2, 0xCC, 0x3A, 0x04, 0x1F, 0xC8, 0x1F, 0xB4, 0x9F, 0x8E, 0xD1, 0xC2, 0x59, 0x67, 0xC3, 0xA7, 0xD9, 0xC1, 0x6A, 0x14, 0x10, 0x6D, 0xA9, 0x98, 0xAC, 0x66, 0xE2, 0xE7, 0xBE, 0x7D, 0xF2, 0x64, 0x6C, 0x68, 0x9E, 0x60, 0xD9, 0x9D, 0x27, 0xFE, 0x75, 0xE1, 0xEC, 0xDD, 0xF2, 0x4D, 0xB3, 0x67, 0xE5, 0xEF, 0xB2, 0x8F, 0x8F, 0xA1, 0x90, 0xA5, 0xE1, 0x5F, 0xC8, 0x21, 0xF5, 0xFA, 0xD3, 0xD0, 0x2B, 0xAC, 0xB3, 0x6A, 0x31, 0x5F, 0xCA, 0xBD, 0x19, 0x7E, 0xDD, 0x23, 0x66, 0x4B, 0xCC, 0xBA, 0x52, 0xFB, 0x55, 0x35, 0x85, 0xC0, 0xA1, 0x14, 0xCE, 0x98, 0x3C, 0xCE, 0x65, 0xB2, 0x67, 0x31, 0xA2, 0x88, 0x01, 0x94, 0x22, 0x82, 0x67, 0x77, 0x27, 0xD1, 0xCA, 0x80, 0x64, 0xFF, 0xE6, 0xC8, 0xE9, 0x33, 0xF6, 0x0C, 0x2A, 0x7F, 0xDA, 0x20, 0x7C, 0x97, 0x4D, 0x50, 0x4A, 0xDF, 0xC5, 0x6D, 0xC6, 0x7A, 0x20, 0x95, 0x56, 0x2C, 0x6A, 0x24, 0xDA, 0x41, 0xD8, 0x91, 0x54, 0xCB, 0x48, 0xA6, 0xE5, 0xF7, 0x09, 0xCE, 0x6A, 0x80, 0x20, 0xB0, 0xF1, 0xEE, 0x1D, 0x8C, 0x07, 0xD9, 0xDA, 0x58, 0x80, 0x72, 0x7B, 0xD9, 0xDF, 0x5B, 0xEE, 0x18, 0x35, 0x87, 0xD2, 0x6C, 0x4A, 0x08, 0xFE, 0x64, 0x70, 0xCF, 0x63, 0xFE, 0x7B, 0x75, 0xD7, 0xF1, 0x70, 0xE6, 0x49, 0xAB, 0x88, 0x70, 0x92, 0xEF, 0x55, 0x99, 0x13, 0x1F, 0xBA, 0x8D, 0x87, 0xAB, 0xAC, 0x3B, 0xDA, 0xF2, 0x62, 0x46, 0x67, 0x9A, 0x52, 0xFE, 0x82, 0xA5, 0x07, 0x9C, 0x79, 0x4A, 0x3D, 0x8A, 0x71, 0x3D, 0x67, 0xA1, 0x70, 0x26, 0xE6, 0x04, 0xA9, 0xEB, 0xFD, 0xCD, 0x0B, 0x9E, 0x18, 0x26, 0x2C, 0x41, 0x68, 0x7A, 0xFE, 0x9E, 0x6A, 0xC9, 0x81, 0xC9, 0xD0, 0x5E, 0xE5, 0x05, 0x8B, 0x8C, 0xD5, 0xB2, 0x22, 0x3A, 0xC1, 0x56, 0x87, 0xD4, 0x69, 0x16, 0xD5, 0x8F, 0xD3, 0xAD, 0x9B, 0x03, 0xCD, 0x8D, 0x5E, 0x09, 0x24, 0x80, 0x40, 0xEA, 0x90, 0x65, 0x4D, 0xE7, 0xA3, 0xA0, 0x1A, 0xD8, 0x4D, 0xD0, 0x49, 0x06, 0x5C, 0x9A, 0x9A, 0x90, 0x98, 0xDD, 0xD9, 0x48, 0x7F, 0x04, 0x67, 0x4F, 0x39, 0x8D, 0x19, 0xF9, 0x83, 0xE9, 0x96, 0x59, 0xB7, 0xAF, 0x48, 0xFF, 0x09, 0x05, 0xF2, 0x6A, 0x1A, 0x3F, 0xAB, 0xD8, 0x25, 0xDF, 0x27, 0xFE, 0x38, 0x5E, 0xAD, 0x7B, 0xA2, 0x20, 0x02, 0x66, 0x6C, 0xC6, 0x71, 0xEF, 0x03, 0x79, 0x18, 0x38, 0x55, 0xB3, 0x54, 0xD7, 0x91, 0xA1, 0x22, 0x33, 0xAA, 0x64, 0x4D, 0x91, 0x39, 0xBD, 0xF4, 0xFD, 0x89, 0x5B, 0x9A, 0x05, 0x66, 0x29, 0xBC, 0xD7, 0x11, 0xB0, 0x15, 0x9F, 0x9C, 0x70, 0xE9, 0x98, 0x0F, 0x53, 0xA5, 0xF2, 0x69, 0x0D, 0x76, 0x13, 0x0B, 0x6D, 0xD4, 0xD6, 0x20, 0xB2, 0xF3, 0xCD, 0x27, 0x2E, 0xA1, 0x1C, 0x05, 0xD4, 0x20, 0x81, 0xC6, 0xA4, 0x4E, 0x19, 0xCB, 0xCE, 0xC9, 0xC5, 0xE2, 0xD3, 0xAD, 0xC0, 0x8A, 0x79, 0x4F, 0xBC, 0x24, 0x00, 0x34, 0xEC, 0x97, 0xAD, 0x71, 0xC5, 0x04, 0x07, 0x81, 0x24, 0x23, 0x8F, 0xBB, 0x1C, 0xB2, 0x64, 0xCE, 0x83, 0xC6, 0x5B, 0x38, 0xFC, 0xED, 0xA8, 0x24, 0x83, 0x6C, 0x1A, 0x5F, 0x7A, 0x9B, 0x48, 0xEB, 0xC0, 0x48, 0x2F, 0x79, 0xFA, 0x9C, 0x51, 0xAF, 0xB6, 0x9D, 0x65, 0xC1, 0x4C, 0xC4, 0x80, 0xCA, 0x72, 0x96, 0x58, 0xF1, 0x80, 0x5B, 0x52, 0x00, 0x7F, 0x75, 0x02, 0x0A, 0xC7, 0xB1, 0x19, 0xE9, 0xBA, 0xAD, 0xA7, 0xC9, 0xD9, 0xB8, 0xA7, 0x38, 0xBE, 0x3B, 0xBB, 0x00, 0x0A, 0xD3, 0x6D, 0x40, 0xB3, 0x0A, 0xC5, 0x72, 0x4A, 0x20, 0xC6, 0x2E, 0x3E, 0x03, 0x58, 0x21, 0x77, 0xA5, 0x6E, 0xDF, 0x2D, 0xCE, 0x4D, 0xBD, 0xDF, 0x3B, 0x8C, 0x52, 0x1D, 0x71, 0xDE, 0x9A, 0x0B, 0x23, 0x9F, 0x15, 0xE5, 0x11, 0x39, 0x84, 0x17, 0x20, 0x70, 0x17, 0x8C, 0x1D, 0x62, 0xA4, 0xFB, 0x7B, 0xA0, 0x5F, 0xA6, 0xD3, 0x2B, 0x63, 0x94, 0xAF, 0x6C, 0xB2, 0x1C, 0x75, 0x7E, 0x38, 0x25, 0x1C, 0xBE, 0xD1, 0x6D, 0x2D, 0x93, 0x4A, 0xB1, 0x9F, 0x25, 0x44, 0xFE, 0x27, 0x9D, 0x33, 0x0F, 0xE9, 0x93, 0xEE, 0x4C, 0x4E, 0x13, 0x57, 0xC6, 0x07, 0xF1, 0x1F, 0xDA, 0xFB, 0x08, 0x5F, 0xD8, 0x75, 0x1D, 0x2D, 0x81, 0xFB, 0x92, 0x1A, 0x5B, 0x43, 0x94, 0x8F, 0x70, 0xD0, 0x7F, 0x0C, 0xA3, 0x25, 0xE3, 0x8B, 0x66, 0x0B, 0x9C, 0x43, 0xEB, 0x72, 0x42, 0xB2, 0xB1, 0x14, 0xF1, 0x73, 0x6C, 0x89, 0xDC, 0x6B, 0x63, 0x78, 0xE8, 0x17, 0xA4, 0x9B, 0x9D, 0x21, 0x92, 0xA6, 0x53, 0x84, 0x8C, 0x1A, 0x3E, 0x73, 0x34, 0x47, 0x58, 0x61, 0xB3, 0x33, 0xDA, 0x48, 0xDA, 0x9D, 0x18, 0x9F, 0x27, 0x48, 0xA6, 0xC9, 0x42, 0x08, 0xA6, 0xBD, 0xCF, 0x22, 0xAE, 0x12, 0x99, 0x43, 0x27, 0xB3, 0x12, 0xC5, 0x05, 0x05, 0x06, 0x16, 0x7E, 0x2C, 0xEB, 0x74, 0xE5, 0x67, 0xDC, 0xCD, 0x09, 0x8A, 0xE9, 0x95, 0x38, 0x52, 0x80, 0x3C, 0x6E, 0x6C, 0x8F, 0xEE, 0xAA, 0x18, 0x90, 0x5E, 0x10, 0x93, 0xCE, 0x5D, 0x9E, 0x27, 0xCD, 0xA3, 0x1F, 0x31, 0xCD, 0xB8, 0xAC, 0x73, 0xDC, 0x0D, 0xDA, 0xA7, 0x42, 0x4B, 0xC3, 0x35, 0x87, 0xB1, 0x17, 0xC4, 0x25, 0x73, 0x94, 0xDF, 0x81, 0x52, 0xE7, 0xAC, 0x4B, 0xDF, 0x38, 0xE9, 0x9F, 0x00, 0x1B, 0xCD, 0xB9, 0x0C, 0x25, 0x1B, 0x30, 0xC1, 0x58, 0x4A, 0x24, 0x4E, 0x39, 0x51, 0xB4, 0xEA, 0xE2, 0xC7, 0xF9, 0x83, 0x20, 0x4A, 0xE4, 0x1A, 0xAC, 0x85, 0x21, 0xD6, 0x00, 0x1D, 0x3E, 0x75, 0x96, 0xD5, 0xD7, 0xEE, 0xD4, 0xEF, 0xF9, 0xAC, 0xAA, 0xAD, 0xD9, 0x52, 0xAF, 0x49, 0xE6, 0xBA, 0x18, 0x86, 0x1B, 0x0A, 0x19, 0xA5, 0x24, 0x23, 0x22, 0xC3, 0xD7, 0xB0, 0x41, 0x57, 0xC2, 0x43, 0xD5, 0x41, 0xDC, 0xE1, 0x6B, 0x9A, 0x3A, 0x4B, 0x37, 0x13, 0x5E, 0x60, 0x61, 0xCF, 0xAD, 0x9E, 0xB2, 0x57, 0xD0, 0x00, 0x1C, 0xC7, 0x7B, 0x33, 0xE6, 0xD5, 0xDF, 0xDA, 0xFB, 0x86, 0xE6, 0x51, 0x1E, 0x22, 0x4E, 0x36, 0x4E, 0x39, 0xDB, 0xAF, 0x27, 0x14, 0xA0, 0xE6, 0x92, 0xB8, 0x6E, 0x7E, 0xB8, 0x08, 0x64, 0x2D, 0x9D, 0x46, 0x47, 0x07, 0x90, 0x68, 0x5A, 0x46, 0x79, 0x75, 0x84, 0xCB, 0x7B, 0xEE, 0x93, 0xD5, 0xEE, 0xB2, 0x2E, 0x70, 0x48, 0x52, 0x7C, 0xF8, 0x64, 0x08, 0xB0, 0x94, 0xB8, 0x0D, 0x15, 0xDA, 0xBE, 0xBC, 0x2B, 0x13, 0x06, 0xA4, 0x6B, 0xF5, 0xB2, 0x1F, 0x06, 0x64, 0x8B, 0x8A, 0xC1, 0x42, 0x69, 0x06, 0x2B, 0x93, 0x64, 0xB8, 0x7F, 0x0B, 0x7F, 0x2F, 0x45, 0xED, 0xB2, 0x89, 0x5F, 0x84, 0x11, 0x8C, 0x44, 0x0F, 0xE2, 0x76, 0x5F, 0x71, 0x78, 0xA5, 0x45, 0xC1, 0x4A, 0xEE, 0xDD, 0xBB, 0x62, 0x9B, 0x56, 0xFC, 0x40, 0xF7, 0x56, 0xB2, 0xBC, 0xBE, 0x71, 0xBD, 0x49, 0x1D, 0xFE, 0x46, 0xD7, 0x4C, 0xA8, 0xF6, 0x29, 0x2D, 0x4D, 0xC7, 0x33, 0x73, 0x2B, 0x27, 0xBF }.ToReadOnlyList());
            yield return new AsymmetricPrivateEncryptionKey(new byte[] { 0x07, 0x02, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x52, 0x53, 0x41, 0x32, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x1D, 0x16, 0x66, 0x58, 0x65, 0x60, 0x0C, 0xB8, 0xBC, 0x3C, 0x43, 0x2C, 0x4A, 0xFF, 0xD6, 0x96, 0x16, 0x74, 0x6B, 0x7D, 0xF4, 0x6C, 0x97, 0x42, 0xAD, 0xA1, 0x18, 0x97, 0x51, 0x66, 0x90, 0xE3, 0x3C, 0xE9, 0xA9, 0x4A, 0x6E, 0xB3, 0xC6, 0x41, 0x77, 0x72, 0x68, 0x7E, 0x6E, 0xA0, 0x79, 0x57, 0xD2, 0xEC, 0xD5, 0xA7, 0x4E, 0x79, 0x20, 0x2D, 0x73, 0x0E, 0xD9, 0xA6, 0x6D, 0x10, 0x4A, 0x27, 0x1A, 0xB1, 0x84, 0x81, 0xE8, 0x87, 0x78, 0x26, 0x5B, 0xE6, 0x52, 0x00, 0x9B, 0xA9, 0x62, 0xD5, 0x35, 0x8C, 0x19, 0x68, 0xDC, 0x17, 0xF4, 0x57, 0x80, 0x7A, 0x98, 0xDA, 0x9C, 0xD5, 0x62, 0xF2, 0x9D, 0x22, 0x57, 0x6D, 0xCF, 0x2E, 0xFE, 0x61, 0xCC, 0x54, 0xD3, 0xE6, 0x4E, 0xCF, 0x3C, 0x3B, 0x53, 0x20, 0x41, 0x84, 0xC0, 0x04, 0x5D, 0xB8, 0xE7, 0xC4, 0x06, 0xC5, 0x8E, 0x56, 0x07, 0x8D, 0x5F, 0xAE, 0x07, 0x52, 0xAC, 0xA9, 0x40, 0xA5, 0x19, 0x7B, 0xFF, 0x96, 0xE0, 0x89, 0xAE, 0xCF, 0xA1, 0x27, 0x97, 0x46, 0xD2, 0x78, 0x89, 0x7D, 0xCF, 0x85, 0x17, 0x9C, 0xA6, 0x13, 0x1F, 0xD6, 0xF7, 0x89, 0x00, 0xA3, 0x7B, 0x5C, 0x67, 0xE2, 0xD7, 0xB6, 0xD7, 0x84, 0x2C, 0x17, 0x77, 0x02, 0x09, 0xD8, 0x5A, 0xC9, 0xBB, 0xB3, 0x87, 0xFD, 0x72, 0x75, 0x94, 0x8D, 0xC8, 0x5C, 0xD2, 0x10, 0x51, 0x0A, 0x7C, 0xD7, 0xE9, 0x53, 0x20, 0x03, 0xCA, 0x21, 0xC7, 0xB4, 0xC4, 0x45, 0x9C, 0x54, 0x92, 0x8B, 0xD3, 0x82, 0x0D, 0x7D, 0xEF, 0xB8, 0x13, 0xDA, 0x24, 0x8B, 0x03, 0xFE, 0x1E, 0x36, 0x20, 0x4B, 0x18, 0x24, 0x74, 0x51, 0x27, 0xE8, 0x0B, 0x12, 0xB8, 0x14, 0xDB, 0xC5, 0x70, 0x27, 0x7F, 0x62, 0x79, 0xE8, 0x75, 0xAC, 0xB0, 0x09, 0xDF, 0xBE, 0x1F, 0x5D, 0x64, 0xA9, 0x5B, 0xC1, 0xEB, 0x49, 0x12, 0xA7, 0x8A, 0xFB, 0xFA, 0x8F, 0xE0, 0x26, 0x53, 0x25, 0x99, 0xB7, 0x5F, 0x04, 0xD6, 0x8E, 0x33, 0xBD, 0xFC, 0x6E, 0xB3, 0x04, 0x44, 0x71, 0xFC, 0x33, 0x84, 0x86, 0xA6, 0x8B, 0x86, 0x45, 0x96, 0x67, 0x05, 0x08, 0x41, 0x2B, 0x4A, 0x86, 0x37, 0xC9, 0x48, 0xD8, 0x52, 0xE1, 0x22, 0x54, 0xF2, 0xFB, 0xCF, 0x18, 0x28, 0x8B, 0x04, 0x40, 0x5B, 0xC9, 0xD8, 0xDC, 0x8F, 0x7F, 0x67, 0xE0, 0xD0, 0x19, 0x4D, 0x41, 0xB9, 0x6B, 0x35, 0x0D, 0x62, 0x67, 0x51, 0x0D, 0x6E, 0x26, 0xE2, 0x15, 0x42, 0x92, 0xA1, 0xD3, 0xE5, 0x99, 0x11, 0x25, 0xD5, 0xD0, 0x9D, 0x81, 0xFF, 0xCD, 0x70, 0x71, 0x6E, 0x36, 0xBB, 0xA4, 0x82, 0xD5, 0x3A, 0xD3, 0x86, 0x26, 0x32, 0x49, 0xCC, 0xB9, 0x3D, 0x56, 0x78, 0x17, 0x89, 0xC5, 0x0E, 0x55, 0x58, 0x7B, 0xC6, 0x3C, 0xD2, 0x9C, 0xDC, 0xE5, 0x17, 0xD6, 0x25, 0xD7, 0x2A, 0x77, 0xC9, 0x2E, 0x1D, 0xDD, 0x61, 0x41, 0x3D, 0x05, 0x9E, 0x48, 0xCB, 0x79, 0x8D, 0xB6, 0x4A, 0xF1, 0x2F, 0x8A, 0xA1, 0x10, 0x20, 0xEB, 0x5F, 0x1B, 0x88, 0xBE, 0xA7, 0xA2, 0x71, 0x46, 0x4C, 0xB7, 0x30, 0xE0, 0x16, 0x0A, 0x6B, 0x77, 0x57, 0x85, 0xE5, 0xF2, 0x4B, 0x96, 0x10, 0x85, 0x17, 0x9C, 0x9D, 0x91, 0xF5, 0xCC, 0x9C, 0x45, 0xE5, 0xEA, 0xCC, 0xEC, 0x1C, 0x0A, 0x56, 0xED, 0x62, 0x4B, 0x5C, 0x99, 0xB5, 0xF7, 0xFE, 0xAC, 0xC5, 0x6E, 0xDA, 0x19, 0x9B, 0x4F, 0xD3, 0xEF, 0x69, 0x4E, 0x21, 0xA8, 0x1C, 0x0B, 0x0B, 0x93, 0x5B, 0xE8, 0xC7, 0xC3, 0x6F, 0xFD, 0x5F, 0xDF, 0x67, 0x00, 0x9E, 0xE2, 0xC9, 0xB9, 0x2E, 0xB4, 0xBF, 0x62, 0x0C, 0xDA, 0x0C, 0xBD, 0x82, 0xDA, 0x27, 0x1D, 0x12, 0x88, 0x62, 0x63, 0x1B, 0xF1, 0x85, 0x68, 0x58, 0xD7, 0xF1, 0xB9, 0xC5, 0x14, 0xE6, 0x68, 0x28, 0x4E, 0xC2, 0xE6, 0x99, 0xB8, 0x75, 0x70, 0x5A, 0x10, 0x23, 0x0A, 0xED, 0x4B, 0xB7, 0x4C, 0xAB, 0xAB, 0x50, 0xAD, 0x2D, 0x39, 0x0D, 0xB8, 0x2C, 0xBE, 0xE6, 0x67, 0xC0, 0xA8, 0x36, 0x17, 0x55, 0x09, 0x9D, 0xB1, 0x14, 0xF8, 0x5A, 0xAE, 0x61, 0x65, 0xDF, 0xD6, 0x83, 0x32, 0xBC, 0x7E, 0xC9, 0xCA, 0xD3, 0xBA, 0x3C, 0xC3, 0xC6, 0x30, 0x36, 0x91, 0x3F, 0xE1, 0xCE, 0x66, 0xBD, 0x7C, 0xD9, 0x5E, 0xE9, 0x7F, 0x9B, 0x94, 0x5E, 0xD2, 0x55, 0xD3, 0x2E, 0xAB, 0x26, 0xB3, 0x1F, 0xC3, 0xCF, 0x25, 0x34, 0x17, 0x57, 0xB7, 0x4A, 0xF9, 0x8E, 0x10, 0x48, 0x24, 0xFF, 0x38, 0xBF, 0x10, 0xE0, 0x6A, 0x12, 0x05, 0xBC, 0x60, 0x56, 0x4D, 0x6A, 0x6E, 0x6E, 0xE4, 0xAF, 0x0E, 0x2F, 0x81, 0xC5, 0xA9, 0xB0, 0xE5, 0x55, 0xE5, 0x15, 0x6C, 0x95, 0x7F, 0x73, 0xD8, 0xC6, 0x47, 0x1D, 0xAA, 0x40, 0xD1, 0x11, 0x6F, 0x0F, 0x12, 0x25, 0xF3, 0x21, 0xF4, 0x09, 0xD8, 0x3E, 0x2A, 0xDF, 0x88, 0x6A, 0xF0, 0xD1, 0x38, 0xB1, 0xAC, 0x7E, 0x54, 0x83, 0x02, 0x5B, 0x18, 0x6F, 0xD9, 0x1E, 0x27, 0x85, 0xA9, 0x37, 0x95, 0x08, 0x79, 0xFB, 0xFD, 0x42, 0x34, 0x88, 0x11, 0x82, 0x06, 0x55, 0x1D, 0x1C, 0x19, 0x23, 0xFE, 0x3A, 0xCD, 0xFD, 0x00, 0x16, 0xCB, 0xC9, 0x97, 0x50, 0x12, 0x2B, 0x90, 0x01, 0xAC, 0xF3, 0xF2, 0x30, 0x8A, 0x28, 0xFA, 0x9A, 0x8D, 0xAE, 0xD9, 0x26, 0x08, 0xDA, 0xCD, 0x9B, 0xB2, 0x4F, 0xEE, 0x01, 0x38, 0x64, 0x06, 0xBE, 0xD0, 0xB5, 0x63, 0xEE, 0x0C, 0x62, 0x8B, 0xFB, 0x78, 0xA7, 0xCF, 0xDE, 0xBC, 0x13, 0x23, 0x11, 0x11, 0x95, 0xBA, 0x2A, 0xCD, 0x7A, 0x36, 0x4B, 0x0C, 0x16, 0x8B, 0x92, 0x3A, 0xE0, 0x81, 0x9F, 0x28, 0x34, 0x19, 0x65, 0x4F, 0x44, 0xDA, 0x35, 0x55, 0x4F, 0x75, 0x3D, 0x65, 0x69, 0x50, 0xFB, 0x2D, 0x44, 0xF5, 0x4A, 0xF0, 0xD1, 0x3F, 0x64, 0xB1, 0xAB, 0x73, 0x40, 0x88, 0xE2, 0x27, 0x46, 0x7B, 0x38, 0x93, 0x35, 0xAF, 0x31, 0x10, 0x2D, 0xB0, 0x77, 0x81, 0xC1, 0x3E, 0x3F, 0xE4, 0x49, 0xB4, 0xD7, 0x45, 0x96, 0x6F, 0xFF, 0x6F, 0xFE, 0xF2, 0xC0, 0xEF, 0x8C, 0xFA, 0x6B, 0x46, 0x8D, 0x38, 0x1E, 0x90, 0x20, 0x5F, 0xB1, 0xF9, 0xC8, 0x1D, 0xB5, 0x15, 0x70, 0xC2, 0x12, 0xDF, 0x01, 0xE6, 0x56, 0xE6, 0xD7, 0x52, 0x1F, 0x3B, 0xC7, 0x20, 0x4B, 0x68, 0x20, 0x92, 0xA5, 0x6F, 0xD1, 0x16, 0xB9, 0x46, 0xF0, 0xD0, 0x39, 0x47, 0x80, 0xFD, 0x15, 0x4F, 0x19, 0x69, 0x83, 0xF9, 0x04, 0x14, 0x67, 0x0C, 0x02, 0xBB, 0x64, 0xA1, 0x40, 0x33, 0xA3, 0x29, 0x58, 0x3C, 0x6C, 0x87, 0x9C, 0x9C, 0xC5, 0x8B, 0x56, 0xC2, 0x92, 0x0C, 0x87, 0xB0, 0x63, 0x2C, 0x5C, 0xBA, 0xC9, 0x88, 0xBB, 0x30, 0x45, 0x6C, 0xB9, 0xD9, 0x2F, 0x4F, 0x1A, 0x82, 0xEC, 0x6F, 0xF9, 0x0F, 0xD3, 0x1A, 0x3B, 0x38, 0x4E, 0xCD, 0xA7, 0x87, 0xEF, 0x0E, 0x8A, 0xCE, 0x92, 0x8B, 0x27, 0xF2, 0x3F, 0x96, 0x75, 0xEB, 0x8F, 0xBA, 0xC9, 0xB0, 0x28, 0x50, 0x6E, 0xAF, 0x95, 0x36, 0xC1, 0xB3, 0x77, 0x97, 0xAE, 0x82, 0x86, 0x7E, 0x86, 0xD6, 0x65, 0x61, 0xE5, 0xD8, 0xBD, 0x75, 0x8E, 0x37, 0x7C, 0x7A, 0x22, 0xE3, 0x5F, 0xBF, 0x83, 0x17, 0x99, 0x2D, 0xB2, 0x58, 0x70, 0x44, 0x1B, 0x45, 0x6F, 0x65, 0x13, 0xDF, 0x19, 0x0C, 0xFD, 0xF5, 0x1E, 0xB0, 0xFF, 0xD5, 0x6D, 0xAA, 0x09, 0xB8, 0x13, 0x46, 0x29, 0x3F, 0x61, 0x4D, 0xC0, 0x89, 0xA0, 0x15, 0x3F, 0xD3, 0xDC, 0xBA, 0xB3, 0xA6, 0xBE, 0x93, 0xBC, 0x89, 0x53, 0x40, 0x3E, 0xFC, 0x7E, 0x84, 0xCD, 0xD5, 0xDD, 0xCC, 0x76, 0x12, 0xFD, 0x8D, 0xCB, 0xA6, 0xFD, 0x8A, 0x0E, 0x5A, 0x0F, 0x18, 0x52, 0x34, 0x13, 0xA1, 0x5C, 0x96, 0xF9, 0xBD, 0x4B, 0xEE, 0x9F, 0x60, 0xFD, 0xDA, 0xA9, 0x4A, 0xA3, 0x3C, 0x64, 0xE0, 0x22, 0x34, 0xAE, 0x1C, 0x6F, 0xA3, 0xF7, 0x0E, 0xC7, 0xD2, 0x0B, 0x8B, 0x42, 0x87, 0xB9, 0x9D, 0xEA, 0xD6, 0xB0, 0x5A, 0x14, 0x55, 0x4C, 0x89, 0x28, 0x9E, 0x35, 0x0F, 0x8A, 0xD0, 0x88, 0xF0, 0x1F, 0x46, 0x9E, 0x49, 0xB8, 0x7B, 0x53, 0x57, 0xDF, 0x1D, 0xE1, 0x3B, 0xEB, 0x76, 0x4A, 0x3D, 0x78, 0xC4, 0x9F, 0x22, 0x89, 0x14, 0x25, 0xA6, 0xCF, 0x09, 0x94, 0x3E, 0x36, 0x76, 0xD9, 0x25, 0xCA, 0xFE, 0xA9, 0xE5, 0x71, 0x73, 0xC2, 0xCA, 0x66, 0xFC, 0x5E, 0xC2, 0xB5, 0x9D, 0x17, 0xB2, 0x3C, 0x92, 0x5B, 0xB9, 0xA7, 0xBC }.ToReadOnlyList());
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
        }
    }
}
