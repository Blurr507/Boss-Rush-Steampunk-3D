using UnityEngine;

public class PrewarmButtons : MonoBehaviour
{
    void Start()
    {
        //  Activate the children to trigger their awake script, so it doesn't cause lag later, then deactivate this object
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
