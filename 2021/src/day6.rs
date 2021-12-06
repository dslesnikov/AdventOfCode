use std::{fs::File, io::Read};

pub struct Solution {
    initial_state: Vec<i32>,
}

impl Solution {
    pub fn new() -> Solution {
        let mut input_file = File::open("input/6.txt").unwrap();
        let mut raw_input = String::new();
        input_file.read_to_string(&mut raw_input).unwrap();
        let state = raw_input
            .trim()
            .split(',')
            .map(|token| token.parse().unwrap())
            .collect();
        Solution {
            initial_state: state,
        }
    }
}

impl crate::Solution for Solution {
    type Output = String;

    fn solve_first_part(&self) -> String {
        self.simulate_growth(80).to_string()
    }

    fn solve_second_part(&self) -> String {
        self.simulate_growth(256).to_string()
    }
}

impl Solution {
    fn simulate_growth(&self, days: i32) -> u64 {
        let mut state: [u64; 9] = [0; 9];
        for &item in self.initial_state.iter() {
            state[item as usize] += 1;
        }
        for _ in 0..days {
            let mut new_state = [0; 9];
            for index in (0..=8).rev() {
                let producer_index = (index + 1) % 9;
                new_state[index] = state[producer_index];
            }
            new_state[6] += state[0];
            state = new_state;
        }
        state.iter().sum::<u64>()
    }
}