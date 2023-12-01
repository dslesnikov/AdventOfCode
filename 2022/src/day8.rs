use crate::FromLines;

pub(crate) struct Solution {
    field: Vec<Vec<u8>>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let field = lines
            .map(|line| {
                line.chars()
                    .map(|char| char.to_digit(10).unwrap() as u8)
                    .collect()
            })
            .collect();
        Solution { field }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 8;

    fn solve_first_part(&self) -> String {
        let mut left_prefix_map = self.field.clone();
        let mut top_prefix_map = self.field.clone();
        let height = self.field.len();
        let width = self.field[0].len();
        for row in 0..height {
            for col in 0..width {
                if row > 0 {
                    top_prefix_map[row][col] =
                        std::cmp::max(top_prefix_map[row - 1][col], top_prefix_map[row][col]);
                }
                if col > 0 {
                    left_prefix_map[row][col] =
                        std::cmp::max(left_prefix_map[row][col - 1], left_prefix_map[row][col]);
                }
            }
        }
        let mut bottom_suffix_map = self.field.clone();
        let mut right_suffix_map = self.field.clone();
        for row in (0..height).rev() {
            for col in (0..width).rev() {
                if row < height - 1 {
                    bottom_suffix_map[row][col] =
                        std::cmp::max(bottom_suffix_map[row + 1][col], bottom_suffix_map[row][col]);
                }
                if col < width - 1 {
                    right_suffix_map[row][col] =
                        std::cmp::max(right_suffix_map[row][col + 1], right_suffix_map[row][col]);
                }
            }
        }
        let mut visible_trees = height * 2 + width * 2 - 4;
        for row in 1..(height - 1) {
            for col in 1..(width - 1) {
                let current = self.field[row][col];
                if current > left_prefix_map[row][col - 1]
                    || current > right_suffix_map[row][col + 1]
                    || current > top_prefix_map[row - 1][col]
                    || current > bottom_suffix_map[row + 1][col]
                {
                    visible_trees += 1;
                }
            }
        }
        visible_trees.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut max = u64::MIN;
        let height = self.field.len();
        let width = self.field[0].len();
        for row in 0..height {
            for col in 0..width {
                let score = self.get_scenic_scrore(row, col);
                max = std::cmp::max(max, score);
            }
        }
        max.to_string()
    }
}

impl Solution {
    fn get(&self, row: usize, col: usize) -> u8 {
        self.field[row][col]
    }

    fn get_scenic_scrore(&self, row: usize, col: usize) -> u64 {
        let mut top = 0;
        let mut bottom = 0;
        let mut left = 0;
        let mut right = 0;
        if row > 0 {
            top += 1;
            while row > top && self.get(row - top, col) < self.get(row, col) {
                top += 1;
            }
        }
        if col > 0 {
            left += 1;
            while col > left && self.get(row, col - left) < self.get(row, col) {
                left += 1;
            }
        }
        if row < self.field.len() - 1 {
            bottom += 1;
            while row + bottom < self.field.len() - 1
                && self.get(row + bottom, col) < self.get(row, col)
            {
                bottom += 1;
            }
        }
        if col < self.field[0].len() - 1 {
            right += 1;
            while col + right < self.field[0].len() - 1
                && self.get(row, col + right) < self.get(row, col)
            {
                right += 1;
            }
        }
        (top * right * left * bottom) as u64
    }
}
