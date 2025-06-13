public class Board
{
    private int[,] _board = new int[3,3];

    public bool TryPlace(int x, int y, int player)
    {
        if (_board[x, y] == 0)
        {
            _board[x, y] = player;
            return true;
        }
        return false;
    }

    public int[,] GetBoard()
    {
        return _board;
    }

    public void ResetBoard()
    {
        _board = new int[3,3];
    }

    public int CheckWinner()
    {
        for (int i = 0; i < 3; ++i)
        {
            var currentCheck = _board[i, 0];
            if (currentCheck != 0 && _board[i, 1] == currentCheck && _board[i, 2] == currentCheck)
            {
                return currentCheck;
            }
            currentCheck = _board[0, i];
            if (currentCheck != 0 && _board[1, i] == currentCheck && _board[2, i] == currentCheck)
            {
                return currentCheck;
            }
        }
        if(_board[0,0] != 0 && _board[1,1] == _board[0,0] && _board[2,2] == _board[0,0])
        {
            return _board[0,0];
        }
        if(_board[0,2] != 0 && _board[1,1] == _board[0,2] && _board[2,0] == _board[0,2])
        {
            return _board[0,2];
        }
        
        bool isFull = true;
        foreach (var cell in _board)
        {
            if (cell == 0)
            {
                isFull = false;
                break;
            }
        }
        if (isFull)
            return 0;
        
        return -1;
    }
}