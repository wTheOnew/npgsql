﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TlsClientStream
{
    internal class MD5SHA1 : HashAlgorithm
    {
        private MD5CryptoServiceProvider _md5;
        private SHA1CryptoServiceProvider _sha1;

        public MD5SHA1()
        {
            _md5 = new MD5CryptoServiceProvider();
            _sha1 = new SHA1CryptoServiceProvider();
        }

        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Security.Cryptography.HashAlgorithm
        //     and optionally releases the managed resources.
        //
        // Parameters:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to release only
        //     unmanaged resources.
        protected override void Dispose(bool disposing)
        {
            _md5.Dispose();
            _sha1.Dispose();
            if (HashValue != null)
                Array.Clear(HashValue, 0, 36);
            HashValue = null;
        }
        //
        // Summary:
        //     When overridden in a derived class, routes data written to the object into
        //     the hash algorithm for computing the hash.
        //
        // Parameters:
        //   array:
        //     The input to compute the hash code for.
        //
        //   ibStart:
        //     The offset into the byte array from which to begin using data.
        //
        //   cbSize:
        //     The number of bytes in the byte array to use as data.
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _md5.TransformBlock(array, ibStart, cbSize);
            _sha1.TransformBlock(array, ibStart, cbSize);
        }
        //
        // Summary:
        //     When overridden in a derived class, finalizes the hash computation after
        //     the last data is processed by the cryptographic stream object.
        //
        // Returns:
        //     The computed hash code.
        protected override byte[] HashFinal()
        {
            byte[] hash = new byte[36];
            _md5.TransformFinalBlock(hash, 0, 0);
            _sha1.TransformFinalBlock(hash, 0, 0);
            var md5Hash = _md5.Hash;
            var sha1Hash = _sha1.Hash;
            Buffer.BlockCopy(md5Hash, 0, hash, 0, 16);
            Buffer.BlockCopy(sha1Hash, 0, hash, 16, 20);
            Array.Clear(md5Hash, 0, 16);
            Array.Clear(sha1Hash, 0, 20);
            HashValue = hash;
            return hash;
        }
        //
        // Summary:
        //     Initializes an implementation of the System.Security.Cryptography.HashAlgorithm
        //     class.
        public override void Initialize()
        {
            _md5.Initialize();
            _sha1.Initialize();
        }
    }
}
