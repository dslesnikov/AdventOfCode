use crate::parsing::{FromContent, InputParser};

pub struct Solution {
    crabs: Vec<i32>,
}

impl FromContent for Solution {
    fn new(content: String) -> Self {
        let mut numbers: Vec<i32> = content
            .trim()
            .split(',')
            .map(|token| token.parse().unwrap())
            .collect();
        numbers.sort();
        Solution { crabs: numbers }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 7;

    fn create() -> Self {
        InputParser::from_content()
    }

    fn solve_first_part(&self) -> String {
        let length = self.crabs.len();
        let median = if length % 2 == 1 {
            self.crabs[length / 2]
        } else {
            (self.crabs[length / 2] + self.crabs[length / 2 - 1]) / 2
        };
        let fuel = self
            .crabs
            .iter()
            .fold(0, |acc, &item| acc + (item - median).abs());
        fuel.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut result = i32::MAX;
        let min = self.crabs[0];
        let max = self.crabs[self.crabs.len() - 1];
        for candidate in min..=max {
            let total_fuel = self.crabs.iter().fold(0, |acc, &item| {
                let diff = (candidate - item).abs();
                acc + (diff + 1) * diff / 2
            });
            if total_fuel < result {
                result = total_fuel;
            }
        }
        result.to_string()
    }
}
