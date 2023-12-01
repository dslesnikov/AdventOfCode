use crate::FromLines;

pub(crate) struct Solution {
    bags: Vec<i32>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let mut bags = Vec::new();
        let mut current = 0;
        for line in lines {
            if line.is_empty() {
                bags.push(current);
                current = 0;
                continue;
            }
            let calorie: i32 = line.parse().unwrap();
            current += calorie;
        }
        bags.sort();
        Solution { bags }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 1;

    fn solve_first_part(&self) -> String {
        self.bags.last().unwrap().to_string()
    }

    fn solve_second_part(&self) -> String {
        self.bags.iter().rev().take(3).sum::<i32>().to_string()
    }
}
