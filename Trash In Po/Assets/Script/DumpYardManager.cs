using UnityEngine;

public class DumpYardManager : MonoBehaviour
{
    public GameObject dumpYardObject;
    //private bool isRevealed = false;

    void Start()
    {
        if (dumpYardObject == null)
        {
            Debug.LogError("DumpYardObject not assigned!");
            return;
        }
        //dumpYardObject.SetActive(false);
    }

    public void RevealDumpYard()
    {
        //if (!isRevealed)
        //{
        //    //dumpYardObject.SetActive(true);
        //    isRevealed = true;
        //}
    }
}