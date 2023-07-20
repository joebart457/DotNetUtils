using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenizerCore.Models.Constants
{
    internal static class BuiltinTokenTypes
    {
        public const string Space = "Space";
        public const string Tab = "Tab";
        public const string CarriageReturn = "CarriageReturn";
        public const string LineFeed = "LineFeed";

        public const string Word = "TTWord";
        public const string String = "TTString";
        public const string Integer = "TTInteger";
        public const string UnsignedInteger = "TTUnsignedInteger";
        public const string Double = "TTDouble";
        public const string Float = "TTFloat";

        public const string EndOfLineComment = "EndOfLineComment";
        public const string MultiLineComment = "MultiLineComment";

        public const string EndOfFile = "EndOfFile";
    }
}
