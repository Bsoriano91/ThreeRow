using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent (typeof(Button))]
public class Slot : MonoBehaviour
{
    Button _button;
    TextMeshProUGUI _text;

    int _index;

    private void Awake()
    {
        _button = GetComponent (typeof (Button)) as Button;
        _text = GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

        _index = GetSlotIndex ();
    }

    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(SlotPressed);
    }

    int GetSlotIndex()
    {
        int childIndex = transform.GetSiblingIndex();
        int parentIndex = transform.parent.GetSiblingIndex();

        return childIndex + parentIndex * 3;
    }

    void SlotPressed()
    {
        HudManager.SlotPressed(_index);

    }
}
