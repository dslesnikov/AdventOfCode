use std::collections::HashMap;

use crate::parsing::{FromLines, InputParser};

pub struct Solution {
    template: Vec<char>,
    insertions: HashMap<(char, char), char>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let template = &lines[0];
        let insertion_iter = lines
            .iter()
            .skip(2)
            .filter(|&line| !line.is_empty())
            .map(|line| {
                let split: Vec<&str> = line.split(" -> ").collect();
                let left: Vec<char> = split[0].chars().collect();
                let right: Vec<char> = split[1].chars().collect();
                ((left[0], left[1]), right[0])
            });
        let mut insertions: HashMap<(char, char), char> = HashMap::new();
        for insertion in insertion_iter {
            insertions.insert(insertion.0, insertion.1);
        }
        Solution {
            template: template.chars().collect(),
            insertions,
        }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 14;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let result = self.perform_insertions(10);
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let result = self.perform_insertions(40);
        result.to_string()
    }
}

impl Solution {
    fn perform_insertions(&self, steps: i32) -> u64 {
        let mut character_stats: HashMap<char, u64> = HashMap::new();
        let mut pair_stats: HashMap<(char, char), u64> = HashMap::new();
        let len = self.template.len();
        for index in 1..len {
            let pair = (self.template[index - 1], self.template[index]);
            if !character_stats.contains_key(&pair.0) {
                character_stats.insert(pair.0, 0);
            }
            if !character_stats.contains_key(&pair.1) {
                character_stats.insert(pair.1, 0);
            }
            if !pair_stats.contains_key(&pair) {
                pair_stats.insert(pair, 0);
            }
            *character_stats.get_mut(&pair.0).unwrap() += 1;
            *pair_stats.get_mut(&pair).unwrap() += 1;
        }
        *character_stats.get_mut(&self.template[len - 1]).unwrap() += 1;
        for _ in 0..steps {
            let current_stats = pair_stats.clone();
            for pair in current_stats {
                if self.insertions.contains_key(&pair.0) {
                    let new = self.insertions[&pair.0];
                    if !character_stats.contains_key(&new) {
                        character_stats.insert(new, 0);
                    }
                    *character_stats.get_mut(&new).unwrap() += pair.1;
                    let left_pair: (char, char) = (pair.0 .0, new);
                    let right_pair: (char, char) = (new, pair.0 .1);
                    if !pair_stats.contains_key(&left_pair) {
                        pair_stats.insert(left_pair, 0);
                    }
                    if !pair_stats.contains_key(&right_pair) {
                        pair_stats.insert(right_pair, 0);
                    }
                    *pair_stats.get_mut(&left_pair).unwrap() += pair.1;
                    *pair_stats.get_mut(&right_pair).unwrap() += pair.1;
                    *pair_stats.get_mut(&pair.0).unwrap() -= pair.1;
                }
            }
        }
        let max = character_stats.iter().max_by_key(|&pair| pair.1).unwrap();
        let min = character_stats.iter().min_by_key(|&pair| pair.1).unwrap();
        max.1 - min.1
    }
}
