using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject truckButton;
    public GameObject truckPanel;
    public GameObject truckPrefab;
    public Transform truckBuilding;
    public Transform dumpYard;
    public GameObject[] trashAreas; // Use an array for existing trash piles

    private bool gameStarted = false;
    private int hiredTrucks = 0;
    private const int maxFreeTrucks = 5;
    private Queue<GameObject> trashQueue = new Queue<GameObject>(); // Ordered list of trash

    void Start()
    {
        if (!ValidateReferences()) return;
        truckButton.SetActive(false);
        truckPanel.SetActive(false);

        // Add existing trash to queue
        foreach (GameObject trash in trashAreas)
        {
            trashQueue.Enqueue(trash);
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            StartCoroutine(ZoomOutAndShowButton());
        }
    }

    IEnumerator ZoomOutAndShowButton()
    {
        Vector3 startPos = mainCamera.transform.position;
        Vector3 endPos = new Vector3(15, 15, -15);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
        mainCamera.transform.position = endPos;
        truckButton.SetActive(true);
    }

    public void OpenTruckPanel()
    {
        truckPanel.SetActive(true);
    }

    public void HireTruck()
    {
        if (hiredTrucks < maxFreeTrucks)
        {
            GameObject truck = Instantiate(truckPrefab, truckBuilding.position, Quaternion.identity);
            TruckBehavior truckScript = truck.GetComponent<TruckBehavior>();
            truckScript.Initialize(dumpYard, this);
            hiredTrucks++;
            truckPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Max free trucks reached!");
        }
    }

    public GameObject GetNextTrash()
    {
        if (trashQueue.Count > 0)
        {
            return trashQueue.Dequeue();
        }
        return null;
    }

    private bool ValidateReferences()
    {
        if (mainCamera == null || truckButton == null || truckPanel == null || truckPrefab == null ||
            truckBuilding == null || dumpYard == null)
        {
            Debug.LogError("Missing references in GameManager!");
            return false;
        }
        return true;
    }
}
