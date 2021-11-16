using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class EpisodeSpawnEditor : EditorWindow
{
    private string NodeData;

    [MenuItem("Lando/Episode Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EpisodeSpawnEditor));
    }   

    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Input Episode Code");
        NodeData = GUILayout.TextArea(NodeData, TextAreaStyle, GUILayout.Width(400), GUILayout.MinHeight(100), GUILayout.MaxHeight(400));
        if (GUILayout.Button("Generate"))
        {
            GenerateNodes();
        }
    }

    //does not support nested options
    public void GenerateNodes()
    {
        EpisodeSpawnData d = ConvertData(NodeData);

        GameObject obj = new GameObject();
        obj.AddComponent<Episode>();
        Episode episode = obj.GetComponent<Episode>();
        episode.gameObject.name = d.EpisodeName;

        List<EpisodeNode> newNodes = new List<EpisodeNode>();

        //create nodes
        foreach (EpisodeSpawnData.Node n in d.Nodes)
        {
            newNodes.Add(EpisodeNode.CreateNewNode(episode.transform, d.VideoRoot + n.VideoFile + ".mp4", d.VideoRoot + n.LoopVideoFile + ".mp4", n.Script, n.Options));
        }

        //link nodes
        for (int i = 0; i < newNodes.Count; i++)
        {
            EpisodeNode nextNode = null;
            if (i + 1 < newNodes.Count)
            {
                nextNode = newNodes[i + 1];
            }
            newNodes[i].NextNode = nextNode;

            List<EpisodeNode.Option> o = newNodes[i].Options;
            if (o.Count > 0)
            {
                newNodes[i].NextNode = null;    //TA first choose an option first before skipping
                for (int ii = 0; ii < o.Count; ii++)
                {
                    o[ii].Node.Options = o;
                    o[ii].Node.NextNode = nextNode;
                }
            }
        }

        episode.StartingNode = newNodes[0];

        //rename duplicates
        List<EpisodeNode> expandedNewNodes = new List<EpisodeNode>();
        foreach(EpisodeNode n in newNodes)
        {
            expandedNewNodes.Add(n);
            if (n.Options.Count > 0)
            {
                foreach(EpisodeNode.Option o in n.Options)
                {
                    expandedNewNodes.Add(o.Node);
                }
            }
        }

        Dictionary<string, int> nameIds = new Dictionary<string, int>();
        foreach(EpisodeNode n in expandedNewNodes)
        {
            if (nameIds.ContainsKey(n.name))
            {
                nameIds[n.name]++;
                n.name = n.name + "-" + nameIds[n.name];
            } else
            {
                nameIds[n.name] = 0;
            }
        }
    }

    static EpisodeSpawnData ConvertData(string input)
    {
        var data = new EpisodeSpawnData();

        #region RootParsing
        Match rootMatch = Regex.Match(input, "VideoRoot:.*");
        data.VideoRoot = rootMatch.Value.Replace("VideoRoot: ", "");
        input = input.Remove(rootMatch.Index, rootMatch.Length + 1); //+1 to remove \n
        #endregion

        #region EpisodeNameParsing
        Match episodeNameMatch = Regex.Match(input, "EpisodeName:.*");
        data.EpisodeName = episodeNameMatch.Value.Replace("EpisodeName: ", "");
        input = input.Remove(episodeNameMatch.Index, episodeNameMatch.Length + 1); //+1 to remove \n
        #endregion

        #region NodesParsing
        data.Nodes = CreateNodes(input);
        #endregion

        #region OptionsParsing
        CreateOptionsForNodes(data.Nodes);
        #endregion

        return data;
    }



    static List<string> BreakMainSections(string input)
    {
        var list = new List<string>();

        var r1 = @"(([A-Z]+\d+\-\d+\.\d+).*\n){2}((.|\n)*?)(([A-Z]+\d+\-\d+\.\d+).*\n){2}";
        var r2 = @"(([A-Z]+\d+\-\d+\.\d+).*\n){2}";

        while (Regex.Match(input, r1).Success)
        {
            //r1 expression get the content between two headers (couldn't come up with better regex)
            var value = Regex.Match(input, r1).Value;
            input = input.Replace(value, "");
            //r2 matches the second header
            Match secondMatch = Regex.Match(value, r2, RegexOptions.RightToLeft);
            //remove the second header from value
            //value = value.Replace(secondMatch.Value, "");
            value = value.Substring(0, value.LastIndexOf(secondMatch.Value));
            //add the second header back to input text start
            input = secondMatch.Value + input;
            list.Add(value);
        }

        //adds what's left of the text to list.
        list.Add(input);

        return list;
    }

    static List<EpisodeSpawnData.Node> CreateNodes(string input)
    {
        var list = new List<EpisodeSpawnData.Node>();
        var mainSections = BreakMainSections(input);
        var r1 = @"(([A-Z]+\d+\-\d+\.\d+).*\n){2}";

        foreach (var section in mainSections)
        {
            //identify header
            var header = Regex.Match(section, r1).Value;
            //split header to get VideoFile and LoopVideoFile
            var headerSplit = header.Split('\n');
            var node = new EpisodeSpawnData.Node();
            node.VideoFile = headerSplit[0];
            node.LoopVideoFile = headerSplit[1];
            //remove header to get the rest of the content
            var reminderText = section.Replace(header, "");
            node.Script = reminderText;
            list.Add(node);
        }

        return list;
    }

    static void CreateOptionsForNodes(List<EpisodeSpawnData.Node> nodes)
    {
        foreach (var n in nodes)
        {
            var r1 = @"\t{2,}.*\n((\t{2,}[A-Z]+\d+\-\d+\.\d+\n).*)(.|\n)*?<\/i>";
            var r2 = @"\t{2,}.*\n((\t{2,}[A-Z]+\d+\-\d+\.\d+\n).*)(.|\n)*?";

            if (Regex.IsMatch(n.Script, r1))
            {
                foreach (Match m in Regex.Matches(n.Script, r1))
                {
                    var optionNode = new EpisodeSpawnData.NodeOption();

                    //store matched value into <value>
                    var value = m.Value;
                    //remove matched content from node script
                    n.Script = n.Script.Replace(value, "");
                    //identify header
                    var header = Regex.Match(value, r2).Value;
                    //remove header from value
                    value = value.Replace(header, "");
                    //split header to get VideoFile and LoopVideoFile values
                    var headerSplit = header.Split('\n');
                    optionNode.Name = headerSplit[0].Replace("\t", "");
                    optionNode.Node = new EpisodeSpawnData.Node();
                    optionNode.Node.VideoFile = headerSplit[1].Replace("\t", "");
                    optionNode.Node.LoopVideoFile = headerSplit[2].Replace("\t", "");
                    //set the option node script to remaining text value and cleaning script
                    optionNode.Node.Script = value.Replace("\t", "");

                    if (optionNode.Node.Script.StartsWith("\n"))
                        optionNode.Node.Script = optionNode.Node.Script.Remove(0, 1);

                    if (optionNode.Node.Script.EndsWith("\n"))
                        optionNode.Node.Script = optionNode.Node.Script.Remove(optionNode.Node.Script.Length - 1);

                    n.Options.Add(optionNode);
                }
            }


            //cleaning script \t and \n
            n.Script = n.Script.Replace("\t", "");

            if (n.Script.StartsWith("\n"))
                n.Script = n.Script.Remove(0, 1);

            if (n.Script.EndsWith("\n"))
                n.Script = n.Script.Remove(n.Script.Length - 1);
        }
    }

    private GUIStyle TextAreaStyle
    {
        get
        {
            return new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true
            };
        }
    }
}
