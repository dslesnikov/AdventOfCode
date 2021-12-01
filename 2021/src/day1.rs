use std::{
    fs::File,
    io::{self, BufRead},
};

pub struct Solution {
    input: Vec<i32>,
}

impl Solution {
    pub fn new() -> Solution {
        let input_file = File::open("input/1.txt");
        let file_lines = match input_file {
            Err(_) => panic!("Failed to read input file"),
            Ok(file) => io::BufReader::new(file).lines(),
        };
        let mut result: Vec<i32> = Vec::new();
        for line in file_lines {
            match line {
                Err(_) => panic!("Failed to read line"),
                Ok(content) => {
                    if content.len() > 0 {
                        let number = content.parse::<i32>();
                        match number {
                            Err(_) => panic!("Failed to parse number"),
                            Ok(number) => result.push(number),
                        }
                    }
                }
            }
        }
        Solution { input: result }
    }
}

impl crate::Solution for Solution {
    type Output = String;

    fn solve_first_part(&self) -> String {
        let length = self.input.len();
        let mut result = 0;
        for index in 1..length {
            if self.input[index] > self.input[index - 1] {
                result += 1;
            }
        }
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let length = self.input.len();
        let mut current_sliding_window = self.input[0] + self.input[1] + self.input[2];
        let mut result = 0;
        for index in 3..length {
            let new_sliding_window =
                current_sliding_window - self.input[index - 3] + self.input[index];
            if new_sliding_window > current_sliding_window {
                result += 1;
            }
            current_sliding_window = new_sliding_window;
        }
        result.to_string()
    }
}
