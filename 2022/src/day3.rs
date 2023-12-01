use std::collections::HashSet;

use crate::FromLines;

pub struct Solution {
    bags: Vec<String>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        Solution {
            bags: lines.collect(),
        }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 3;

    fn solve_first_part(&self) -> String {
        self.bags
            .iter()
            .map(|bag| {
                let length = bag.len();
                let half = length / 2;
                let first: HashSet<char> = HashSet::from_iter((&bag[..half]).chars());
                let second: HashSet<char> = HashSet::from_iter((&bag[half..]).chars());
                let intersection: Vec<char> = first.intersection(&second).cloned().collect();
                return Self::get_priority(intersection[0]);
            })
            .sum::<i32>()
            .to_string()
    }

    fn solve_second_part(&self) -> String {
        self.bags
            .chunks(3)
            .map(|chunk| {
                let first: HashSet<char> = HashSet::from_iter(chunk[0].chars());
                let second: HashSet<char> = HashSet::from_iter(chunk[1].chars());
                let third: HashSet<char> = HashSet::from_iter(chunk[2].chars());
                let common: HashSet<char> = first.intersection(&second).cloned().collect();
                let result: Vec<char> = common.intersection(&third).cloned().collect();
                return Self::get_priority(result[0]);
            })
            .sum::<i32>()
            .to_string()
    }
}

impl Solution {
    fn get_priority(ch: char) -> i32 {
        if ch.is_ascii_lowercase() {
            let result = (ch as i32) - ('a' as i32) + 1;
            return result;
        }
        if ch.is_ascii_uppercase() {
            let result = (ch as i32) - ('A' as i32) + 27;
            return result;
        }
        return 0;
    }
}
