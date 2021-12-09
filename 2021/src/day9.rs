use std::{
    collections::{HashSet, VecDeque},
    ops::Index,
};

use crate::parsing::{FromLines, InputParser};

#[derive(PartialEq, Eq, Hash, Clone, Copy)]
struct Point {
    x: usize,
    y: usize,
}

pub struct Solution {
    field: Vec<Vec<u8>>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let result: Vec<Vec<u8>> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|string| {
                string
                    .chars()
                    .map(|char| char.to_digit(10).unwrap() as u8)
                    .collect()
            })
            .collect();
        Solution { field: result }
    }
}

impl Index<&Point> for Solution {
    type Output = u8;

    fn index(&self, index: &Point) -> &Self::Output {
        &self.field[index.y][index.x]
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 9;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let lowest_points = self.find_lowest_points();
        let total_risk_level: u32 = lowest_points
            .iter()
            .map(|point| self.field[point.y][point.x] as u32)
            .sum::<u32>()
            + (lowest_points.len() as u32);
        total_risk_level.to_string()
    }

    fn solve_second_part(&self) -> String {
        let lowest_points = self.find_lowest_points();
        let mut basin_sizes: Vec<u32> = Vec::new();
        let height = self.field.len();
        let width = self.field[0].len();
        for point in lowest_points {
            let mut visited: HashSet<Point> = HashSet::new();
            let mut queue: VecDeque<Point> = VecDeque::new();
            queue.push_back(point);
            let mut basin_size = 0;
            while queue.len() > 0 {
                basin_size += 1;
                let current = queue.pop_front().unwrap();
                let current_value = self[&current];
                let neighbours = self.get_neighbours(&current, height, width);
                let good_neighbours: Vec<Point> = neighbours
                    .into_iter()
                    .filter(|point| !visited.contains(point) && self[point] > current_value && self[point] != 9)
                    .collect();
                for neighbour in good_neighbours.into_iter() {
                    visited.insert(neighbour);
                    queue.push_back(neighbour);
                }
            }
            basin_sizes.push(basin_size);
        }
        basin_sizes.sort_by(|a, b| b.cmp(a));
        let result = basin_sizes[0] * basin_sizes[1] * basin_sizes[2];
        result.to_string()
    }
}

impl Solution {
    fn find_lowest_points(&self) -> Vec<Point> {
        let height = self.field.len();
        let width = self.field[0].len();
        let mut result: Vec<Point> = Vec::new();
        for row in 0..height {
            for col in 0..width {
                let current = Point { x: col, y: row };
                let neighbours = self.get_neighbours(&current, height, width);
                let current = self.field[row][col];
                let is_lowest = neighbours
                    .iter()
                    .all(|point| self.field[point.y][point.x] > current);
                if is_lowest {
                    result.push(Point { x: col, y: row });
                }
            }
        }
        result
    }

    fn get_neighbours(&self, point: &Point, height: usize, width: usize) -> Vec<Point> {
        let row = point.y;
        let col = point.x;
        let mut neighbours: Vec<Point> = Vec::new();
        if row > 0 {
            neighbours.push(Point { x: col, y: row - 1 });
        }
        if col > 0 {
            neighbours.push(Point { x: col - 1, y: row });
        }
        if row < height - 1 {
            neighbours.push(Point { x: col, y: row + 1 });
        }
        if col < width - 1 {
            neighbours.push(Point { x: col + 1, y: row });
        }
        neighbours
    }
}
