using UnityEngine;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SlotPressed(int slotIndex)
    {
        GameManager.Instance?.ChangeSlot(slotIndex);
    }
}
