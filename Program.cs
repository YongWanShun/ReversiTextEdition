using System;

class ReversiGame
{
    private const int BoardSize = 8;
    private char[,] board;

    private char currentPlayer;
    private char opponentPlayer;

    public ReversiGame()
    {
        board = new char[BoardSize, BoardSize];
        InitializeBoard();
        currentPlayer = 'B'; // 黑方先手
        opponentPlayer = 'W'; // 白方后手
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                board[row, col] = '-';
            }
        }

        // 初始化中间的四个棋子
        board[3, 3] = 'W';
        board[3, 4] = 'B';
        board[4, 3] = 'B';
        board[4, 4] = 'W';
    }

    private void PrintBoard()
    {
        Console.WriteLine("  a b c d e f g h");
        for (int row = 0; row < BoardSize; row++)
        {
            Console.Write((row + 1) + " ");
            for (int col = 0; col < BoardSize; col++)
            {
                Console.Write(board[row, col] + " ");
            }
            Console.WriteLine();
        }
    }

    private bool IsValidMove(int row, int col)
    {
        if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize || board[row, col] != '-')
        {
            return false;
        }

        // 检查当前位置的八个方向
        for (int dRow = -1; dRow <= 1; dRow++)
        {
            for (int dCol = -1; dCol <= 1; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                int r = row + dRow;
                int c = col + dCol;
                bool foundOpponent = false;

                while (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize)
                {
                    if (board[r, c] == '-')
                        break;

                    if (board[r, c] == opponentPlayer)
                    {
                        foundOpponent = true;
                    }
                    else if (board[r, c] == currentPlayer)
                    {
                        if (foundOpponent)
                            return true;
                        break;
                    }

                    r += dRow;
                    c += dCol;
                }
            }
        }

        return false;
    }

    private void MakeMove(int row, int col)
    {
        if (IsValidMove(row, col))
        {
            board[row, col] = currentPlayer;

            // 翻转对手的棋子
            for (int dRow = -1; dRow <= 1; dRow++)
            {
                for (int dCol = -1; dCol <= 1; dCol++)
                {
                    if (dRow == 0 && dCol == 0)
                        continue;

                    int r = row + dRow;
                    int c = col + dCol;
                    bool foundOpponent = false;
                    bool hasValidMove = false;

                    while (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize)
                    {
                        if (board[r, c] == '-')
                            break;

                        if (board[r, c] == opponentPlayer)
                        {
                            foundOpponent = true;
                        }
                        else if (board[r, c] == currentPlayer)
                        {
                            if (foundOpponent)
                            {
                                hasValidMove = true;
                                break;
                            }
                            break;
                        }

                        r += dRow;
                        c += dCol;
                    }

                    if (hasValidMove && foundOpponent)
                    {
                        // 翻转对手的棋子
                        r = row + dRow;
                        c = col + dCol;
                        while (board[r, c] == opponentPlayer)
                        {
                            board[r, c] = currentPlayer;
                            r += dRow;
                            c += dCol;
                        }
                    }
                }
            }

            // 切换玩家
            char temp = currentPlayer;
            currentPlayer = opponentPlayer;
            opponentPlayer = temp;
        }
        else
        {
            Console.WriteLine("Invalid move. Please try again.");
        }
    }

    private bool HasValidMoves()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (IsValidMove(row, col))
                    return true;
            }
        }
        return false;
    }

    private void PlayGame()
    {
        while (true)
        {
            PrintBoard();
            Console.WriteLine($"Current Player: {currentPlayer}");

            if (!HasValidMoves())
            {
                // 如果没有合法的移动，轮到另一个玩家走
                char temp = currentPlayer;
                currentPlayer = opponentPlayer;
                opponentPlayer = temp;

                if (!HasValidMoves())
                {
                    // 游戏结束
                    Console.WriteLine("Game Over!");
                    int blackCount = 0;
                    int whiteCount = 0;

                    for (int row = 0; row < BoardSize; row++)
                    {
                        for (int col = 0; col < BoardSize; col++)
                        {
                            if (board[row, col] == 'B')
                                blackCount++;
                            else if (board[row, col] == 'W')
                                whiteCount++;
                        }
                    }

                    Console.WriteLine($"Black: {blackCount}, White: {whiteCount}");
                    if (blackCount > whiteCount)
                        Console.WriteLine("Black wins!");
                    else if (whiteCount > blackCount)
                        Console.WriteLine("White wins!");
                    else
                        Console.WriteLine("It's a draw!");

                    break;
                }
            }

            Console.Write("Enter your move (e.g., 'a2'): ");
            string input = Console.ReadLine().ToLower();

            if (input.Length == 2 && input[0] >= 'a' && input[0] <= 'h' && input[1] >= '1' && input[1] <= '8')
            {
                int col = input[0] - 'a';
                int row = input[1] - '1';

                MakeMove(row, col);
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }

    public void Start()
    {
        Console.WriteLine("Welcome to Reversi!");
        PlayGame();
    }
}

class Program
{
    static void Main(string[] args)
    {
        ReversiGame game = new ReversiGame();
        game.Start();
    }
}