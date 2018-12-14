using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D13P2 : Solver<Tuple<int, int>>
    {
        public override int Day => 13;

        public override int Part => 2;

        protected override string Filename => @"Inputs\d13.input";

        protected override Tuple<int, int> GetAnswer(string input)
        {
            var (carts, tracks) = ParseInput(input);

            var cartLocationHash = new Dictionary<string, Cart>();
            foreach (var cart in carts)
                cartLocationHash[cart.Location.Identifier] = cart;

            while (true)
            {
                var orderedCarts = carts.OrderBy(c => c.Location.Y).ThenBy(c => c.Location.X);
                var removeCarts = new List<Cart>();

                foreach (var cart in orderedCarts)
                {
                    var track = tracks[cart.Location.Identifier];

                    cartLocationHash.Remove(cart.Location.Identifier);

                    switch (track.TrackType)
                    {
                        case TrackType.Horizontal:
                            if (cart.CurrentDirection == Direction.Left)
                                cart.Location = cart.Left;
                            else
                                cart.Location = cart.Right;
                            break;

                        case TrackType.Vertical:
                            if (cart.CurrentDirection == Direction.Up)
                                cart.Location = cart.Up;
                            else
                                cart.Location = cart.Down;
                            break;

                        case TrackType.Intersection:
                            HandleCartOnIntersection(cart);
                            break;

                        case TrackType.Curve:
                            HandleCartOnCurve(cart, track);
                            break;
                    }

                    if (cartLocationHash.ContainsKey(cart.Location.Identifier)
                    && !cartLocationHash[cart.Location.Identifier].Crashed)
                    {
                        var otherCart = cartLocationHash[cart.Location.Identifier];
                        cart.Crashed = otherCart.Crashed = true;

                        removeCarts.Add(cart);
                        removeCarts.Add(otherCart);
                    }
                    else
                    {
                        cartLocationHash[cart.Location.Identifier] = cart;
                    }
                }

                foreach (var cart in removeCarts)
                    carts.Remove(cart);

                if (carts.Count == 1)
                {
                    var lastCart = carts[0];
                    return new Tuple<int, int>(lastCart.Location.X, lastCart.Location.Y);
                }
            }
        }

        private Tuple<List<Cart>, Dictionary<string, Track>> ParseInput(string input)
        {
            var carts = new List<Cart>();
            var tracks = new Dictionary<string, Track>();

            var map = input.Split("\n").Select(l => l.ToCharArray()).ToArray();
            for (var y = 0; y < map.Length; y++)
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);
                    if (map[y][x] != ' ' && map[y][x] != '\r')
                    {
                        var track = new Track(p, map[y][x]);
                        tracks[track.Location.Identifier] = track;
                    }

                    if (Cart.IsCartCharacter(map[y][x]))
                    {
                        carts.Add(new Cart(p, map[y][x], carts.Count));
                    }
                }

            return new Tuple<List<Cart>, Dictionary<string, Track>>(carts, tracks);
        }

        private void HandleCartOnCurve(Cart cart, Track track)
        {
            switch (cart.CurrentDirection = _curveDirectionHelper[cart.CurrentDirection][track.Character])
            {
                case Direction.Up: cart.Location = cart.Up; break;
                case Direction.Down: cart.Location = cart.Down; break;
                case Direction.Left: cart.Location = cart.Left; break;
                case Direction.Right: cart.Location = cart.Right; break;
            }
        }

        private void HandleCartOnIntersection(Cart cart)
        {
            switch (cart.CurrentDirection = _intersectionDirectionHelper[cart.CurrentDirection][cart.NextIntersectionChoice])
            {
                case Direction.Up: cart.Location = cart.Up; break;
                case Direction.Down: cart.Location = cart.Down; break;
                case Direction.Left: cart.Location = cart.Left; break;
                case Direction.Right: cart.Location = cart.Right; break;
            }

            switch (cart.NextIntersectionChoice)
            {
                case IntersectionChoice.Left: cart.NextIntersectionChoice = IntersectionChoice.Straight; break;
                case IntersectionChoice.Straight: cart.NextIntersectionChoice = IntersectionChoice.Right; break;
                case IntersectionChoice.Right: cart.NextIntersectionChoice = IntersectionChoice.Left; break;
            }
        }

        private readonly Dictionary<Direction, Dictionary<char, Direction>>
            _curveDirectionHelper = new Dictionary<Direction, Dictionary<char, Direction>>
            {
                [Direction.Right] = new Dictionary<char, Direction>
                {
                    ['/'] = Direction.Up,
                    ['\\'] = Direction.Down
                },
                [Direction.Left] = new Dictionary<char, Direction>
                {
                    ['/'] = Direction.Down,
                    ['\\'] = Direction.Up
                },
                [Direction.Up] = new Dictionary<char, Direction>
                {
                    ['/'] = Direction.Right,
                    ['\\'] = Direction.Left,
                },
                [Direction.Down] = new Dictionary<char, Direction>
                {
                    ['/'] = Direction.Left,
                    ['\\'] = Direction.Right
                }
            };

      
        private readonly Dictionary<Direction, Dictionary<IntersectionChoice, Direction>>
            _intersectionDirectionHelper = new Dictionary<Direction, Dictionary<IntersectionChoice, Direction>>
            {
                [Direction.Right] = new Dictionary<IntersectionChoice, Direction>
                {
                    [IntersectionChoice.Left] = Direction.Up,
                    [IntersectionChoice.Straight] = Direction.Right,
                    [IntersectionChoice.Right] = Direction.Down
                },
                [Direction.Left] = new Dictionary<IntersectionChoice, Direction>
                {
                    [IntersectionChoice.Left] = Direction.Down,
                    [IntersectionChoice.Straight] = Direction.Left,
                    [IntersectionChoice.Right] = Direction.Up
                },
                [Direction.Up] = new Dictionary<IntersectionChoice, Direction>
                {
                    [IntersectionChoice.Left] = Direction.Left,
                    [IntersectionChoice.Straight] = Direction.Up,
                    [IntersectionChoice.Right] = Direction.Right,
                },
                [Direction.Down] = new Dictionary<IntersectionChoice, Direction>
                {
                    [IntersectionChoice.Left] = Direction.Right,
                    [IntersectionChoice.Straight] = Direction.Down,
                    [IntersectionChoice.Right] = Direction.Left
                }
            };

        private class Cart
        {
            public Cart(Point p, char directionChar, int number)
            {
                Location = p;
                CurrentDirection = CharacterToDirection(directionChar);
                Number = number;
            }
            public int Number { get; set; }
            public Point Location { get; set; }
            public Point Left => new Point(Location.X - 1, Location.Y);
            public Point Right => new Point(Location.X + 1, Location.Y);
            public Point Up => new Point(Location.X, Location.Y - 1);
            public Point Down => new Point(Location.X, Location.Y + 1);
            public bool Crashed { get; set; }
            public Direction CurrentDirection { get; set; }
            public IntersectionChoice NextIntersectionChoice { get; set; } = IntersectionChoice.Left;
            private Direction CharacterToDirection(char c)
            {
                switch (c)
                {
                    case '>': return Direction.Right;
                    case '<': return Direction.Left;
                    case 'v': return Direction.Down;
                    case '^': return Direction.Up;
                    default:
                        throw new Exception("You didn't parse cart directions right.");
                }
            }
            public static bool IsCartCharacter(char c)
            {
                return c == '>' || c == '<' || c == 'v' || c == '^';
            }
        }

        private class Track
        {
            public Track(int x, int y, char c) : this(new Point(x, y), c) { }
            public Track(Point p, char c)
            {
                Location = p;
                Character = c;
                TrackType = CharacterToTrackType(c);
            }
            public Point Location { get; }
            public char Character { get; }
            public TrackType TrackType { get; }
            private TrackType CharacterToTrackType(char c)
            {
                switch (c)
                {
                    case '>':
                    case '<':
                    case '-': return TrackType.Horizontal;
                    case '\\':
                    case '/': return TrackType.Curve;
                    case '+': return TrackType.Intersection;
                    case '|':
                    case 'v':
                    case '^': return TrackType.Vertical;
                    default:
                        throw new Exception("You didn't parse track types right.");
                }
            }
        }

        private class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
                Identifier = $"{X},{Y}";
            }
            public int X { get; set; }
            public int Y { get; set; }
            public string Identifier { get; }
        }

        private enum Direction { Up, Down, Left, Right }
        private enum IntersectionChoice { Left, Straight, Right }
        private enum TrackType { Vertical, Horizontal, Curve, Intersection }
    }
}
