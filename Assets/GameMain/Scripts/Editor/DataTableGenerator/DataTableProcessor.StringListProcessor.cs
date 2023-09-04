using System;
using System.Collections.Generic;
using System.IO;
using static GameMain.Editor.DataTableTools.DataTableProcessor;

namespace GameMain.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class StringListProcessor : GenericDataProcessor<List<String>>
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
                    return "List<String>";
                }
            }

            /// <summary>
            /// 定义的类型名称
            /// </summary>
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "List<String>",
                };
            }

            /// <summary>
            /// 解析输入的值，返回定义的类型值
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public override List<String> Parse(string value)
            {
                List<String> temp = new List<String>();
                if (value == "" || value == "empty")
                {
                    return temp;
                }
                string[] values = value.Split(',');
                foreach (var VarIAble in values)
                {
                    temp.Add(VarIAble);
                }
                return temp;
            }

            /// <summary>
            /// 写入二进制流
            /// </summary>
            /// <param name="dataTableProcessor"></param>
            /// <param name="binaryWriter"></param>
            /// <param name="value"></param>
            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                List<string> NodeTagList = Parse(value);
                binaryWriter.Write(NodeTagList.Count);
                foreach (var NodeTagItem in NodeTagList)
                {
                    binaryWriter.Write(NodeTagItem);
                }

            }
        }
    }
}
