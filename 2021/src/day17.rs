use regex::Regex;

use crate::parsing::{FromContent, InputParser};

pub struct Solution {
    x1: i32,
    x2: i32,
    y1: i32,
    y2: i32,
}

impl FromContent for Solution {
    fn new(content: String) -> Self {
        let regex = Regex::new(r"^target area: x=(\d+)..(\d+), y=-(\d+)..-(\d+)").unwrap();
        let captures = regex.captures(&content).unwrap();
        let x1: i32 = captures[1].parse().unwrap();
        let x2: i32 = captures[2].parse().unwrap();
        let y1: i32 = -captures[3].parse::<i32>().unwrap();
        let y2: i32 = -captures[4].parse::<i32>().unwrap();
        Solution { x1, x2, y1, y2 }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 17;

    fn create() -> Self {
        InputParser::from_content()
    }

    fn solve_first_part(&self) -> String {
        let velocity_pairs = self.get_velocity_candidates();
        let mut max_y = 0;
        for pair in velocity_pairs {
            let mut current = (0, 0);
            let mut velocity = pair;
            let mut current_max_y = 0;
            while current.0 <= self.x2 && current.1 >= self.y1 {
                current.0 += velocity.0;
                current.1 += velocity.1;
                current_max_y = i32::max(current_max_y, current.1);
                velocity.0 = i32::max(0, velocity.0 - 1);
                velocity.1 = velocity.1 - 1;
                if current.0 >= self.x1
                    && current.0 <= self.x2
                    && current.1 >= self.y1
                    && current.1 <= self.y2
                {
                    max_y = i32::max(max_y, current_max_y);
                }
            }
        }
        max_y.to_string()
    }

    fn solve_second_part(&self) -> String {
        let velocity_pairs = self.get_velocity_candidates();
        let mut successful_pairs = 0;
        for pair in velocity_pairs {
            let mut current = (0, 0);
            let mut velocity = pair;
            while current.0 <= self.x2 && current.1 >= self.y1 {
                current.0 += velocity.0;
                current.1 += velocity.1;
                velocity.0 = i32::max(0, velocity.0 - 1);
                velocity.1 = velocity.1 - 1;
                if current.0 >= self.x1
                    && current.0 <= self.x2
                    && current.1 >= self.y1
                    && current.1 <= self.y2
                {
                    successful_pairs += 1;
                    break;
                }
            }
        }
        successful_pairs.to_string()
    }
}

impl Solution {
    fn get_velocity_candidates(&self) -> Vec<(i32, i32)> {
        let mut valid_vx: Vec<i32> = Vec::new();
        for vx in 1..=500 {
            let mut velocity = vx;
            let mut current_x = 0;
            while velocity > 0 {
                current_x += velocity;
                velocity = i32::max(0, velocity - 1);
                if current_x >= self.x1 && current_x <= self.x2 {
                    valid_vx.push(vx);
                    break;
                }
            }
        }
        let mut valid_vy: Vec<i32> = Vec::new();
        for vy in self.y1..=500 {
            let mut velocity = vy;
            let mut current_y = 0;
            while current_y >= self.y1 {
                current_y += velocity;
                velocity = velocity - 1;
                if current_y >= self.y1 && current_y <= self.y2 {
                    valid_vy.push(vy);
                    break;
                }
            }
        }
        let pairs: Vec<(i32, i32)> = valid_vx
            .iter()
            .flat_map(|&x| valid_vy.iter().map(move |&y| (x, y)))
            .collect();
        pairs
    }
}
