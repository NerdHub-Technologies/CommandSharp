/* DataConverter.cs
 * ----------------
 * PROJECT: CommandSharp
 * AUTHOR: WinMister332
 * LICENSE: MIT (https://opensource.org/licenses/MIT)
 * ----------------
 * NOTICE:
 *      You must include a copy of "license.txt" if you use CommandSharp. If you're using code pulled from the repo, you must also include this header.
 * ----------------
 * Copyright (c) 2017-2021 NerdHub Technologies, All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandSharp.Commands
{
    /// <summary>
    /// Handles converting Bytes into KiB, MiB, GiB, or TiB which are factors of 1024.
    /// KB, MB, GB, and TB are not yet supported and they're in factors of 1000.
    /// </summary>
    internal static class DataConverter
    {
        internal abstract class Data
        {
            private long data;

            public Data(long data)
                => this.data = data;

            internal long GetData() => data;
        }

        /// <summary>
        /// Handles conversion from a byte to KiB, MiB, GiB, or TiB.
        /// </summary>
        public sealed class Byte : Data
        {
            Byte(long bytes) : base(bytes) { }

            /// <summary>
            /// Convert byte to Kibibyte.
            /// </summary>
            /// <returns>Kibibyte</returns>
            public Kibibyte ToKibibyte()
                => Kibibyte.Create(GetData() / 1024);

            /// <summary>
            /// Convert byte to Mebibyte.
            /// </summary>
            /// <returns>Mebibyte</returns>
            public Mebibyte ToMebibyte()
                => ToKibibyte().ToMebibyte();

            /// <summary>
            /// Convert byte to Gibibyte.
            /// </summary>
            /// <returns>Gibibyte</returns>
            public Gibibyte ToGibibyte()
                => ToMebibyte().ToGibibyte();

            /// <summary>
            /// Convert byte to Tebibyte.
            /// </summary>
            /// <returns>Tebibyte</returns>
            public Tebibyte ToTebibyte()
                => ToGibibyte().ToTebibyte();

            /// <summary>
            /// The mathmatical result of the conversion in factor of 1024 as a string.
            /// </summary>
            /// <returns>The result in factor of 1024</returns>
            public override string ToString()
                => $"{GetData()} B";

            /// <summary>
            /// Create a new instance of Byte with a base value.
            /// </summary>
            /// <param name="kibibytes">The base value in bytes.</param>
            /// <returns>Byte</returns>
            public static Byte Create(long bytes)
                => new Byte(bytes);
        }

        /// <summary>
        /// Handles conversion from a Kibibyte to KiB, MiB, GiB, or TiB.
        /// </summary>
        public sealed class Kibibyte : Data
        {
            Kibibyte(long bytes) : base(bytes) { }

            /// <summary>
            /// Convert Kibibyte to Byte.
            /// </summary>
            /// <returns>Byte.</returns>
            public Byte ToByte()
                => Byte.Create(GetData() * 1024);

            /// <summary>
            /// Convert Kibibyte to Mebibyte.
            /// </summary>
            /// <returns>Mebibyte</returns>
            public Mebibyte ToMebibyte()
                => Mebibyte.Create(GetData() / 1024);

            /// <summary>
            /// Convert Kibibyte to Gibibyte
            /// </summary>
            /// <returns>Gibibyte</returns>
            public Gibibyte ToGibibyte()
                => ToMebibyte().ToGibibyte();

            /// <summary>
            /// Convert Kibibyte to Tebibyte.
            /// </summary>
            /// <returns>Tebibyte</returns>
            public Tebibyte ToTebibyte()
                => ToGibibyte().ToTebibyte();

            /// <summary>
            /// The mathmatical result of the conversion in factor of 1024 as a string.
            /// </summary>
            /// <returns>The result in factor of 1024</returns>
            public override string ToString()
                => $"{GetData()} KiB";

            /// <summary>
            /// Create a new instance of Kibibyte with a base value.
            /// </summary>
            /// <param name="kibibytes">The base value in kibibytes.</param>
            /// <returns>Kibibyte</returns>
            public static Kibibyte Create(long kibibytes)
                => new Kibibyte(kibibytes);
        }

        /// <summary>
        /// Handles conversion from a Mebibyte to KiB, MiB, GiB, or TiB.
        /// </summary>
        public sealed class Mebibyte : Data
        {
            Mebibyte(long bytes) : base(bytes) { }

            /// <summary>
            /// Convert Mebibyte to Byte.
            /// </summary>
            /// <returns>Byte.</returns>
            public Byte ToByte()
                => ToKibibyte().ToByte();

            /// <summary>
            /// Convert Mebibyte to Kibibyte.
            /// </summary>
            /// <returns>Kibibyte</returns>
            public Kibibyte ToKibibyte()
                => Kibibyte.Create(GetData() * 1024);

            /// <summary>
            /// Convert Mebibyte to Gibibyte.
            /// </summary>
            /// <returns>Gibibyte</returns>
            public Gibibyte ToGibibyte()
                => Gibibyte.Create(GetData() / 1024);

            /// <summary>
            /// Convert Mebibyte to Tebibyte.
            /// </summary>
            /// <returns>Tebibyte</returns>
            public Tebibyte ToTebibyte()
                => ToGibibyte().ToTebibyte();

            /// <summary>
            /// The mathmatical result of the conversion in factor of 1024 as a string.
            /// </summary>
            /// <returns>The result in factor of 1024</returns>
            public override string ToString()
                => $"{GetData()} MiB";

            /// <summary>
            /// Create a new instance of Mebibyte with a base value.
            /// </summary>
            /// <param name="kibibytes">The base value in Mebibytes.</param>
            /// <returns>Mebibyte</returns>
            public static Mebibyte Create(long mebibyte)
                => new Mebibyte(mebibyte);
        }

        /// <summary>
        /// Handles conversion from a Gibibyte to KiB, MiB, GiB, or TiB.
        /// </summary>
        public sealed class Gibibyte : Data
        {
            Gibibyte(long bytes) : base(bytes) { }

            /// <summary>
            /// Convert Gibibyte to Byte.
            /// </summary>
            /// <returns>Byte.</returns>
            public Byte ToByte()
                => ToKibibyte().ToByte();

            /// <summary>
            /// Convert Gibibyte to Kibibyte.
            /// </summary>
            /// <returns>Kibibyte</returns>
            public Kibibyte ToKibibyte()
                => ToMebibyte().ToKibibyte();

            /// <summary>
            /// Convert Gibibyte to Mebibyte.
            /// </summary>
            /// <returns>Mebibyte</returns>
            public Mebibyte ToMebibyte()
                => Mebibyte.Create(GetData() * 1024);

            /// <summary>
            /// Convert Gibibyte to Tebibyte.
            /// </summary>
            /// <returns>Tebibyte</returns>
            public Tebibyte ToTebibyte()
                => Tebibyte.Create(GetData() / 1024);

            /// <summary>
            /// The mathmatical result of the conversion in factor of 1024 as a string.
            /// </summary>
            /// <returns>The result in factor of 1024</returns>
            public override string ToString()
                => $"{GetData()} GiB";

            /// <summary>
            /// Create a new instance of Gibibyte with a base value.
            /// </summary>
            /// <param name="kibibytes">The base value in Gibibytes.</param>
            /// <returns>Gibibyte</returns>
            public static Gibibyte Create(long gibibyte)
                => new Gibibyte(gibibyte);
        }

        /// <summary>
        /// Handles conversion from a Tebibyte to KiB, MiB, GiB, or TiB.
        /// </summary>
        public sealed class Tebibyte : Data
        {
            Tebibyte(long bytes) : base(bytes) { }

            /// <summary>
            /// Convert Tebibyte to Byte.
            /// </summary>
            /// <returns>Byte.</returns>
            public Byte ToByte()
                => ToKibibyte().ToByte();

            /// <summary>
            /// Convert Tebibyte to Kibibyte.
            /// </summary>
            /// <returns>Kibibyte.</returns>
            public Kibibyte ToKibibyte()
                => ToMebibyte().ToKibibyte();

            /// <summary>
            /// Convert Tebibyte to Mebibyte.
            /// </summary>
            /// <returns>Mebibyte.</returns>
            public Mebibyte ToMebibyte()
                => ToGibibyte().ToMebibyte();

            /// <summary>
            /// Convert Tebibyte to Gibibyte.
            /// </summary>
            /// <returns>Gibibyte.</returns>
            public Gibibyte ToGibibyte()
                => Gibibyte.Create(GetData() * 1024);

            /// <summary>
            /// The mathmatical result of the conversion in factor of 1024 as a string.
            /// </summary>
            /// <returns>The result in factor of 1024</returns>
            public override string ToString()
                => $"{GetData()} TiB";

            /// <summary>
            /// Create a new instance of Tibibyte with a base value.
            /// </summary>
            /// <param name="kibibytes">The base value in Tibibytes.</param>
            /// <returns>Tibibyte</returns>
            public static Tebibyte Create(long tebibyte)
                => new Tebibyte(tebibyte);
        }
    }
}
