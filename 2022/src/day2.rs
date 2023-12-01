use std::str::FromStr;

use crate::FromLines;

#[derive(Clone, Copy)]
enum Shape {
    Rock,
    Paper,
    Scissors,
}

impl FromStr for Shape {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s {
            "A" => Ok(Self::Rock),
            "B" => Ok(Self::Paper),
            "C" => Ok(Self::Scissors),
            _ => panic!("Unknown shape"),
        }
    }
}

impl Shape {
    fn get_score(&self) -> i32 {
        match self {
            Shape::Rock => 1,
            Shape::Paper => 2,
            Shape::Scissors => 3,
        }
    }

    fn get_loser(&self) -> Self {
        match self {
            Self::Rock => Self::Scissors,
            Self::Paper => Self::Rock,
            Self::Scissors => Self::Paper,
        }
    }

    fn get_winner(&self) -> Self {
        match self {
            Self::Rock => Self::Paper,
            Self::Paper => Self::Scissors,
            Self::Scissors => Self::Rock,
        }
    }
}

struct Round {
    opponent: Shape,
    me: String,
}

pub(crate) struct Solution {
    game: Vec<Round>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let rounds = lines
            .map(|line| {
                let tokens: Vec<&str> = line.split(' ').collect();
                let opponent = match tokens[0] {
                    "A" => Shape::Rock,
                    "B" => Shape::Paper,
                    "C" => Shape::Scissors,
                    _ => panic!("Unknown shape"),
                };
                Round {
                    opponent,
                    me: tokens[1].to_string(),
                }
            })
            .collect();
        Solution { game: rounds }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 2;

    fn solve_first_part(&self) -> String {
        self.game
            .iter()
            .map(|round| {
                let me = match round.me.as_str() {
                    "X" => Shape::Rock,
                    "Y" => Shape::Paper,
                    "Z" => Shape::Scissors,
                    _ => panic!(),
                };
                let score = Self::get_score(round.opponent, me);
                score
            })
            .sum::<i32>()
            .to_string()
    }

    fn solve_second_part(&self) -> String {
        self.game
            .iter()
            .map(|round| {
                let me = match round.me.as_str() {
                    "X" => round.opponent.get_loser(),
                    "Y" => round.opponent,
                    "Z" => round.opponent.get_winner(),
                    _ => panic!(),
                };
                let score = Self::get_score(round.opponent, me);
                score
            })
            .sum::<i32>()
            .to_string()
    }
}

impl Solution {
    fn get_score(opponent: Shape, me: Shape) -> i32 {
        let base_score = me.get_score();
        let win_score = match (opponent, me) {
            (Shape::Rock, Shape::Rock) => 3,
            (Shape::Rock, Shape::Paper) => 6,
            (Shape::Rock, Shape::Scissors) => 0,
            (Shape::Paper, Shape::Rock) => 0,
            (Shape::Paper, Shape::Paper) => 3,
            (Shape::Paper, Shape::Scissors) => 6,
            (Shape::Scissors, Shape::Rock) => 6,
            (Shape::Scissors, Shape::Paper) => 0,
            (Shape::Scissors, Shape::Scissors) => 3,
        };
        base_score + win_score
    }
}
