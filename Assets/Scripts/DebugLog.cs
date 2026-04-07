using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(-3)]
public class DebugLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLog;

    static DebugLog inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
        txtLog.text = "";
    }

    // Update is called once per frame
    public static void Add(string txt)
    {
        if (inst)
        {
            inst.txtLog.text += txt + "\n";
        }
    }
}
