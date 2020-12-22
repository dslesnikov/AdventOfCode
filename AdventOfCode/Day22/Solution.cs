using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day22
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private int[] _playerOneCards;
        private int[] _playerTwoCards;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 22;

        public async Task InitializeAsync()
        {
            var text = await _reader.ReadTextAsync();
            var split = text.Split("\n\n");
            _playerOneCards = split[0].Split('\n')[1..].Select(int.Parse).ToArray();
            _playerTwoCards = split[1].Split('\n')[1..].Select(int.Parse).ToArray();
        }

        public string PartOne()
        {
            var player1 = new Queue<int>(_playerOneCards);
            var player2 = new Queue<int>(_playerTwoCards);
            while (player1.Count > 0 && player2.Count > 0)
            {
                var player1Card = player1.Dequeue();
                var player2Card = player2.Dequeue();
                if (player1Card > player2Card)
                {
                    player1.Enqueue(player1Card);
                    player1.Enqueue(player2Card);
                }
                else
                {
                    player2.Enqueue(player2Card);
                    player2.Enqueue(player1Card);
                }
            }
            var winner = player1.Count > 0 ? player1 : player2;
            var score = GetScore(winner);
            return score.ToString();
        }

        public string PartTwo()
        {
            var left = new Queue<int>(_playerOneCards);
            var right = new Queue<int>(_playerTwoCards);
            var gameHashes = new HashSet<ulong>();
            var leftWon = PlayGame(left, right, gameHashes);
            var score = leftWon ? GetScore(left) : GetScore(right);
            return score.ToString();
        }

        private bool PlayGame(Queue<int> leftDeck, Queue<int> rightDeck, HashSet<ulong> previousGames)
        {
            while (leftDeck.Count > 0 && rightDeck.Count > 0)
            {
                var gameHash = GetGameHash(leftDeck, rightDeck);
                if (!previousGames.Add(gameHash))
                {
                    return true;
                }
                var left = leftDeck.Dequeue();
                var right = rightDeck.Dequeue();
                bool leftWon;
                if (leftDeck.Count >= left && rightDeck.Count >= right)
                {
                    leftWon = PlayGame(new Queue<int>(leftDeck.Take(left)), new Queue<int>(rightDeck.Take(right)),
                        new HashSet<ulong>());
                }
                else
                {
                    leftWon = left > right;
                }
                if (leftWon)
                {
                    leftDeck.Enqueue(left);
                    leftDeck.Enqueue(right);
                }
                else
                {
                    rightDeck.Enqueue(right);
                    rightDeck.Enqueue(left);
                }
            }
            return leftDeck.Count > 0;
        }

        private int GetScore(Queue<int> deck)
        {
            var score = 0;
            while (deck.Count > 0)
            {
                score += deck.Count * deck.Dequeue();
            }
            return score;
        }

        private ulong GetGameHash(Queue<int> leftDeck, Queue<int> rightDeck)
        {
            var result = 0ul;
            var x = 1ul;
            foreach (var card in leftDeck)
            {
                result += (ulong)card * x;
                x <<= 1;
            }
            x = (ulong)1 << 50;
            foreach (var card in rightDeck)
            {
                result += (ulong)card * x;
                x <<= 1;
            }
            return result;
        }
    }
}