using System;
using System.Collections.Generic;
using System.IO;
using static GameMain.Editor.DataTableTools.DataTableProcessor;

namespace GameMain.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class NodeTagProcessor : GenericDataProcessor<NodeTag>
        {
            //是否是系统自带的类型
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// 定义的类型名称
            /// </summary>
            public override string LanguageKeyword
            {
                get
                {
                    return "NodeTag";
                }
            }

            /// <summary>
            /// 定义的类型名称
            /// </summary>
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "NodeTag",
                };
            }

            /// <summary>
            /// 解析输入的值，返回定义的类型值
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public override NodeTag Parse(string value)
            {
                return (NodeTag)System.Enum.Parse(typeof(NodeTag), value);
            }

            /// <summary>
            /// 写入二进制流
            /// </summary>
            /// <param name="dataTableProcessor"></param>
            /// <param name="binaryWriter"></param>
            /// <param name="value"></param>
            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                    binaryWriter.Write((float)Parse(value));
            }
        }
    }
}
