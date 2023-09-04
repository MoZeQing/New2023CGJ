using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace GameMain
{
    public static class BinaryReaderExtension
    {
        public static List<NodeTag> ReadListNodeTag(this BinaryReader binaryReader)
        {
            List<NodeTag> temp = new List<NodeTag>();
            float count = binaryReader.ReadSingle();
            for (int i = 0; i < count; i++)
            {
                temp.Add((NodeTag)binaryReader.ReadByte());
            }
            return temp;
        }
        public static NodeTag ReadNodeTag(this BinaryReader binaryReader)
        {
               return (NodeTag)binaryReader.Read();
        }
        public static List<String> ReadListString(this BinaryReader binaryReader)
        {
            List<String> temp = new List<String>();
            float count = binaryReader.ReadSingle();
            for (int i = 0; i < count; i++)
            {
                temp.Add(binaryReader.ReadString());
            }
            return temp;
        }
    }
}

