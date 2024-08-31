using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public interface IDialogSerializeHelper
    {
        public DialogData Serialize( object data);
    }
}
