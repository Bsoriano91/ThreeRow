using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField]
    private GameStates currentState; // field
    public GameStates Currentstate   // property
    {
        get { return currentState; }
        set { NewGameState (value); }
    }

    [SerializeField]
    PlayerStatus CurrentPlayer;

    [SerializeField]
    PlayerStatus[] _slots;

    private void Awake()
    {
        InitializeManager();

        _slots = new PlayerStatus[9];
        CurrentPlayer = PlayerStatus.P1;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void NewGameState(GameStates newState)
    {
        currentState = newState;

        string message = string.Format("{0} {1} {2} {3}", "GameState = ", "<color=yellow", currentState, "</color>");
        Debug.Log(message);

        switch (currentState)
        {
            case GameStates.MainMenu:
                break;
            case GameStates.Loading:
                break;
            case GameStates.Playing:
                break;
            case GameStates.GamePaused:
                break;
            case GameStates.EndGame:
                break;
        }
    }

    internal void ChangeSlot(int slotIndex)
    {
        if (CanChangeSlot(slotIndex))
        {
            SlotChosen(slotIndex);

            if (CheckDiagonals(slotIndex)) Debug.Break();
            else ChangePlayer();

            //if (!CheckRow(slotIndex)) ChangePlayer();
            //else Debug.Log("Game Over");
        }
        else Debug.Log("Slot already used");
    }

    bool CanChangeSlot(int slotIndex)
    {
        return _slots[slotIndex] == PlayerStatus.None;
    }

    void SlotChosen(int slotIndex)
    {
        _slots[slotIndex] = CurrentPlayer;
    }

    void ChangePlayer()
    {
        if (CurrentPlayer == PlayerStatus.P1) CurrentPlayer = PlayerStatus.P2;
        else if (CurrentPlayer == PlayerStatus.P2) CurrentPlayer = PlayerStatus.P1;
    }

    bool CheckRow(int slotIndex)
    {
        int from = (slotIndex / 3) * 3;
        int to = from + 3;

        return _slots[from..to].Where (x => x == CurrentPlayer).Count() == 3;
    }

    bool CheckColumn(int slotIndex)
    {
        int from = (slotIndex % 3);
        return _slots.Skip (from).Where((x, index) => (index % 3) == 0).Count() == 3;
    }

    bool CheckDiagonals(int slotIndex)
    {
        bool checkColumnLeftToRight = slotIndex % 4 == 0 ? true : false;
        if (checkColumnLeftToRight && CheckDiagonalLeftToRight(slotIndex)) return true;
        
        bool checkColumnRightToLeft = slotIndex > 0 && slotIndex < 8 && slotIndex % 2 == 0 ? true : false;
        if (checkColumnRightToLeft && CheckDiagonalRightToLeft(slotIndex)) return true;

        return false;
    }

    bool CheckDiagonalLeftToRight(int slotIndex)
    {
        return _slots.Where((x, index) => (index % 4) == 0).Count () == 3;
    }

    bool CheckDiagonalRightToLeft(int slotIndex)
    {
        return _slots.Skip (2).Where((x, index) => (index % 2) == 0 && index < _slots.Length - 3).Count() == 3;
    }
}

public enum PlayerStatus {None, P1, P2}

public enum GameStates{ MainMenu, Loading, Playing, GamePaused, EndGame }
