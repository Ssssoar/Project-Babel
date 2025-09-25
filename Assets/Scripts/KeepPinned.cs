using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class KeepPinned : MonoBehaviour{
    [SerializeField] GameObject keepPinnedTo;
    [SerializeField] bool pinning = false;

    RectTransform rectComp;
    RectTransform targetRectComp;

    void Start(){
        rectComp = GetComponent<RectTransform>();
        targetRectComp = keepPinnedTo.GetComponent<RectTransform>();
    }

    void Update(){
        if (pinning){
            transform.position = keepPinnedTo.transform.position;
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3 (keepPinnedTo.transform.lossyScale.x/transform.lossyScale.x, keepPinnedTo.transform.lossyScale.y/transform.lossyScale.y, keepPinnedTo.transform.lossyScale.z/transform.lossyScale.z);
            rectComp.sizeDelta = targetRectComp.sizeDelta;
        }
    }
}
