using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStationManager : StationManager
{
    //display available resources, and then hide after 15s and say VO

    //display no resources with a map and say VO

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (string.Equals("give-resources", arguments[0]))
        {
            GiveResources(new List<string>(arguments.GetRange(1, arguments.Count - 1)));
        }

        if (string.Equals("more-resources", arguments[0]))
        {
            if (arguments.Count > 3)
            {
                int position = int.Parse(arguments[1]); //player's current position
                int total = int.Parse(arguments[2]);    //total challenges available
                int nextResource = int.Parse(arguments[3]); //where the next resource is
                MoreResources(position, total, nextResource);
            }
        }

        if (string.Equals("no-resources", arguments[0]))
        {
            NoResources();
        }
    }

    private void GiveResources(List<string> resources)
    {
        //show resources coming out of wristband
        //announce with VO
        //dispatcher should turn light green, maybe that sends two commands, one here and one to the light
        //turn screen off after 15s

        Debug.Log("get your resources! " + string.Join(", ", resources));
    }

    private void MoreResources(int position, int total, int nextResource)
    {
        //show position out of total nodes, show where next resource is
        //announce with VO

        Debug.Log(string.Format("you get more resources when you complete more challenges. position: {0} total: {1} nextResource: {2}", position, total, nextResource));
    }

    private void NoResources()
    {
        //no more resources left in the level
        //announce VO

        Debug.Log("no more resources left in class");
    }
}
