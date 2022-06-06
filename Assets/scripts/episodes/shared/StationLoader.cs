using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationLoader : MonoBehaviour
{
    [SerializeField] private StationManager stationPrefab_;
    [SerializeField] private NetworkManager networkManager_;

    private void Start()
    {
        StationManager station = Instantiate(stationPrefab_);
        station.transform.SetParent(transform);
        station.transform.localPosition = Vector3.one;
        station.transform.localScale = Vector3.one;

        networkManager_.AddNewGameManager(station);
    }
}
