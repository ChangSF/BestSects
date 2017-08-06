using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSF_SetActive : MonoBehaviour {
    /*
     下面是被指定的键（它们也可以组合起来使用）：
    %-CTRL 在Windows / CMD在OSX
    # -Shift
    & -Alt
    LEFT/RIGHT/UP/DOWN-光标键
    F1…F12
    HOME,END,PGUP,PDDN
    字母键不是key-sequence的一部分，要让字母键被添加到key-sequence中必须在前面加上下划线（例如：_g对应于快捷键”G”）。
    */
    [MenuItem("Tools/CSF/SetActive %&Z")]
    static void SeletEnable()
    {
        GameObject[] gos = Selection.gameObjects;

        if (gos == null || gos.Length == 0)
            return;
        foreach (GameObject go in gos)
        {
            bool enable = !go.activeSelf;
            go.SetActive(enable);
        }
    }
}
