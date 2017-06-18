#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

public class CompileProtoFiles : Editor
{
    const string PROTO_FOLDER = @"..\Product\proto";
    const string PROTO_CSHARP_FOLDER = @".\Plugins\Network\protocol";
    const string PROTOC_PATH = @"..\Product\proto\tools\bin\protoc.exe";
    const string NameSpace = @"BestSects.protocol";

    [MenuItem("Tools/Protobuf/CompileAll")]
    public static void CompileAllProto()
    {
        DirectoryInfo protoPath = new DirectoryInfo(Path.Combine(Application.dataPath, PROTO_FOLDER));
        DirectoryInfo exportPath = new DirectoryInfo(Path.Combine(Application.dataPath, PROTO_CSHARP_FOLDER));
        protoFolder = protoPath.FullName;
        exportFolder = exportPath.FullName;
        FileInfo[] fileInfos = protoPath.GetFiles();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Extension.ToLower() == ".proto")
            {
                CompileProtoFile(fileInfos[i].FullName);
            }
        }
    }

    static string protoFolder = "";
    static string exportFolder = "";
    static Queue<string> protoFiles = new Queue<string>();
    public static void CompileProtoFile(string filePath)
    {
        lock (protoFiles)
        {
            protoFiles.Enqueue(filePath);
        }
        StartCompilerProcess();
    }

    static void StartCompilerProcess()
    {
        string filePath = "";
        lock (protoFiles)
        {
            if (protoFiles.Count <= 0)
            {
                AssetDatabase.Refresh();
                Debug.Log("全部编译结束");
                return;
            }
            filePath = protoFiles.Dequeue();
        }
        //option csharp_namespace = "BestSects.protocol";
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gbk"));

        string content = sr.ReadToEnd();
        if (!content.Contains("option csharp_namespace"))
        {
            int index = content.IndexOf("option java_outer_classname");
            if (index < 0)
                index = 0;
            content = content.Insert(index, "option csharp_namespace = \"" + NameSpace + "\";\n");
            sr.Close();
            fs.Close();
            fs.Dispose();

            StreamWriter sw = new StreamWriter(filePath, false, Encoding.GetEncoding("gbk"));
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
        
        Process process = new Process();
        process.StartInfo.FileName = Path.Combine(Application.dataPath, PROTOC_PATH);
        process.StartInfo.Arguments = " --csharp_out=" + exportFolder + " --proto_path=" + protoFolder + " " + filePath;
        process.Disposed += DisposedHandler;
        Debug.Log("->" + process.StartInfo.FileName + "  " + process.StartInfo.Arguments);
        process.Start();
        process.WaitForExit();
        process.Close();
        //process.Dispose();
    }

    static void DisposedHandler(object sender, System.EventArgs e)
    {
        Debug.Log("进程结束");
        StartCompilerProcess();
    }
}

#endif
