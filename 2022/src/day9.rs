use std::{collections::HashSet, str::FromStr};

use crate::FromLines;

enum Direction {
    Up,
    Down,
    Left,
    Right,
}

struct Command {
    direction: Direction,
    length: usize,
}

pub(crate) struct Solution {
    commands: Vec<Command>,
}

impl FromStr for Command {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let split: Vec<&str> = s.split(' ').collect();
        let direction = match split[0] {
            "R" => Direction::Right,
            "L" => Direction::Left,
            "U" => Direction::Up,
            "D" => Direction::Down,
            _ => panic!("Unknown direction"),
        };
        let length = split[1].parse().unwrap();
        Ok(Command { direction, length })
    }
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let commands = lines.map(|line| line.parse().unwrap()).collect();
        Solution { commands }
    }
}

#[derive(Hash, PartialEq, Eq, Clone, Copy, Default)]
struct Position {
    x: i32,
    y: i32,
}

impl Position {
    fn travel(self, direction: &Direction) -> Position {
        match direction {
            Direction::Up => Position {
                x: self.x,
                y: self.y + 1,
            },
            Direction::Down => Position {
                x: self.x,
                y: self.y - 1,
            },
            Direction::Left => Position {
                x: self.x - 1,
                y: self.y,
            },
            Direction::Right => Position {
                x: self.x + 1,
                y: self.y,
            },
        }
    }

    fn follow(self, head: &Position) -> Position {
        if (self.x - head.x).abs() > 1 && (self.y - head.y).abs() > 1 {
            return Position {
                x: (self.x + head.x) / 2,
                y: (self.y + head.y) / 2,
            };
        }
        if (self.x - head.x).abs() > 1 {
            return Position {
                x: (self.x + head.x) / 2,
                y: head.y,
            };
        }
        if (self.y - head.y).abs() > 1 {
            return Position {
                x: head.x,
                y: (self.y + head.y) / 2,
            };
        }
        return self;
    }
}

struct Rope {
    knots: Vec<Position>,
}

impl Rope {
    fn consume<F: FnMut(&Vec<Position>) -> ()>(&mut self, command: &Command, on_movement: &mut F) {
        for _ in 0..command.length {
            self.knots[0] = self.knots[0].travel(&command.direction);
            for i in 1..self.knots.len() {
                self.knots[i] = self.knots[i].follow(&self.knots[i - 1]);
            }
            on_movement(&self.knots);
        }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 9;

    fn solve_first_part(&self) -> String {
        self.simulate_movement(2).to_string()
    }

    fn solve_second_part(&self) -> String {
        self.simulate_movement(10).to_string()
    }
}

impl Solution {
    fn simulate_movement(&self, length: usize) -> usize {
        let knots = vec![Default::default(); length];
        let mut rope = Rope { knots };
        let mut visited = HashSet::new();
        let mut on_movement = |knots: &Vec<Position>| {
            visited.insert(*knots.last().unwrap());
        };
        for command in self.commands.iter() {
            rope.consume(command, &mut on_movement);
        }
        visited.len()
    }
}
