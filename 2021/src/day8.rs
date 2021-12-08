use std::collections::HashMap;

use crate::parsing::{FromLines, InputParser};

struct Encoding {
    signals: Vec<u8>,
    output: Vec<u8>,
}

pub struct Solution {
    entries: Vec<Encoding>,
}

fn create_signal_mapping(signals: &Vec<u8>) -> HashMap<u8, u8> {
    let mut by_length: HashMap<u8, Vec<u8>> = HashMap::new();
    for &signal in signals.iter() {
        let length = signal.count_ones() as u8;
        if by_length.contains_key(&length) {
            let vec = by_length.get_mut(&length).unwrap();
            vec.push(signal);
        } else {
            by_length.insert(length, vec![signal]);
        }
    }
    let one: u8 = by_length[&2][0];
    let four: u8 = by_length[&4][0];
    let seven: u8 = by_length[&3][0];
    let eight: u8 = by_length[&7][0];
    let top_bit = seven ^ one;
    let five_length = &by_length[&5];
    let top_middle_bottom = five_length[0] & five_length[1] & five_length[2];
    let middle_bit = four & top_middle_bottom;
    let bottom_bit = top_middle_bottom ^ (top_bit | middle_bit);
    let six_length = &by_length[&6];
    let bottom_right_bit = (seven & six_length[0] & six_length[1] & six_length[2]) ^ top_bit;
    let top_right_bit = one ^ bottom_right_bit;
    let top_left_bit = four ^ one ^ middle_bit;
    let bottom_left_bit = eight ^ four ^ top_bit ^ bottom_bit;
    let mut result = HashMap::new();
    let zero = eight ^ middle_bit;
    result.insert(zero, 0);
    result.insert(one, 1);
    let two = eight ^ top_left_bit ^ bottom_right_bit;
    result.insert(two, 2);
    let three = eight ^ top_left_bit ^ bottom_left_bit;
    result.insert(three, 3);
    result.insert(four, 4);
    let five = eight ^ top_right_bit ^ bottom_left_bit;
    result.insert(five, 5);
    let six = eight ^ top_right_bit;
    result.insert(six, 6);
    result.insert(seven, 7);
    result.insert(eight, 8);
    let nine = eight ^ bottom_left_bit;
    result.insert(nine, 9);
    result
}

fn str_to_num(str: &str) -> u8 {
    let mut result: u8 = 0;
    for char in str.chars() {
        let char_index = (char as u8) - b'a';
        result |= 1 << char_index;
    }
    result
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let encodings = lines
            .into_iter()
            .filter(|line| !line.is_empty())
            .map(|content| {
                let split: Vec<&str> = content.split(" | ").collect();
                let signals = split[0].split(' ').map(str_to_num).collect();
                let output = split[1].split(' ').map(str_to_num).collect();
                Encoding {
                    signals: signals,
                    output: output,
                }
            })
            .collect();
        Solution { entries: encodings }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 8;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let result: usize = self
            .entries
            .iter()
            .map(|enc| {
                enc.output
                    .iter()
                    .map(|&item| item.count_ones())
                    .filter(|&count| count == 2 || count == 3 || count == 4 || count == 7)
                    .count()
            })
            .sum();
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let result: u32 = self
            .entries
            .iter()
            .map(|encoding| {
                let signal = create_signal_mapping(&encoding.signals);
                let decoded_output = encoding
                    .output
                    .iter()
                    .map(|item| signal[item])
                    .fold(0 as u32, |acc, item| acc * 10 + item as u32);
                decoded_output
            })
            .sum();
        result.to_string()
    }
}
