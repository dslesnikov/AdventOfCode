use crate::parsing::{FromLines, InputParser};

pub struct Solution {
    input: Vec<i32>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let numbers = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|string| string.parse().unwrap())
            .collect();
        Solution { input: numbers }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 1;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        self.input
            .windows(2)
            .filter(|window| window[1] > window[0])
            .count()
            .to_string()
    }

    fn solve_second_part(&self) -> String {
        let length = self.input.len();
        let mut result = 0;
        for index in 3..length {
            if self.input[index] > self.input[index - 3] {
                result += 1;
            }
        }
        result.to_string()
    }
}
