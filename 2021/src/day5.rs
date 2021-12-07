use crate::parsing::{FromLines, InputParser};

#[derive(Clone, Copy, PartialEq, Eq)]
struct Point {
    x: i32,
    y: i32,
}

struct Line {
    start: Point,
    end: Point,
}

pub struct Solution {
    lines: Vec<Line>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let result: Vec<Line> = lines
            .into_iter()
            .filter(|line| !line.is_empty())
            .map(|string| {
                let raw_points: Vec<Vec<i32>> = string
                    .split(" -> ")
                    .map(|raw_point| {
                        raw_point
                            .split(',')
                            .map(|num| num.parse().unwrap())
                            .collect()
                    })
                    .collect();
                let start = Point {
                    x: raw_points[0][0],
                    y: raw_points[0][1],
                };
                let end = Point {
                    x: raw_points[1][0],
                    y: raw_points[1][1],
                };
                Line {
                    start: start,
                    end: end,
                }
            })
            .collect();
        Solution { lines: result }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 5;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let result = self.count_overlapping_cells(|line| {
            let x_diff = i32::abs(line.start.x - line.end.x);
            let y_diff = i32::abs(line.start.y - line.end.y);
            x_diff == 0 || y_diff == 0
        });
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let result = self.count_overlapping_cells(|line| {
            let x_diff = i32::abs(line.start.x - line.end.x);
            let y_diff = i32::abs(line.start.y - line.end.y);
            x_diff == 0 || y_diff == 0 || x_diff == y_diff
        });
        result.to_string()
    }
}

impl Solution {
    fn count_overlapping_cells<F>(&self, line_selector: F) -> usize
    where
        F: Fn(&&Line) -> bool,
    {
        let straight_lines = self.lines.iter().filter(line_selector);
        let mut field = [[0; 1000]; 1000];
        for line in straight_lines {
            let x_diff = i32::signum(line.end.x - line.start.x);
            let y_diff = i32::signum(line.end.y - line.start.y);
            let mut current = line.start;
            while current != line.end {
                field[current.y as usize][current.x as usize] += 1;
                current.x += x_diff;
                current.y += y_diff;
            }
            field[current.y as usize][current.x as usize] += 1;
        }
        let result: usize = field
            .iter()
            .map(|&row| row.iter().filter(|&&cell| cell > 1).count())
            .sum();
        result
    }
}
