use std::str::FromStr;

use once_cell::sync::Lazy;
use regex::Regex;

use crate::FromLines;

struct Command {
    from: usize,
    to: usize,
    quantity: u32,
}

const COMMAND_REGEX: Lazy<Regex> =
    Lazy::new(|| Regex::new("move (\\d+) from (\\d+) to (\\d+)").unwrap());

impl FromStr for Command {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let captures = COMMAND_REGEX.captures(s).unwrap();
        let quantity: u32 = captures.get(1).unwrap().as_str().parse().unwrap();
        let from: usize = captures.get(2).unwrap().as_str().parse().unwrap();
        let to: usize = captures.get(3).unwrap().as_str().parse().unwrap();
        Ok(Command {
            quantity,
            from: from - 1,
            to: to - 1,
        })
    }
}

pub(crate) struct Solution {
    stacks: [Vec<char>; 9],
    commands: Vec<Command>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let mut stacks: [Vec<char>; 9] = Default::default();
        let mut commands: Vec<Command> = Vec::new();
        let mut parsing_stacks = true;
        for line in lines.filter(|line| !line.is_empty()) {
            if parsing_stacks && line.chars().nth(1) == Some('1') {
                parsing_stacks = false;
                continue;
            }
            if parsing_stacks {
                for i in 0..9 {
                    let index = if i == 0 { 1 } else { 1 + 4 * i };
                    let character = line.chars().nth(index);
                    if let Some(ch) = character {
                        if ch != ' ' {
                            stacks[i].push(ch);
                        }
                    }
                }
                continue;
            }
            commands.push(line.parse().unwrap())
        }
        for stack in &mut stacks {
            stack.reverse();
        }
        Solution { stacks, commands }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 5;

    fn solve_first_part(&self) -> String {
        let mut stacks = self.stacks.clone();
        for command in self.commands.iter() {
            for _ in 0..command.quantity {
                let ch = stacks[command.from].pop().unwrap();
                stacks[command.to].push(ch);
            }
        }
        let mut result = String::new();
        for stack in stacks {
            if let Some(ch) = stack.last() {
                result.push(*ch);
            }
        }
        result
    }

    fn solve_second_part(&self) -> String {
        let mut stacks = self.stacks.clone();
        for command in self.commands.iter() {
            let mut to_move = Vec::new();
            for _ in 0..command.quantity {
                let ch = stacks[command.from].pop().unwrap();
                to_move.push(ch);
            }
            for ch in to_move.into_iter().rev() {
                stacks[command.to].push(ch);
            }
        }
        let mut result = String::new();
        for stack in stacks {
            if let Some(ch) = stack.last() {
                result.push(*ch);
            }
        }
        result
    }
}
