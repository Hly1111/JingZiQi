using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private AudioClip[] audioClips;
    
    private IResourceFactory _resourceFactory;
    private readonly ObjectPool[] _tokenPool = new ObjectPool[2];
    
    private GameObject _cursor;
    private int _currentCursor;
    private Transform _cursorTransform;
    
    private readonly Board _board = new Board();
    private bool _playerStarted = true; // 是否玩家先手
    private int _currentPlacer = 1; // 1 for player 1, 2 for player 2
    
    private GameStateMachine _gameStateMachine;
    private IAIStrategy _aiStrategy;
    
    public delegate void OnOutcome(int winner);
    public OnOutcome OutcomeEvent;
    
    public delegate void OnRestart();
    public OnRestart RestartEvent;

    private void Start()
    {
        _resourceFactory = ServiceProvider.Instance.GetService<IResourceFactory>();
        if(_resourceFactory == null)
        {
            Debug.LogError("ResourceFactory service not found.");
            return;
        }
        _cursor = _resourceFactory.GetResource<GameObject>("Cursor");
        
        var cursor = Instantiate(_cursor, slots[_currentCursor].position, Quaternion.identity);
        _cursorTransform = cursor.transform;
        
        _gameStateMachine = new GameStateMachine();
        _gameStateMachine.ChangeState(new PlayerTurnState(this)); //根据先后手调整
        
        _aiStrategy = new SmartAIStrategy();
        _tokenPool[0] = new ObjectPool(_resourceFactory.GetResource<GameObject>("Disc"), 5);
        _tokenPool[1] = new ObjectPool(_resourceFactory.GetResource<GameObject>("Rectangle"), 5);
    }

    private void Update()
    {
        _gameStateMachine.Update();
    }

    public void HandlePlayerInput()
    {
        Vector2 input = Vector2.zero;
        if(Input.GetKeyDown(KeyCode.LeftArrow)) input = Vector2.left;
        if(Input.GetKeyDown(KeyCode.RightArrow)) input = Vector2.right;
        if(Input.GetKeyDown(KeyCode.UpArrow)) input = Vector2.up;
        if(Input.GetKeyDown(KeyCode.DownArrow)) input = Vector2.down;
        if (input != Vector2.zero)
        {
            MoveCursor(input);
            AudioManager.Instance.PlaySound(audioClips[0]); // 播放移动音效
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TryPlace(_currentCursor))
            {
                OnPlayerPlaced();
                AudioManager.Instance.PlaySound(audioClips[1]);
            }
            else
            {
                AudioManager.Instance.PlaySound(audioClips[2]);
            }
        }
    }

    public void HandleAIInput()
    {
        //AI 落子
        int move = _aiStrategy.GetMove(_board);
        if (move == -1) return;
        if (TryPlace(move))
        {
            OnAIPlaced();
            AudioManager.Instance.PlaySound(audioClips[1]);
        }
        else
        {
            Debug.LogError("AI tried to place in an invalid position: " + move);
        }
    }

    private void MoveCursor(Vector2 direction)
    {
        _cursorTransform.SetParent(null);
        int x = _currentCursor % 3;
        int y = _currentCursor / 3;
        int dx = (int)direction.x;
        int dy = -(int)direction.y;
        x = (x + dx + 3) % 3;
        y = (y + dy + 3) % 3;
        _currentCursor = y * 3 + x;
        _cursorTransform.position = slots[_currentCursor].position;
    }

    private bool TryPlace(int index)
    {
        int x = index % 3;
        int y = index / 3;
        if (_board.TryPlace(x, y, _currentPlacer))
        {
            var token = _tokenPool[_currentPlacer - 1].GetObject();
            token.transform.position = slots[index].position;
            token.transform.SetParent(slots[index]);
            _currentPlacer = _currentPlacer == 1 ? 2 : 1; // Switch placer
            return true;
        }
        return false;
    }

    private void OnPlayerPlaced()
    {
        int winner = _board.CheckWinner();
        if (winner != -1)
        {
            _gameStateMachine.ChangeState(new ResultState(winner, this));
        }
        else
        {
            _gameStateMachine.ChangeState(new AITurnState(this));
        }
    }
    
    private void OnAIPlaced()
    {
        int winner = _board.CheckWinner();
        if (winner != -1)
        {
            _gameStateMachine.ChangeState(new ResultState(winner, this));
        }
        else
        {
            _gameStateMachine.ChangeState(new PlayerTurnState(this));
        }
    }

    private void ClearBoard()
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount > 0)
            {
                var token = slot.GetChild(0);
                if (token.name.Contains("Disc"))
                {
                    _tokenPool[0].Return(token.gameObject);
                }
                else if (token.name.Contains("Rectangle"))
                {
                    _tokenPool[1].Return(token.gameObject);
                }
            }
        }
    }

    public void HandleRestart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearBoard();
            _board.ResetBoard();
            _currentPlacer = _playerStarted ? 2 : 1;
            if (_playerStarted)
            {
                _playerStarted = false;
                _gameStateMachine.ChangeState(new AITurnState(this));
            }
            else
            {
                _playerStarted = true;
                _gameStateMachine.ChangeState(new PlayerTurnState(this));
            }
        }
    }
}