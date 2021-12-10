use std::collections::VecDeque;

use crate::parsing::{FromLines, InputParser};

pub struct Solution {
    lines: Vec<String>,
}

enum LineType {
    Corrupted(u32),
    Incomplete(u64),
}

fn is_open_bracket(char: char) -> bool {
    char == '(' || char == '[' || char == '{' || char == '<'
}

fn pair_bracket(open: char) -> char {
    match open {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>',
        _ => panic!("Unsupported bracket type"),
    }
}

fn completion_cost(char: char) -> u64 {
    match char {
        '(' => 1,
        '[' => 2,
        '{' => 3,
        '<' => 4,
        _ => panic!("Unsupported bracket type"),
    }
}

fn mismatch_cost(char: char) -> u32 {
    match char {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => panic!("Unsupported bracket type"),
    }
}

fn get_line_type(line: &String) -> LineType {
    let mut stack: VecDeque<char> = VecDeque::new();
    for char in line.chars() {
        if is_open_bracket(char) {
            stack.push_back(char);
            continue;
        }
        let opening = stack.pop_back();
        if let Some(value) = opening {
            if char != pair_bracket(value) {
                let cost = mismatch_cost(char);
                return LineType::Corrupted(cost);
            }
        }
    }
    let completion_cost = stack
        .iter()
        .rev()
        .fold(0 as u64, |acc, &char| 5 * acc + completion_cost(char));
    LineType::Incomplete(completion_cost)
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let lines: Vec<String> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .collect();
        Solution { lines }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 10;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let mut total_mismatch_cost: u32 = 0;
        for line in self.lines.iter() {
            if let LineType::Corrupted(cost) = get_line_type(line) {
                total_mismatch_cost += cost;
            }
        }
        total_mismatch_cost.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut completion_costs: Vec<u64> = Vec::new();
        for line in self.lines.iter() {
            if let LineType::Incomplete(cost) = get_line_type(line) {
                completion_costs.push(cost);
            }
        }
        completion_costs.sort();
        completion_costs[completion_costs.len() / 2].to_string()
    }
}
