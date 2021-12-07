use create_macro_derive::CreateFromLines;

use crate::parsing::FromLines;

#[derive(CreateFromLines)]
pub struct Solution {
    input: Vec<u16>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let result: Vec<u16> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|content| u16::from_str_radix(content.as_str(), 2).unwrap())
            .collect();
        Solution { input: result }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 3;

    fn solve_first_part(&self) -> String {
        let mut statistics = [0; 12];
        for number in self.input.iter() {
            for bit in 0..12 {
                let bit_value = (number >> bit) & 1;
                statistics[bit] += bit_value;
            }
        }
        let mut gamma: u32 = 0;
        let length = self.input.len();
        for bit in 0..12 {
            let bit_stat = statistics[bit] as usize;
            if bit_stat > length - bit_stat {
                gamma |= 1 << bit;
            }
        }
        let epsilon = !gamma & 0b00000000000000000000111111111111;
        (gamma * epsilon).to_string()
    }

    fn solve_second_part(&self) -> String {
        let length = self.input.len();
        let mut oxigen_raiting_items: Vec<usize> = (0..length).collect();
        let mut co2_raiting_items: Vec<usize> = oxigen_raiting_items.clone();
        for bit in (0..12).rev() {
            if oxigen_raiting_items.len() != 1 {
                self.filter_by_bit_frequency(&mut oxigen_raiting_items, bit, true, 1);
            }
            if co2_raiting_items.len() != 1 {
                self.filter_by_bit_frequency(&mut co2_raiting_items, bit, false, 0);
            }
        }
        let oxigen_raiting = self.input[oxigen_raiting_items[0]] as u32;
        let co2_raiting = self.input[co2_raiting_items[0]] as u32;
        (co2_raiting * oxigen_raiting).to_string()
    }
}

impl Solution {
    fn filter_by_bit_frequency(
        &self,
        indexes: &mut Vec<usize>,
        bit: usize,
        most_common: bool,
        tie_breaker: u16,
    ) {
        let mut ones_count = 0;
        for &index in indexes.iter() {
            let bit_value = (self.input[index] >> bit) & 1;
            ones_count += bit_value;
        }
        let length = indexes.len();
        let ones_count = ones_count as usize;
        let comparison = ones_count.cmp(&(length - ones_count));
        let target_bit = if most_common { 1 } else { 0 };
        let target_bit = match comparison {
            std::cmp::Ordering::Less => 1 - target_bit,
            std::cmp::Ordering::Equal => tie_breaker,
            std::cmp::Ordering::Greater => target_bit,
        };
        for index in (0..length).rev() {
            let bit_value = (self.input[indexes[index]] >> bit) & 1;
            if bit_value != target_bit {
                indexes.remove(index);
            }
        }
    }
}
