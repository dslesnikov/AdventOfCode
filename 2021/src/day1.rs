use std::{
    fs::File,
    io::{self, BufRead},
};

pub struct Solution {
    input: Vec<i32>,
}

impl Solution {
    pub fn new() -> Solution {
        let input_file = File::open("input/1.txt").expect("Failed to read input file");
        let file_lines = io::BufReader::new(input_file).lines();
        let result: Vec<i32> = file_lines
            .map(|line| line.expect("Failed to read a line"))
            .filter(|line| !line.is_empty())
            .map(|string| string.parse().expect("Failed to parse a number"))
            .collect();
        Solution { input: result }
    }
}

impl crate::Solution for Solution {
    type Output = String;

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
