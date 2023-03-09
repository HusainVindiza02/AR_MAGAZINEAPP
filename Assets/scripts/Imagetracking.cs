using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class Imagetracking : MonoBehaviour
{
    public GameObject[] placeblePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        /*
        foreach(GameObject prefab in placeblePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
        */
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private GameObject FindPrefabByNameInPlaceblePrefabs(string name)
    {
        foreach (GameObject go in placeblePrefabs)
        {
            if (go.name == name)
            {
                return go;
            }
        }
        return null;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            string name = trackedImage.referenceImage.name;
            GameObject prefab = FindPrefabByNameInPlaceblePrefabs(name);
            GameObject newPrefab = Instantiate(prefab, transform.localPosition, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);

            //UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string name = trackedImage.referenceImage.name;
            GameObject prefab = spawnedPrefabs[name];

            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Vector3 position = trackedImage.transform.position;
                Quaternion rotaion = trackedImage.transform.rotation;

                prefab.gameObject.transform.position = position;
                prefab.gameObject.transform.rotation = rotaion;
                prefab.gameObject.SetActive(true);
            }
            else
            {
                prefab.gameObject.SetActive(false);
            }
            //UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Destroy(spawnedPrefabs[trackedImage.name]);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject prefab = spawnedPrefabs[name];
        Vector3 position = trackedImage.transform.position;

        prefab.gameObject.transform.position = position;
        prefab.gameObject.SetActive(true);

        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.name != name || trackedImage.trackingState == TrackingState.None || trackedImage.trackingState == TrackingState.Limited)
            {
                go.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
