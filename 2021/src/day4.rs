use std::{
    collections::{HashMap, HashSet},
    fs::File,
    io::{self, BufRead},
};

const BOARD_SIZE: i32 = 5;

pub struct Solution {
    sequence: Vec<i32>,
    boards: Vec<HashMap<i32, (i32, i32)>>,
}

impl Solution {
    pub fn new() -> Solution {
        let input_file = File::open("input/4.txt").unwrap();
        let mut file_lines = io::BufReader::new(input_file).lines();
        let seqeunce: Vec<i32> = file_lines
            .next()
            .unwrap()
            .unwrap()
            .split(',')
            .map(|num| num.parse().unwrap())
            .collect();
        let mut boards: Vec<HashMap<i32, (i32, i32)>> = Vec::new();
        let mut current_board: HashMap<i32, (i32, i32)> = HashMap::new();
        let _ = file_lines.next().unwrap();
        let mut y = 0;
        for line in file_lines {
            let mut x = 0;
            let content = line.unwrap();
            if content.is_empty() {
                boards.push(current_board);
                current_board = HashMap::new();
                y = 0;
                continue;
            }
            let numbers = content
                .split(' ')
                .map(|token| token.trim())
                .filter(|token| !token.is_empty())
                .map(|num| num.parse().unwrap());
            for num in numbers {
                current_board.insert(num, (x, y));
                x += 1;
            }
            y += 1;
        }
        Solution {
            sequence: seqeunce,
            boards: boards,
        }
    }
}

impl crate::Solution for Solution {
    type Output = String;

    fn solve_first_part(&self) -> String {
        let winners = self.play_bingo();
        return winners[0].to_string();
    }

    fn solve_second_part(&self) -> String {
        let winners = self.play_bingo();
        return winners.last().unwrap().to_string();
    }
}

impl Solution {
    fn play_bingo(&self) -> Vec<i32> {
        let mut has_won: HashSet<usize> = HashSet::new();
        let mut winners: Vec<i32> = Vec::new();
        let mut marks = vec![0; self.boards.len()];
        for &lucky_number in self.sequence.iter() {
            for (index, board) in self.boards.iter().enumerate() {
                if !has_won.contains(&index) && board.contains_key(&lucky_number) {
                    let coordinates = board.get(&lucky_number).unwrap();
                    marks[index] |= 1 << (coordinates.0 + BOARD_SIZE * coordinates.1);
                    let row_mask = 0b11111 << (BOARD_SIZE * coordinates.1);
                    let col_mask = 0b100001000010000100001 << coordinates.0;
                    if marks[index] & row_mask == row_mask || marks[index] & col_mask == col_mask {
                        let score = self.calculate_score(index, marks[index], lucky_number);
                        winners.push(score);
                        has_won.insert(index);
                    }
                }
            }
        }
        winners
    }

    fn calculate_score(&self, board: usize, mask: i32, lucky: i32) -> i32 {
        let board = &self.boards[board];
        let mut result = 0;
        for (value, coordinates) in board.iter() {
            let current_mask = 1 << (coordinates.0 + BOARD_SIZE * coordinates.1);
            if mask & current_mask == 0 {
                result += value;
            }
        }
        result * lucky
    }
}
