use std::collections::HashSet;

use crate::parsing::{FromLines, InputParser};

#[derive(PartialEq, Eq, Clone, Copy, Hash)]
struct Point {
    x: usize,
    y: usize,
}

#[derive(Clone, Copy)]
enum Fold {
    Vertical(usize),
    Horizontal(usize),
}

struct Field {
    points: HashSet<Point>,
    height: usize,
    width: usize,
}

pub struct Solution {
    field: Field,
    folds: Vec<Fold>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let mut points: HashSet<Point> = HashSet::new();
        let mut index = 0;
        let length = lines.len();
        let mut max_x: usize = 0;
        let mut max_y: usize = 0;
        while index < length {
            let line = &lines[index];
            if line.is_empty() {
                break;
            }
            let coordinates: Vec<usize> = line
                .split(',')
                .map(|token| token.parse().unwrap())
                .collect();
            let point = Point {
                x: coordinates[0],
                y: coordinates[1],
            };
            max_x = usize::max(max_x, point.x);
            max_y = usize::max(max_y, point.y);
            points.insert(point);
            index += 1;
        }
        index += 1;
        let mut folds: Vec<Fold> = Vec::new();
        while index < length {
            let line = &lines[index];
            if line.is_empty() {
                break;
            }
            let split: Vec<&str> = line.split(' ').collect();
            let last = split[split.len() - 1];
            let folding: Vec<&str> = last.split('=').collect();
            let fold_index: usize = folding[1].parse().unwrap();
            let fold = match folding[0] {
                "x" => Fold::Vertical(fold_index),
                "y" => Fold::Horizontal(fold_index),
                &_ => panic!("Unknown folding direction"),
            };
            folds.push(fold);
            index += 1;
        }
        let field = Field {
            points,
            height: max_y + 1,
            width: max_x + 1,
        };
        Solution { field, folds }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 13;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let new_field = Self::fold(&self.field, self.folds[0]);
        let result = new_field.points.len();
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut field = &self.field;
        let mut new_field: Field;
        for &fold in self.folds.iter() {
            new_field = Self::fold(&field, fold);
            field = &new_field;
        }
        let mut result = String::new();
        result.push('\n');
        for row in 0..field.height {
            for col in 0..field.width {
                let token = if field.points.contains(&Point { x: col, y: row }) {
                    '#'
                } else {
                    '.'
                };
                result.push(token);
            }
            result.push('\n');
        }
        result
    }
}

impl Solution {
    fn fold(field: &Field, fold: Fold) -> Field {
        let (new_width, new_height) = match fold {
            Fold::Horizontal(_) => (field.width, field.height / 2),
            Fold::Vertical(_) => (field.width / 2, field.height),
        };
        let mut new_points = HashSet::<Point>::new();
        for point in field.points.iter() {
            let adjusted_point = match fold {
                Fold::Horizontal(axis) => Point {
                    x: point.x,
                    y: usize::min(point.y, 2 * axis - point.y),
                },
                Fold::Vertical(axis) => Point {
                    x: usize::min(point.x, 2 * axis - point.x),
                    y: point.y,
                },
            };
            new_points.insert(adjusted_point);
        }
        Field {
            points: new_points,
            height: new_height,
            width: new_width,
        }
    }
}
