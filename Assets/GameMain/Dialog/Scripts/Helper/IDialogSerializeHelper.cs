using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public interface IDialogSerializeHelper
    {
        public void Serialize(DialogData dialogData, object data);
    }
}
